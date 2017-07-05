namespace ResxTranslatorRunner.Translators.Interfaces
{

    /// <summary>
    /// Abstraction for translator service
    /// </summary>
    public interface ITranslatorService
    {
        string[] GetSupportedLanguages();
        string Translate(string text, string targetLanguage, string sourceLanguage = "en");
    }
}
