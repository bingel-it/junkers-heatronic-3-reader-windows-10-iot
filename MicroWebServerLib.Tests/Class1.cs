using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingelIT.MicroWebServerLib.Tests
{
    [TestClass]
    public class MyTestClass
    {

        //[TestMethod]
        public void TestDefaultWebServerPort()
        {
            var webServer = new BingelIT.MicroWebServerLib.WebServer();
            Assert.AreEqual(webServer.Port, 80);
        }
    }
}
