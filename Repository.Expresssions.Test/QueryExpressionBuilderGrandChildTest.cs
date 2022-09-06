using Configuration.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Expressions.Test;

#region model

public class GrandChild2
{
    public Guid Id { get; set; }
    public int Age { get; set; }
}

public class Child2
{
    public Guid Id { get; set; }
    public GrandChild2 GrandChild { get; set; }
    public int Age { get; set; }
}

public class Parent2
{
    public Guid Id { get; set; }
    public GenderEnum Gender { get; set; }
    public Child2 Child { get; set; }
}

#endregion


[TestClass]
[TestCategory("Unit")]
public class QueryExpressionBuilderGrandChildTest
{
    public IConfiguration Configuration { get; private set; }
    public ILogger<IQueryExpressionBuilder<Parent2>> Logger { get; private set; }
    public List<Parent2> Parents { get; private set; }
    public List<QueryObject> Query1 { get; private set; }
    public List<QueryObject> Query2 { get; private set; }
    public IQueryExpressionBuilder<Parent2> Builder { get; private set; }

    public QueryExpressionBuilderGrandChildTest()
    {
        var dependencyBuilder = new UtilityBuilder();
        Configuration = dependencyBuilder.Configuration;
        Logger = dependencyBuilder.Logger<IQueryExpressionBuilder<Parent2>>();
    }

    [TestInitialize]
    public void SetUp()
    {
        Parents = new List<Parent2>()
        {
            new Parent2()
            {
                Id = Guid.NewGuid(),
                Gender = GenderEnum.Male,
            },
            new Parent2()
            {
                Id = Guid.NewGuid(),
                Gender = GenderEnum.Female,
            },
            new Parent2()
            {
                Id = Guid.NewGuid(),
                Gender = GenderEnum.Male,
            },
        };

        Query1 = new List<QueryObject>()
        {
            new QueryObject()
            {
                PropertyName = "Gender",
                Operator = "eq",
                Value = "Female"
            },
            new QueryObject()
            {
                PropertyName = "Child.GrandChild.Age",
                Operator = "eq",
                Value = "5"
            },
        };

        Query2 = new List<QueryObject>()
        {
            new QueryObject()
            {
                PropertyName = "Child.Age",
                Operator = "eq",
                Value = "25"
            },
        };

        Builder = new QueryExpressionBuilder<Parent2>(Configuration, Logger);
    }

    [TestMethod]
    public void TestExpressionChild2()
    {
        var list = Parents.Where(x => x.Gender == GenderEnum.Female && x.Child.GrandChild.Age == 5);
        try
        {
            var count = list.Count();
            Assert.Fail("Should have thrown"); 
        }
        catch { }

        var expression = Builder.CreateExpression(Query1);
        var testList = Parents.Where(expression.Compile());

        try
        {
            var count = testList.Count();
            Assert.Fail("Should have thrown");
        }
        catch { }
    }
}
