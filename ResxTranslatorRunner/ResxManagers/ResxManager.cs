using ResxTranslatorRunner.Translators;
using ResxTranslatorRunner.Translators.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Resources;

namespace ResxTranslatorRunner.ResxManager
{
    /// <summary>
    /// ResxManager class responsible for adding the strings in target resx file.
    /// </summary>
    public static class ResxManager
    {
        /// <summary>
        /// Translates the given resx in the specified language. new language resx
        /// file will be created in the same folder and with the same name suffixed with
        /// the locale name.
        /// </summary>
        /// <param name="targetLanguage">Language in which text to be translated.</param>
        /// <param name="resxFilePath">Source resx file path</param>
        private static void Write(string targetLanguage, string resxFilePath, bool appendMode, ITranslatorService translator)
        {
            if (string.IsNullOrEmpty(resxFilePath))
            {
                throw new ArgumentNullException(resxFilePath, "Resx file path cannot be null or empty");
            }
            if (targetLanguage == null)
            {
                throw new ArgumentNullException(targetLanguage, "Target Language cannot be null");
            }

            using (ResXResourceReader resourceReader = new ResXResourceReader(resxFilePath))
            {
                string locale = targetLanguage.ToLower();

                // Create the required file name with locale.
                string outputFilePath = Path.GetDirectoryName(resxFilePath);
                string outputFileName = Path.GetFileNameWithoutExtension(resxFilePath);
                outputFileName += "." + locale + ".resx";
                outputFilePath = Path.Combine(outputFilePath, outputFileName);
                
                Dictionary<object, object> hashmap = new Dictionary<object, object>();
                if (File.Exists(outputFilePath))
                {
                    if (appendMode)
                    {
                        return;
                    }

                    using (ResXResourceReader resourceValidator = new ResXResourceReader(outputFilePath))
                    {
                        try
                        {
                            resourceValidator.UseResXDataNodes = true;
                            foreach (DictionaryEntry entry in resourceValidator)
                            {
                                ResXDataNode node = (ResXDataNode)entry.Value;

                                if (string.IsNullOrEmpty(node.Comment))
                                {
                                    hashmap.Add(node.Name, node.GetValue((ITypeResolutionService)null));
                                }
                                else
                                {
                                    hashmap.Add(node.Name, string.Empty);
                                }
                            }
                        }
                        catch
                        { }
                    }
                }
                else
                {
                    if (!appendMode)
                    {
                        return;
                    }
                }

                // Create a resx writer.
                using (ResXResourceWriter resourceWriter = new ResXResourceWriter(outputFilePath))
                {
                    foreach (DictionaryEntry entry in resourceReader)
                    {
                        string key = entry.Key as string;
                        // Check if the Key is UI Text element.
                        if (!String.IsNullOrEmpty(key))
                        {
                            string value = entry.Value as string;

                            // check for null or empty
                            if (!String.IsNullOrEmpty(value))
                            {
                                if (hashmap.ContainsKey(key) && !value.Contains(">"))
                                {
                                    if (!string.IsNullOrEmpty(hashmap[key].ToString()))
                                    {
                                        resourceWriter.AddResource(key, hashmap[key]);
                                    }
                                }
                                else
                                {
                                    if (!value.Contains(">"))
                                    {
                                        // Get the translated value.
                                        string translatedValue = translator.Translate(targetLanguage, value);
                                        // add the key value pair.
                                        resourceWriter.AddResource(key, translatedValue);
                                    }
                                }
                            }
                        }
                    }
                    // Generate resx file.
                    resourceWriter.Generate();
                }
            }
        }

        /// <summary>
        /// Main executor
        /// </summary>
        /// <param name="files">List of resource files</param>
        /// <param name="mode">Execution mode</param>
        public static void Execute(string path, bool mode)
        {
            var files = GetAllResxFiles(path);

            if (files != null && files.Any())
            {

                ITranslatorService selectedService = new AzureTranslatorService();

                //Try Azure first...
                var languages = selectedService.GetSupportedLanguages().Where(a => a.Length == 2).ToArray();
                if (!languages.Any())
                {
                    //Call to Azure failed, fallback to Google
                    selectedService = new GoogleTranslatorService();
                }

                foreach (string file in files)
                {
                    foreach (string targetLanguage in languages)
                    {
                        if (targetLanguage.Length == 2)
                        {
                            Write(targetLanguage, file, mode, selectedService);
                            Console.WriteLine($"{targetLanguage}: Done.");
                        }
                    }
                }
            }
        }

        private static string[] GetAllResxFiles(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                Console.Write($" path '{path}' is not valid. Use -h for usage instructions.");
                return null;
            }

            var option = SearchOption.AllDirectories;

            string[] fileNames = Directory.GetFiles(path, "*.resx", option);

            if (fileNames.Length < 0)
            {
                Console.Write("There is no .resx file in the given folder...");
            }

            return fileNames;
        }

    }
}
