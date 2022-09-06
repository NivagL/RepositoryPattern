using Configuration.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Expressions.Test;

public class Level1
{
    public Level2 Level2 { get; set; }
    public Level3 Level3 { get; set; }
}

public class Level2
{
    public string Prop1 { get; set; }
}

public class Level3
{
    public List<Level4> Level4Collection { get; set; }
}

public class Level4
{
    public string Prop1 { get; set; }
}

[TestClass]
[TestCategory("Unit")]
public class QueryExpressionBuilderLevelsCollectionTest
{
    public IConfiguration Configuration { get; private set; }
    public ILogger<IQueryExpressionBuilder<Level1>> Logger { get; private set; }
    public IQueryExpressionBuilder<Level1> Builder { get; private set; }
    public List<Level1> Data { get; private set; }

    public QueryExpressionBuilderLevelsCollectionTest()
    {
        var dependencyBuilder = new UtilityBuilder();
        Configuration = dependencyBuilder.Configuration;
        Logger = dependencyBuilder.Logger<IQueryExpressionBuilder<Level1>>();
    }

    [TestInitialize]
    public void SetUp()
    {
        Builder = new QueryExpressionBuilder<Level1>(Configuration, Logger);
        Data = new List<Level1>()
        {
            new Level1()
            {
                Level2 = new Level2() { Prop1 = "Blah1" },
                Level3 = new Level3()
                {
                    Level4Collection = new List<Level4>()
                    {
                        new Level4() { Prop1 = "Blah2" },
                        new Level4() { Prop1 = "Blah3" },
                    }
                }
            }
        };
    }

    [TestMethod]
    public void TestExpression()
    {
        var list = Data.Where(x => x.Level3.Level4Collection.Any(c => c.Prop1 == "Blah3"));

        var query = new List<QueryObject>()
        {
            new QueryObject()
            {
                PropertyName = "Level3.Level4Collection.Prop1",
                Operator = "eq",
                Value = "Blah3"
            }
        };

        var exp = Builder.CreateExpression(query);
        var testList = Data.Where(exp.Compile());

        Assert.IsTrue(list.Count() == 1);
        Assert.IsTrue(testList.Count() == 1);

        var item1 = list.First();
        var item2 = testList.First();
        Assert.AreEqual(item1, item2);
    }
}
