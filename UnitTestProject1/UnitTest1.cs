using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YnoteEx;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //MyWebClient client = new MyWebClient();
           long result = WebTool.GetCurrentTimestamp();
           Console.WriteLine(result);
           Console.Read();
        }
    }
}
