using CEWSP_Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

        [TestMethod]
        public void TestIsLessEqualWithEqual()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.10");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsTrue(info <= info2);
        }

        [TestMethod]
        public void TestIsLessEqualWithGreater()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.11");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsFalse(info <= info2);
        }

        [TestMethod]
        public void TestIsLessEqualWithSmaller()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.9");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsTrue(info <= info2);
        }

        [TestMethod]
        public void TestIsGreaterEqualWithEqual()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.10");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsTrue(info >= info2);
        }

        [TestMethod]
        public void TestIsGreaterEqualWithGreater()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.11");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsTrue(info >= info2);
        }

        [TestMethod]
        public void TestIsGreaterEqualWithSmaller()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.9");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsFalse(info >= info2);
        }

        [TestMethod]
        public void TestIsGreaterWithEqual()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.10");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsFalse(info > info2);
        }

        [TestMethod]
        public void TestIsGreaterWithSmaller()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.9");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsFalse(info > info2);
        }

        [TestMethod]
        public void TestIsGreaterWithGreater()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.11");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsTrue(info > info2);
        }

        [TestMethod]
        public void TestIsSmallerWithSmaller()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.9");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsTrue(info < info2);
        }

        [TestMethod]
        public void TestIsSmallerWithEqual()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.10");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsFalse(info < info2);
        }

        [TestMethod]
        public void TestIsSmallerWithGreater()
        {
            var info = new CEVersionInfo();
            info.FromString("3.6.11");

            var info2 = new CEVersionInfo();
            info2.FromString("3.6.10");

            Assert.IsFalse(info < info2);
        }

        [TestMethod]
        public void TestIsEaaSWithFreeSDK()
        {
            var info = new CEVersionInfo();
            info.FromString("3.5.8");

            Assert.IsFalse(info.IsEaaS);
        }

        [TestMethod]
        public void TestIsEaaSWithEaaS()
        {
            var info = new CEVersionInfo();
            info.FromString("3.8.3");

            Assert.IsTrue(info.IsEaaS);
        }

        [TestMethod]
        public void TestToString()
        {
            var info = new CEVersionInfo();
            info.FromString("3.8.3");

            Assert.AreEqual("3.8.3", info.ToString());
        }

        [TestMethod]
        public void TestGetHashCodeEqual()
        {
            var info = new CEVersionInfo();
            info.FromString("3.8.3");

            var info2 = new CEVersionInfo();
            info2.FromString("3.8.3");

            Assert.AreEqual(info.GetHashCode(), info2.GetHashCode());
        }

        [TestMethod]
        public void TestGetHashCodeNotEqual()
        {
            var info = new CEVersionInfo();
            info.FromString("3.8.5");

            var info2 = new CEVersionInfo();
            info2.FromString("3.8.3");

            Assert.AreNotEqual(info.GetHashCode(), info2.GetHashCode());
        }
    }
}