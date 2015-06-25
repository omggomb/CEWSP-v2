using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CEWSP_Backend;

namespace LogTest
{
    [TestClass]
    public class CEVersionInfoTest
    {
        [TestMethod]
        public void TestValidPath()
        {
            var info = new CEVersionInfo();
            info.FromFile(@"E:\Dev\CRYENGINE\358\Bin64\Editor.exe");

            Assert.AreEqual(info.MajorVersion, 3);
            Assert.AreEqual(info.MinorVersion, 5);
            Assert.AreEqual(info.SubVersion, 8);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestValidPathNonExistent()
        {
            var info = new CEVersionInfo();
            info.FromFile(@"E:\Dev\CRYENGINE\358\Bin64\Editore.exe");
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidPath()
        {
            var info = new CEVersionInfo();
            info.FromFile(@"Some invalid path ??°!()/&%$");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void TestNullPath()
        {
            var info = new CEVersionInfo();
            info.FromFile(null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void TestNullString()
        {
            var info = new CEVersionInfo();
            info.FromString(null);
        }

        [TestMethod]
        public void TestValidString()
        {
            var info = new CEVersionInfo();
            info.FromString("3.43.56");

            Assert.AreEqual(info.MajorVersion, 3);
            Assert.AreEqual(info.MinorVersion, 43);
            Assert.AreEqual(info.SubVersion, 56);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidStringTooMuchDots()
        {
            var info = new CEVersionInfo();
            info.FromString("3.4.5.6");
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidStringLetters()
        {
            var info = new CEVersionInfo();
            info.FromString("3.4./5");
        }

        [TestMethod]
        public void TestAreEqual()
        {
            var info = new CEVersionInfo();
            info.FromString("3.4.5");

            var info2 = new CEVersionInfo();
            info2.FromString("3.4.5");

            Assert.IsTrue(info == info2);
        }

        [TestMethod]
        public void TestAreNotEqual()
        {
            var info = new CEVersionInfo();
            info.FromString("3.4.5");

            var info2 = new CEVersionInfo();
            info2.FromString("3.4.6");

            Assert.IsTrue(info != info2);
        }
    }
}
