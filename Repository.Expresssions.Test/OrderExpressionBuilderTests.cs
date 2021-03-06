using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using Repository.Model;
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
    public ILogger<OrderExpressionBuilder<OrderingBuilderTestModel>> TestModelOrderingLogger { get; private set; }
    public IOrderExpressionBuilder<OrderingBuilderTestModel> OrderExpressionBuilder { get; private set; }
    public IEnumerable<OrderingBuilderTestModel> Model { get; private set; }

    public OrderExpressionBuilderTests()
    {
        var dependencyBuilder = new DependencyBuilder();
        Configuration = dependencyBuilder.Configuration;
        TestModelOrderingLogger = dependencyBuilder.Logger<OrderExpressionBuilder<OrderingBuilderTestModel>>();
    }

    [TestInitialize]
    public void SetUp()
    {
        OrderExpressionBuilder = new OrderExpressionBuilder<OrderingBuilderTestModel>(
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
        var expressionById = OrderExpressionBuilder.CreateExpression(name);
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
