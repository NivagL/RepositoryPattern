using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;

namespace Repository.Expressions
{
    public class OrderExpressionBuilder<TValue> : IOrderExpressionBuilder<TValue>
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<IOrderExpressionBuilder<TValue>> Logger;

        public Type ExpressionType { get; private set; }
        public ParameterExpression ExpressionParam { get; private set; }

        public OrderExpressionBuilder(IConfiguration configuration, 
            ILogger<IOrderExpressionBuilder<TValue>> logger)
        {
            Configuration = configuration;
            Logger = logger;
            ExpressionType = typeof(TValue);
            ExpressionParam = Expression.Parameter(ExpressionType, "t");
        }

        public Expression<Func<TValue, object>> CreateExpression(string propertyNames, char propertyDelimeter = '|')
        {
            if (string.IsNullOrWhiteSpace(propertyNames))
                throw new Exception("Must provide a property name for the sort order");

            MemberExpression memberExpression = null;
            foreach (var propertyName in propertyNames.Split(propertyDelimeter))
            {
                Expression expressionToUse = memberExpression ?? (Expression)ExpressionParam;
                memberExpression = Expression.Property(expressionToUse, ConvertPropertyNameToPascal(propertyName));
            }

            Expression propertyExpression = Expression.Convert(memberExpression, typeof(object));
            Expression<Func<TValue, object>>
                complexExpression = Expression.Lambda<Func<TValue, object>>(propertyExpression, ExpressionParam);
            return complexExpression;
        }

        private string ConvertPropertyNameToPascal(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return string.Empty;
            }

            return char.ToUpper(propertyName[0]) + propertyName.Substring(1);
        }
    }
}
