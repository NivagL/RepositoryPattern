using Configuration.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Expressions.Test;

#region model

public class GrandChild1
{
    public Guid Id { get; set; }
    public int Age { get; set; }
}

public class Child1
{
    public Guid Id { get; set; }
    public GrandChild1 GrandChild { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
}

public enum GenderEnum
{
    Male,
    Female
}

public class Parent1
{
    public Guid Id { get; set; }
    public GenderEnum Gender { get; set; }
    public Child1 Child { get; set; }
}

#endregion


[TestClass]
[TestCategory("Unit")]
public class QueryExpressionBuilderParentChildTest
{
    private readonly IConfiguration Configuration;
    private readonly ILogger<IQueryExpressionBuilder<Parent1>> Logger;
    public List<Parent1> Parents { get; private set; }
    public List<QueryObject> Query1 { get; private set; }
    public List<QueryObject> Query2 { get; private set; }
    public IQueryExpressionBuilder<Parent1> Builder { get; private set; }

    public QueryExpressionBuilderParentChildTest()
    {
        var dependencyBuilder = new UtilityBuilder();
        Configuration = dependencyBuilder.Configuration;
        Logger = dependencyBuilder.Logger<IQueryExpressionBuilder<Parent1>>();
    }

    [TestInitialize]
    public void SetUp()
    {
        Parents = new List<Parent1>()
        {
            new Parent1()
            {
                Id = Guid.NewGuid(),
                Gender = GenderEnum.Male,
                Child = new Child1()
                {
                    Id = Guid.NewGuid(),
                    Age = 20,
                    GrandChild = new GrandChild1()
                    {
                        Id = Guid.NewGuid(),
                        Age = 2
                    },
                    Name = "Bob"
                },
            },
            new Parent1()
            {
                Id = Guid.NewGuid(),
                Gender = GenderEnum.Female,
                Child = new Child1()
                {
                    Id = Guid.NewGuid(),
                    Age = 30,
                    GrandChild = new GrandChild1()
                    {
                        Id = Guid.NewGuid(),
                        Age = 5
                    },
                    Name = "Mary"
                },
            },
            new Parent1()
            {
                Id = Guid.NewGuid(),
                Gender = GenderEnum.Male,
                Child = new Child1()
                {
                    Id = Guid.NewGuid(),
                    Age = 25,
                    GrandChild = new GrandChild1()
                    {
                        Id = Guid.NewGuid(),
                        Age = 10
                    },
                    Name = "Justin"
                },
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

        Builder = new QueryExpressionBuilder<Parent1>(Configuration, Logger);
    }

    [TestMethod]
    public void TestExpressionChild1()
    {
        var list = Parents.Where(x => x.Gender == GenderEnum.Female && x.Child.GrandChild.Age == 5);

        var expression = Builder.CreateExpression(Query1);
        var testList = Parents.Where(expression.Compile());

        Assert.IsTrue(list.Count() == 1);
        Assert.IsTrue(testList.Count() == 1);

        var item1 = list.First();
        var item2 = testList.First();
        Assert.AreEqual(item1, item2);
    }

    [TestMethod]
    public void TestExpressionChildNameLike()
    {
        var builder = new QueryExpressionBuilder<Parent1>(Configuration, Logger, false);
        var query = new List<QueryObject>()
        {
            new QueryObject()
            {
                PropertyName = "Child.Name",
                Operator = "like",
                Value = "Just"
            },
        };

        var expression = builder.CreateExpression(query);
        var test = Parents.Where(expression.Compile());
        var test1 = Parents.Where(x => x.Child.Name.Contains("Just"));
        Assert.IsTrue(test.Count() == test1.Count());
    }
}
