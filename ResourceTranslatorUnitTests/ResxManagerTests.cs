using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResxTranslatorRunner.Constants.Languages;
using System.Linq;
using ResxTranslatorRunner.Translators.Interfaces;
using ResxTranslatorRunner.Translators;
using System.IO;

namespace ResourceTranslatorUnitTests
{
    [TestClass]
    public class ResxManagerTests
    {
        string testfileName = "Resources.resx";
        string testFileFolderPath = @"C:\Code\ResxManager\ResxTranslator\mvc-resx-autolocalizer\ResourceTranslatorUnitTests\TestFile";

        [TestCleanup]
        public void CleanupFolder()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(testFileFolderPath);

            foreach (FileInfo file in di.GetFiles())
            {
                if (file.Name != testfileName)
                {
                    file.Delete();
                }
            }
        }

        [TestMethod]
        public void ResourceManagerEnd2End()
        {

            ResxTranslatorRunner.ResxManager.ResxManager.Execute(testFileFolderPath, true);
            System.IO.DirectoryInfo di = new DirectoryInfo(testFileFolderPath);
            Assert.IsTrue(di.EnumerateFiles().Count() > 1);

        }

        [TestMethod]
        public void ResourceManagerSecondRun()
        {
            ResxTranslatorRunner.ResxManager.ResxManager.Execute(testFileFolderPath, false);
            System.IO.DirectoryInfo di = new DirectoryInfo(testFileFolderPath);
            Assert.IsTrue(di.EnumerateFiles().Count() == 1);
        }
    }
}
