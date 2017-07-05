
using ResxTranslatorRunner.Translators.Interfaces;
using ResxTranslatorRunner.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text;
using ResxTranslatorRunner.Constants.Languages;

namespace ResxTranslatorRunner.Translators
{
    /// <summary>
    /// Translator class responsible for translating the text.
    /// </summary>
    public class GoogleTranslatorService : ITranslatorService
    {
        /// <summary>
        /// List of supported languages
        /// </summary>
        /// <returns></returns>
        public string[] GetSupportedLanguages()
        {
            return Languages.TranslatableCollection;
        }

        /// <summary>
        /// Translate Text using Google Translate
        /// </summary>
        /// <param name="input">The string you want translated</param>
        /// <param name="languagePair">2 letter Language Pair, delimited by "|". 
        /// e.g. "en|da" language pair means to translate from English to Danish</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>Translated to String</returns>
        public string Translate(string text, string targetLanguage, string sourceLanguage = "en")
        {
            try
            {
                var languagePair = $"{sourceLanguage}|{targetLanguage}";
                string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", Uri.EscapeUriString(text), languagePair);
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                string result = webClient.DownloadStringUsingResponseEncoding(url);
                result = result.Substring(result.IndexOf("<span title=\"") + "<span title=\"".Length);
                result = result.Substring(result.IndexOf(">") + 1);
                result = result.Substring(0, result.IndexOf("</span>"));
                return result.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}