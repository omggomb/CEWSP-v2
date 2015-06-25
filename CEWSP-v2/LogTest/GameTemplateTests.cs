using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CEWSP_Backend;
using System.Windows.Controls;

namespace CEWSP_v2.Backend.Tests
{
    [TestClass()]
    public class GameTemplateTests
    {
        [TestInitialize()]
        public void Before()
        {
            var tb = new TextBlock();
            Log.Init(tb);
        }

        [TestMethod()]
        public void GameTemplateTest()
        {
            var template = new GameTemplate();
        }

        [TestMethod()]
        public void LoadFromFolderTest()
        {
            var template = new GameTemplate();

            Assert.IsTrue(template.LoadFromFolder(@".\GameTemplates\testTemplate"));
            Assert.AreEqual(template.Name, "testTemplate");
            Assert.AreEqual(template.DescFilePath, "defaultDescFile.txt");
            Assert.AreEqual(template.IconPath, "defaultTemplateImage.png");
            Assert.AreEqual(template.SupportedVersions.Count, 0);
            Assert.AreEqual(template.UnknownConfigNames.Count, 1);
            Assert.AreEqual(template.UnknownConfigNames[0], "unknownValue");
        }

        [TestMethod()]
        public void LoadFromFolderTestMoreVersions()
        {
            var template = new GameTemplate();
            var expectedVersions = new List<CEVersionInfo>();
            var version = new CEVersionInfo();
            version.FromString("3.4.5");
            expectedVersions.Add(version);
            version = new CEVersionInfo();
            version.FromString("3.8.1");
            expectedVersions.Add(version);


            Assert.IsTrue(template.LoadFromFolder(@".\GameTemplates\testTemplate2"));
            Assert.AreEqual(template.Name, "testTemplate2");
            Assert.AreEqual(template.DescFilePath, "defaultDescFile.txt");
            Assert.AreEqual(template.IconPath, "defaultTemplateImage.png");
            Assert.AreEqual(template.SupportedVersions.Count, 2);
            Assert.AreEqual(template.UnknownConfigNames.Count, 1);
            Assert.AreEqual(template.UnknownConfigNames[0], "unknownValue");

            for (int i = 0; i < expectedVersions.Count; ++i)
            {
                Assert.IsTrue(expectedVersions[i] == template.SupportedVersions[i]);
            }

        }
    }
}
