using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CEWSP_Backend;
using System.Windows.Controls;

namespace LogTest
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void InitTestValid()
        {
            var textBlock = new TextBlock();
            Assert.IsTrue(Log.Init(textBlock));
        }

        [TestMethod]
        public void LogTestValid()
        {
            var textBlock = new TextBlock();
            Log.Init(textBlock);
            Log.LogInfo("This is an information message");
        }

        [TestMethod]
        public void LogTestInvalidSeverity()
        {
            var textBlock = new TextBlock();
            Log.Init(textBlock);
            Log.LogMessage("This is an information message", "SomNonExistingSeverity");
        }
    }
}
