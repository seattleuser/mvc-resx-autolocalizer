using Mono.Options;
using System;

namespace ResxTranslatorRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            bool show_help = false;
            bool appendMode = true;
            string path = string.Empty;

            var p = new OptionSet() {
            { "p|path=", "the path to the folder with resource files", v => path = v },
            { "m|mode=", "the execution mode: append or override existing stings. Helpful during incremental runs. In append mode only new strings will be localized", v => appendMode = "override" == v },
            { "h|help",  "show this message and exit", v => show_help = v != null }
            };

            var extra = p.Parse(args);

            if (show_help)
            {
                ShowHelp(p);
                return;
            }

            ResxManager.ResxManager.Execute(path, appendMode);
        }
 
        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: ResxTranslatorRunner [OPTIONS]");
            Console.WriteLine("Translates provided resource file to the set of supported languages");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

    }
}
