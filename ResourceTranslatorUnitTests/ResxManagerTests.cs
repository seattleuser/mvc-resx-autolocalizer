using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResxTranslatorRunner.Constants.Languages;
using System.Linq;
using ResxTranslatorRunner.Translators.Interfaces;
using ResxTranslatorRunner.Translators;

namespace ResourceTranslatorUnitTests
{
    [TestClass]
    public class ResxManagerTests
    {     

        [TestMethod]
        public void ResourceManagerEnd2End()
        {
            ResxTranslatorRunner.ResxManager.ResxManager.Execute(@"C:\Code\ResxManager\ResxTranslator\mvc-resx-autolocalizer\ResourceTranslatorUnitTests\TestFile", true);
        }

    }
}
