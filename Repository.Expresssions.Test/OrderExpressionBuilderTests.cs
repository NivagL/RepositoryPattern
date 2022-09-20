using Configuration.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Expressions.Test;

#region Model

public class OrderingBuilderTestModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

#endregion

[TestClass]
[TestCategory("Unit")]
public class OrderExpressionBuilderTests
{
    public IConfiguration Configuration { get; private set; }
    public ILogger<OrderExpression<OrderingBuilderTestModel>> TestModelOrderingLogger { get; private set; }
    public IOrderExpression<OrderingBuilderTestModel> OrderExpressionBuilder { get; private set; }
    public IEnumerable<OrderingBuilderTestModel> Model { get; private set; }

    public OrderExpressionBuilderTests()
    {
        var dependencyBuilder = new UtilityBuilder();
        Configuration = dependencyBuilder.Configuration;
        TestModelOrderingLogger = dependencyBuilder.Logger<OrderExpression<OrderingBuilderTestModel>>();
    }

    [TestInitialize]
    public void SetUp()
    {
        OrderExpressionBuilder = new OrderExpression<OrderingBuilderTestModel>(
            Configuration, TestModelOrderingLogger);

        Model = new List<OrderingBuilderTestModel>()
        {
            new OrderingBuilderTestModel()
            {
                Id = 1,
                Name = "C"
            },

            new OrderingBuilderTestModel()
            {
                Id = 2,
                Name = "B"
            },

            new OrderingBuilderTestModel()
            {
                Id = 3,
                Name = "A"
            },
        };
    }

    [TestMethod]
    [DataRow("Id", SortOrderEnum.Ascending)]
    [DataRow("Id", SortOrderEnum.Descending)]
    [DataRow("Name", SortOrderEnum.Ascending)]
    [DataRow("Name", SortOrderEnum.Descending)]
    public void TestOrderExpressionById(string name, SortOrderEnum sortOrder)
    {
        var expressionById = OrderExpressionBuilder.Create(name);
        var queryable = Model.AsQueryable<OrderingBuilderTestModel>();
        var orderedById = new List<OrderingBuilderTestModel>();

        orderedById.AddRange(sortOrder == SortOrderEnum.Ascending ? 
            queryable.OrderBy(expressionById)
                : 
            queryable.OrderByDescending(expressionById));

        var first = orderedById.First();
        if(name == "Id")
            Assert.IsTrue(first.Id == (sortOrder == SortOrderEnum.Ascending ? 1 : 3));

        if (name == "Name")
            Assert.IsTrue(first.Id == (sortOrder == SortOrderEnum.Ascending ? 3 : 1));
    }
}
