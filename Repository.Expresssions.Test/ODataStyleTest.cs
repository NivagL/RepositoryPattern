using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Expressions.Test;

#region model

public class TestModel
{
    public int Id { get; set; }
    public string Data { get; set; }
}

#endregion

[TestClass]
public class ODataStyleTest
{
    [TestMethod]
    public void TestSimpleFilter()
    {
        var baseUrl = "http://localhost:55734/contacts/query?";
        
        var filterUrl = "$filter=id eq 1";
        Func<TestModel, bool> filterLambda = x => x.Id == 1;


    }
}
