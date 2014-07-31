using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnoteEx;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace YnoteEx.Tests
{
    [TestClass()]
    public class WebToolTests
    {
        [TestMethod()]
        public void Md5Test()
        {
            string strIn = "232381204";
            string result = WebTool.Md5(strIn);
            Console.WriteLine(strIn +":"+result);
        }

        [TestMethod()]
        public void GetTimestampTest()
        {
            Console.WriteLine(WebTool.GetTimestamp());
        }

        [TestMethod()]
        public void GetCurrentTimestampTest()
        {
            Console.WriteLine(WebTool.GetCurrentTimestamp());
        }

        [TestMethod()]
        public void To64StringTest()
        {
            //string r = WebTool.To64String(WebTool.GetCurrentTimestamp());
            string r = WebTool.To64String(1406786251017);
            Console.WriteLine(r);
        }
    }
}
