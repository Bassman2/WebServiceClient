using WebServiceClient;

namespace WebServiceClientUnitTest
{
    [TestClass]
    public sealed class WebServiceCombineUrlUnitTest
    {
        //[TestMethod]
        //public void TestPathMethod()
        //{
        //    string url = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy");
        //    Assert.AreEqual("/demo/rest/xxxx/yyy", url);
        //}

        //[TestMethod]
        //public void TestQueryNothingMethod()
        //{
        //    string url = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy", ("list", null));
        //    Assert.AreEqual("/demo/rest/xxxx/yyy", url);
        //}

        //[TestMethod]
        //public void TestQueryCmmandMethod()
        //{
        //    string url = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy", ("list", ""));
        //    Assert.AreEqual("/demo/rest/xxxx/yyy?list", url);
        //}

        //[TestMethod]
        //public void TestQueryEmptyMethod()
        //{
        //    string url = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy", ("list", " "));
        //    Assert.AreEqual("/demo/rest/xxxx/yyy?list=", url);
        //}

        //[TestMethod]
        //public void TestQueryValueMethod()
        //{
        //    string url = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy", ("list", "test"), ("add", 6));
        //    Assert.AreEqual("/demo/rest/xxxx/yyy?list=test&add=6", url);
        //}

        //[TestMethod]
        //public void TestQueryBoolMethod()
        //{
        //    string urlTrue = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy", ("flag", true));
        //    string urlFalse = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy", ("flag", false));
        //    Assert.AreEqual("/demo/rest/xxxx/yyy?flag=true", urlTrue);
        //    Assert.AreEqual("/demo/rest/xxxx/yyy?flag=false", urlFalse);
        //}

        //[TestMethod]
        //public void TestQueryNullableBoolMethod()
        //{

        //    string urlTrue = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy", ("flag", (bool?)true));
        //    string urlFalse = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy", ("flag", (bool?)false));
        //    string urlNull = WebService.CombineUrl("demo", "rest", "//xxxx//", "yyy", ("flag", (bool?)null));
        //    Assert.AreEqual("/demo/rest/xxxx/yyy?flag=true", urlTrue);
        //    Assert.AreEqual("/demo/rest/xxxx/yyy?flag=false", urlFalse);
        //    Assert.AreEqual("/demo/rest/xxxx/yyy", urlNull);
        //}
    }
}
