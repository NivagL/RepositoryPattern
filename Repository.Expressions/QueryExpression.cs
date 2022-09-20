using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.Expressions
{
    public class QueryExpression<TValue> : IQueryExpression<TValue>
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<IQueryExpression<TValue>> Logger;
        public Type ExpressionType { get; private set; }
        public ParameterExpression ExpressionParam { get; private set; }
        public bool EfCoreLike { get; private set; }

        public QueryExpression(IConfiguration configuration, 
            ILogger<IQueryExpression<TValue>> logger, 
            bool efCorelike = true)
        {
            Configuration = configuration;
            Logger = logger;
            ExpressionType = typeof(TValue);
            ExpressionParam = Expression.Parameter(ExpressionType, "t");
            EfCoreLike = efCorelike;
        }

        /// <summary>
        /// Given query objects break them down to a single Linq expresion for type T
        /// queryObject: query[0] AND query[1] AND query[2]
        /// The property name and value may have many elements within them
        /// Many Properties => Or across the properties
        /// Many Values => Or across the values
        /// So AND between queries OR within queries
        /// Operator: eq, lt, gt, lte, gte, like
        /// </summary>
        /// <param name="queryObjects"></param>
        /// <returns></returns>
        public Expression<Func<TValue, bool>> Create(IEnumerable<QueryObject> queryObjects)
        {
            Expression expression = null;
            foreach (var query in queryObjects)
            {
                var properties = query.PropertyName.Split("|");
                if (properties.Length == 1)
                {
                    var valueExpression = ValueExpression(properties[0], query.Operator, query.Value);
                    expression = expression == null ? valueExpression : Expression.AndAlso(expression, valueExpression);
                }
                else
                {
                    foreach (var property in properties)
                    {
                        var valueExpression = ValueExpression(property, query.Operator, query.Value);
                        expression = expression == null ? valueExpression : Expression.OrElse(expression, valueExpression);
                    }
                }
            }

            var lambda = Expression.Lambda<Func<TValue, bool>>(expression, ExpressionParam);
            return lambda;
        }

        /// <summary>
        /// A single property (although it may have Child.Parent form) with potentially many values (a|b|c)
        /// When there are many values they are within a string with a | delimeter
        /// String values are cast to the type of the comparison element - the propertyname
        /// </summary>
        /// <param name="property"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Expression ValueExpression(string property, string op, object value)
        {
            string[] values = null;

            if (value != null)
                values = value.ToString().Split('|');

            if (value == null || values.Length == 1)
            {
                var propertyExpression = PropertyExpression(property, op, value);
                return propertyExpression;
            }
            else
            {
                Expression expression = null;
                foreach (var val in values)
                {
                    var propertyExpression = PropertyExpression(property, op, val);
                    expression = expression == null ? propertyExpression : Expression.Or(expression, propertyExpression);
                }
                //var lambda = Expression.Lambda<Func<T, bool>>(expression, ExpressionParam);
                return expression;
            }
        }

        /// <summary>
        /// Single property & value only at this level
        /// The property may refer to a child element in the structure Parent.Child.GrandChild.Age = 
        /// The penultimate elements may be a collection, in which case the final element refers to the type within the collection 
        /// We cannot currently have sub elements 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Expression PropertyExpression(string property, string op, object value)
        {
            var propertyElements = property.Split(".");
            if (propertyElements.Length == 1)
            {
                var propertyName = ConvertPropertyNameToPascal(property);

                MemberExpression memberExpression = GetMemberExpression(propertyName);
                var propertyType = typeof(TValue).GetProperty(propertyName).PropertyType;
                //ConstantExpression valueConstant = GetValueConstant(value, propertyType);
                Expression valueConstant = GetValueConstant(value, propertyType);
                return GetExpression(op, memberExpression, valueConstant);
            }
            else //Parent.Child.GrandChild.Age = 2 or Parent.ChildCollection.Age = 2 
            {
                //TODO **GL** this bit is fugly...refactor it! allow hierarchies of collections?

                Expression body = ExpressionParam;
                var types = GetTypes(property);
                var lastItem = types.ElementAt(types.Count - 1);
                //var parentType = lastItem.Item3;
                var propertyType = lastItem.Item2;
                //var valueName = lastItem.Item1;

                for (int i = 0; i < types.Count; i++)
                {
                    var part = types[i];

                    if (!part.Item4) //Not enumerable just add to the property name
                    {
                        body = Expression.PropertyOrField(body, part.Item1);
                    }
                    else //It is an Enumerable type, leaf is child...or failure
                    {
                        var child = types[i + 1];

                        var childParam = Expression.Parameter(child.Item3, "l");
                        var childProperty = Expression.PropertyOrField(childParam, child.Item1);
                        var childValue = GetValueConstant(value, child.Item2);
                        //var childEqual = Expression.Equal(childProperty, childValue);
                        var childExpr = GetExpression(op, childProperty, childValue);
                        var childLambda = Expression.Lambda(childExpr, childParam);

                        var collProp = Expression.Property(body, part.Item1);
                        return Expression.Call(typeof(Enumerable), "Any", new[] { child.Item3 },
                            collProp, childLambda);
                    }
                }

                //ConstantExpression valueConstant = GetValueConstant(value, propertyType);
                Expression valueConstant = GetValueConstant(value, propertyType);
                return GetExpression(op, body, valueConstant);
            }
        }

        /// <summary>
        /// Get the expression for the property/member being compared
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private MemberExpression GetMemberExpression(string property)
        {
            try
            {
                return Expression.PropertyOrField(ExpressionParam, property);
            }
            catch (ArgumentException ex)
            {
                Logger.LogError("ExpressionBuilder {0} error no such property {0} {1}/{2}",
                    typeof(TValue).Name, ex.ParamName, ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Get the constant expression for the value we are comparing
        /// We cast to the type of the property that is being compared - the propertyName
        /// Cast cases include | for Or values, Date strings, nullable types etc
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        //private ConstantExpression GetValueConstant(object value, Type propertyType)
        private Expression GetValueConstant(object value, Type propertyType)
        {
            if (value == null)
            {
                // check if type can have a null value
                if (Nullable.GetUnderlyingType(propertyType) != null || propertyType == typeof(string))
                {
                    return Expression.Convert(Expression.Constant(value), propertyType);
                }
                else
                {
                    throw new Exception("Error creating expression : The property (type: " + propertyType.Name + ") is not a nullable field therefore the value cannot be null");
                }
            }

            var valueType = value.GetType();
            //ConstantExpression valueConstant = null;
            Expression valueConstant;
            if (valueType != propertyType)
            {
                Object converted;
                try
                {
                    if (propertyType.IsEnum)
                    {
                        converted = Enum.Parse(propertyType, value.ToString());
                        valueConstant = Expression.Constant(converted);
                    }
                    else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        value = string.IsNullOrEmpty(value.ToString()) ? null : value;

                        if (value != null && propertyType == typeof(Nullable<Guid>))
                        {
                            valueConstant = Expression.Convert(Expression.Constant(new Guid(value.ToString())), propertyType);
                        }
                        else if (value != null && propertyType == typeof(Nullable<Int32>) &&
                            (valueType == typeof(Nullable<Int64>) || valueType == typeof(Int64)))
                        {
                            //down cast the int 64 to 32 if needed, json serialiser defaults to 64
                            var convertedtoint32 = Convert.ToInt32(value);
                            valueConstant = Expression.Convert(Expression.Constant(convertedtoint32), propertyType);
                        }
                        else if (value != null)
                        {
                            TypeConverter conv = TypeDescriptor.GetConverter(propertyType);
                            converted = conv.ConvertFrom(value);
                            valueConstant = Expression.Constant(converted, propertyType);
                        }
                        else
                            valueConstant = Expression.Convert(Expression.Constant(value), propertyType);
                        //valueConstant = (ConstantExpression)unary;
                    }
                    else if (propertyType == typeof(Guid))
                    {
                        valueConstant = Expression.Constant(new Guid(value.ToString()), propertyType);
                    }
                    else
                    {
                        converted =  Convert.ChangeType(value, propertyType);
                        valueConstant = Expression.Constant(converted);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("ExpressionBuilder {0} error invalid cast {0} {1}/{2}",
                        typeof(TValue).Name, propertyType.Name, ex.Message, ex.StackTrace);
                    throw;
                }
            }
            else
            {
                valueConstant = Expression.Constant(value);
            }

            return valueConstant;
        }

        /// <summary>
        /// Get the expression comparing the constant with the property/member expression
        /// </summary>
        /// <param name="op"></param>
        /// <param name="propertyExp"></param>
        /// <param name="valueConstant"></param>
        /// <returns></returns>
        private Expression GetExpression(string op, Expression propertyExp, Expression valueConstant)
        //private Expression GetExpression(string op, Expression propertyExp, ConstantExpression valueConstant)
        {
            switch (op.ToLower())
            {
                case "ne":
                    return Expression.NotEqual(propertyExp, valueConstant);
                case "eq":
                    return Expression.Equal(propertyExp, valueConstant);
                case "lt":
                    return Expression.LessThan(propertyExp, valueConstant);
                case "gt":
                    return Expression.GreaterThan(propertyExp, valueConstant);
                case "gte":
                    return Expression.GreaterThanOrEqual(propertyExp, valueConstant);
                case "lte":
                    return Expression.LessThanOrEqual(propertyExp, valueConstant);
                case "like":
                    if (EfCoreLike)
                    {
                        var likeMethod = typeof(DbFunctionsExtensions).GetMethods()
                            .Where(p => p.Name == "Like").First();

                        return Expression.Call(likeMethod, new[]
                        {
                                Expression.Property(null, typeof(EF).GetProperty("Functions")),
                                propertyExp,
                                valueConstant
                        });
                    }
                    else
                    {
                        var containsMethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                        return Expression.Call(propertyExp, containsMethod, valueConstant);
                    }
            }
            Logger.LogError("ExpressionBuilder {0} error unknown operator {0}",
                typeof(TValue).Name, propertyExp);
            throw new Exception("Unrecognised operator");
        }

        private string ConvertPropertyNameToPascal(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return string.Empty;
            }

            return char.ToUpper(propertyName[0]) + propertyName.Substring(1);
        }

        private bool IsEnumerable(Type type)
        {
            //TODO **GL** get the full list? use the type IsEnumerable...didn't seem to work
            if (type.IsGenericType && (
                type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                || type.GetGenericTypeDefinition() == typeof(List<>)
                || type.GetGenericTypeDefinition() == typeof(ISet<>)
                || type.GetGenericTypeDefinition() == typeof(ICollection<>)
                || type.GetGenericTypeDefinition() == typeof(IList<>)
                || type.GetGenericTypeDefinition() == typeof(Collection<>)

                ))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return the types in the hierarchical property name Parent.Child.GrandChild
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>List of Tuple of name, type, parent type, collection</returns>
        private List<Tuple<string, Type, Type, bool>> GetTypes(string propertyName)
        {
            //Get the types of each element of the name
            var partNames = propertyName.Split(".");
            var partTypes = new List<Tuple<string, Type, Type, bool>>(partNames.Length - 1);
            var parentType = typeof(TValue);
            foreach (var partName in partNames)
            {
                var pascalPartName = ConvertPropertyNameToPascal(partName);
                var prop = parentType.GetProperty(pascalPartName);
                if (IsEnumerable(prop.PropertyType))
                {
                    //We need the T in the IEnumerable<T>...
                    partTypes.Add(new Tuple<string, Type, Type, bool>(pascalPartName, prop.PropertyType, parentType, true));
                    parentType = prop.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    partTypes.Add(new Tuple<string, Type, Type, bool>(pascalPartName, prop.PropertyType, parentType, false));
                    parentType = prop.PropertyType;
                }
            }
            return partTypes;
        }
    }
}
