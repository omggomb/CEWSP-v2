using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CEWSP_Backend.Tests
{
    [TestClass()]
    public class UtillitiesTests
    {
        [TestMethod()]
        public void GetCERootFromEditorTestFreeSDK()
        {
            var dir = Utillities.GetCERootFromEditor(@"E:\Dev\CRYENGINE\358\Bin64\Editor.exe");
            Assert.AreEqual("358", dir.Name);
        }

        [TestMethod()]
        public void GetCERootFromEditorTestEaaS()
        {
            var dir = Utillities.GetCERootFromEditor(@"E:\Dev\CRYENGINE\EaaS-Steam\bin\win_x64\Editor.exe");
            Assert.AreEqual("EaaS-Steam", dir.Name);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCERootFromEditorTestNullString()
        {
            var dir = Utillities.GetCERootFromEditor(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCERootFromEditorTestEmptyString()
        {
            var dir = Utillities.GetCERootFromEditor("  ");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCERootFromEditorTestGarbageString()
        {
            var dir = Utillities.GetCERootFromEditor("Hello");
        }

        [TestMethod()]
        public void TrimLmbrExeOutputValidString()
        {
            var testOutput = "lyzard: SomeOutput\nlyzard: Other output\nlyzard: ";

            var result = Utillities.TrimLmbrExeOutput(testOutput);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("SomeOutput", result[0]);
            Assert.AreEqual("Other output", result[1]);
            Assert.AreEqual("", result[2]);
        }

        [TestMethod]
        public void TrimLmbrExeOutputNullOrEmpty()
        {
            string stringOne = null;
            var stringTwo = "";

            var restult = Utillities.TrimLmbrExeOutput(stringOne);

            Assert.AreEqual(0, restult.Count);

            restult = Utillities.TrimLmbrExeOutput(stringTwo);
            Assert.AreEqual(0, restult.Count);
        }
    }
}