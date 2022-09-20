using Configuration.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.Expressions.Test;


#region Model

public class Fish
{
    public string Type { get; set; }
    public int Weight { get; set; }
}

public class Catch
{
    public List<Fish> Morning { get; set; }
    public List<Fish> Day { get; set; }
    public List<Fish> Night { get; set; }
}


#endregion


[TestClass]
[TestCategory("Unit")]
public class QueryExpressionBuilderFishyCollectionTest
{
    public List<Fish> Fishes { get; private set; }
    public List<Catch> Catches { get; private set; }
    public Expression<Func<Fish, bool>> LambdaFish { get; private set; }

    public IConfiguration Configuration { get; private set; }
    public ILogger<IQueryExpression<Catch>> Logger { get; private set; }
    public IQueryExpression<Catch> Builder { get; private set; }

    public QueryExpressionBuilderFishyCollectionTest()
    {
        var dependencyBuilder = new UtilityBuilder();
        Configuration = dependencyBuilder.Configuration;
        Logger = dependencyBuilder.Logger<IQueryExpression<Catch>>();
    }


    [TestInitialize]
    public void SetUp()
    {
        Fishes = new List<Fish>()
            {
                new Fish() { Type = "Trout", Weight = 2 },
                new Fish() { Type = "Salmon", Weight = 1 },
                new Fish() { Type = "Bass", Weight = 4 },
                new Fish() { Type = "Trout", Weight = 1 },
                new Fish() { Type = "Salmon", Weight = 2 },
            };

        Catches = new List<Catch>()
            {
                new Catch()
                {
                    Morning = new List<Fish>()
                    {
                        new Fish()
                        {
                            Type = "Trout", Weight = 1
                        }
                    },
                    Day = new List<Fish>()
                    {
                        new Fish()
                        {
                            Type = "Snapper", Weight = 2
                        }
                    },
                    Night = new List<Fish>()
                    {
                        new Fish()
                        {
                            Type = "Salmon", Weight = 3
                        }
                    },
                },
                new Catch()
                {
                    Morning = new List<Fish>()
                    {
                        new Fish()
                        {
                            Type = "Cat", Weight = 4
                        }
                    },
                    Day = new List<Fish>()
                    {
                        new Fish()
                        {
                            Type = "Dog", Weight = 12
                        }
                    },
                    Night = new List<Fish>()
                    {
                        new Fish()
                        {
                            Type = "Shark", Weight = 20
                        }
                    },
                },
            };

        var pFish = Expression.Parameter(typeof(Fish), "f");
        var leftFish = Expression.Property(pFish, "Weight");
        var rightFish = Expression.Constant(2);
        var eqFish = Expression.Equal(leftFish, rightFish);
        LambdaFish = Expression.Lambda<Func<Fish, bool>>(eqFish, pFish);

        Builder = new QueryExpression<Catch>(Configuration, Logger);
    }

    [TestMethod]
    public void TestExpression()
    {
        //Date example
        var dateStr = Expression.Parameter(typeof(string));
        var asDateTime = Expression.Call(typeof(DateTime), "Parse", null, dateStr); // calls static method "DateTime.Parse"
        var fmtExpr = Expression.Constant("MM/dd/yyyy");
        var dateBody = Expression.Call(asDateTime, "ToString", null, fmtExpr); // calls instance method "DateTime.ToString(string)"
        var lambdaExpr = Expression.Lambda<Func<string, string>>(dateBody, dateStr);
        var method = lambdaExpr.Compile();
        method("05/12/2012 12:00:00"); // "05/12/2012"
    }


    [TestMethod]
    public void ExpressionFishyWhereAny()
    {
        //Where as lambda
        var d1 = Fishes.Where(f => f.Weight == 2);
        var d2 = Fishes.Where(LambdaFish.Compile());
        Assert.IsTrue(d1.Count() == d2.Count());
        Assert.IsTrue(d1.First() == d2.First());


        var pwFishes = Expression.Parameter(typeof(IEnumerable<Fish>), "fishes");
        var fwAnyFishes = Expression.Call(typeof(Enumerable), "Where", new[] { typeof(Fish) },
            pwFishes, LambdaFish);
        var lwAnyFishes = Expression.Lambda<Func<IEnumerable<Fish>, IEnumerable<Fish>>>(fwAnyFishes, pwFishes);
        var d3 = lwAnyFishes.Compile().Invoke(Fishes);
        Assert.IsTrue(d1.Count() == d3.Count());
        Assert.IsTrue(d1.First() == d3.First());


        //Any as lambda
        var d4 = Fishes.Any(f => f.Weight == 2);
        var d5 = Fishes.Any(LambdaFish.Compile());
        Assert.IsTrue(d4 == d5);

        var paFishes = Expression.Parameter(typeof(IEnumerable<Fish>), "fishes");
        var faAnyFishes = Expression.Call(typeof(Enumerable), "Any", new[] { typeof(Fish) },
            paFishes, LambdaFish);
        var laAnyFishes = Expression.Lambda<Func<IEnumerable<Fish>, bool>>(faAnyFishes, paFishes);
        var d6 = laAnyFishes.Compile().Invoke(Fishes);
        Assert.IsTrue(d4 == d6);
    }

    //[TestMethod]
    //public void ExpressionFishyMainCourseChildrenWhereAny()
    //{
    //    //Select based on child properties
    //    var d1 = Catches.Where(c => c.Day.Any(f => f.Weight == 2));

    //    //This would be represented as a property value of Day.Weight == 2 ...as Day = IEnumerable<Fish>, Fish.Weight == 2 (LambdaFish)

    //    //Day property of the Catch - the collection property
    //    //LambdaFish is the child lambda
    //    var catchParam = Expression.Parameter(typeof(Catch), "c");
    //    var dayProperty = Expression.Property(catchParam, "Day");
    //    var anyCatches = Expression.Call(typeof(Enumerable), "Any", new[] { typeof(Fish) },
    //        dayProperty, LambdaFish);
    //    var catchLambda = Expression.Lambda<Func<Catch, bool>>(anyCatches, catchParam);

    //    //Equivalent on the builder...
    //    var l = PredicateBuilder.BuildCollectionAny<Catch, Fish>("Day", LambdaFish);

    //    //The Where across all Catches - the top level collection
    //    var allCatches = Expression.Parameter(typeof(IEnumerable<Catch>), "catches");
    //    var allCatchesWhere = Expression.Call(typeof(Enumerable), "Where", new[] { typeof(Catch) },
    //        allCatches, catchLambda);
    //    var allCatchesLambda = Expression.Lambda<Func<IEnumerable<Catch>, IEnumerable<Catch>>>(allCatchesWhere, allCatches);

    //    //Test it...
    //    var d2 = allCatchesLambda.Compile().Invoke(Catches);

    //    Assert.IsTrue(d1.Count() == d2.Count());
    //    Assert.IsTrue(d1.First() == d2.First());
    //}

    [TestMethod]
    public void ExpressionFishyMainCourseChildrenViaBuilder()
    {
        var queries = new List<QueryObject>()
            {
                new QueryObject()
                {
                    PropertyName = "Day.Weight",
                    Operator = "eq",
                    Value = "2"
                },
                new QueryObject()
                {
                    PropertyName = "Day.Type",
                    Operator = "eq",
                    Value = "Snapper"
                },
            };

        var expression = Builder.Create(queries);
        var list = Catches.Where(expression.Compile());
    }

    [TestMethod]
    //TODO This needs foxing for like queries against children
    public void ExpressionFishyMainCourseChildrenViaBuilderLike()
    {
        var builder = new QueryExpression<Catch>(Configuration, Logger, false);
        var queries = new List<QueryObject>()
        {
            new QueryObject()
            {
                PropertyName = "Day.Weight",
                Operator = "eq",
                Value = "2"
            },
            new QueryObject()
            {
                PropertyName = "Day.Type",
                Operator = "like",
                Value = "Snap"
            },
        };

        var expression = builder.Create(queries);
        var list = Catches.Where(expression.Compile());
        Assert.IsTrue(list.Any());  //This is broken
    }
}
