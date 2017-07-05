
using ResxTranslatorRunner.Translators.Auth;
using ResxTranslatorRunner.Translators.Interfaces;
using System;

namespace ResxTranslatorRunner.Translators
{
    /// <summary>
    /// Translator class responsible for translating the text.
    /// </summary>
    public class AzureTranslatorService : ITranslatorService
    {
        private AzureAuthToken authProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AzureTranslatorService()
        {
            this.authProvider = new AzureAuthToken();
        }

        /// <summary>
        /// Unit test constructor
        /// </summary>
        public AzureTranslatorService(string key)
        {
            this.authProvider = new AzureAuthToken(key);
        }

        /// <summary>
        /// Downloads list of supported languages
        /// </summary>
        /// <returns></returns>
        public string[] GetSupportedLanguages()
        {
            try
            {
                var token = authProvider.Token();
                var translatorService = new ResxTranslatorRunner.TranslatorService.LanguageServiceClient();
                return translatorService.GetLanguagesForTranslate(token);
            }
            catch(Exception)
            {
                return new string[] { };
            }
        }

        /// <summary>
        /// Translates the given text in the given language.
        /// </summary>
        /// <param name="targetLanguage">Target language.</param>
        /// <param name="value">Text to be translated.</param>
        /// <returns>Translated value of the given text.</returns>
        public string Translate(string text, string targetLanguage, string sourceLanguage = "en")
        {
            string translatedValue = string.Empty;
            try
            {
                var token = authProvider.Token();
                var translatorService = new ResxTranslatorRunner.TranslatorService.LanguageServiceClient();
                translatedValue = translatorService.Translate(token, text, sourceLanguage, targetLanguage, "text/plain", "general", string.Empty);
            }
            catch (Exception ex)
            {
               throw ex;
            }
            return translatedValue;
        }
             
    }
}