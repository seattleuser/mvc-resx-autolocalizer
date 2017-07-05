using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResxTranslatorRunner.Constants.Languages;
using System.Linq;
using ResxTranslatorRunner.Translators.Interfaces;
using ResxTranslatorRunner.Translators;

namespace ResourceTranslatorUnitTests
{
    [TestClass]
    public class TranslatorTests
    {
        private string AzureTestKey = "2367bca345014dd6858c9421fbfefa10";

        [TestMethod]
        public void GetLanguagesTests()
        {
            string[] lang = Languages.TranslatableCollection;
            Assert.IsTrue(lang.Any());
        }

        [TestMethod]
        public void GoogleTranslatorTest()
        {
            ITranslatorService service = new GoogleTranslatorService();
            string[] expected = Languages.TranslatableCollection;
            var actual = service.GetSupportedLanguages();
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public void GoogleTranslatorTestActualServiceCalls()
        {
            ITranslatorService service = new GoogleTranslatorService();
            var test = service.Translate("test", "es");
            var expected = "prueba";

            Assert.AreEqual(expected, test);
        }

        [TestMethod]
        public void AzureTranslatorTestActualServiceCalls()
        {
            ITranslatorService service = new AzureTranslatorService(AzureTestKey);
            var test = service.Translate("test", "es");
            var expected = "prueba";

            Assert.AreEqual(expected, test);
        }


        [TestMethod]
        public void AzureTranslatorTestSupportedLanguages()
        {
            ITranslatorService service = new AzureTranslatorService(AzureTestKey);
            var lang = service.GetSupportedLanguages();
            Assert.IsTrue(lang.Any());
        }
    }
}
