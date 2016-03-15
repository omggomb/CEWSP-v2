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
    }
}