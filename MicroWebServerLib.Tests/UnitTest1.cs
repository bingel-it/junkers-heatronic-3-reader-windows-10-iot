using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MicroWebServerLib.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var webServer = new BingelIT.MicroWebServerLib.WebServer();
            Assert.AreEqual(webServer.Port, (uint)80);
        }
    }
}
