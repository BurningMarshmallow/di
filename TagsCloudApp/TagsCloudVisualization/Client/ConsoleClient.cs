using System;
using CommandLine;

namespace TagsCloudVisualization.Client
{
    class ConsoleClient : BaseClient
    {
        protected override TagCloudSettings GetTagCloudSettings(string[] args)
        {
            var options = new Options();
            Parser.Default.ParseArguments(args, options);
            var settingsContainer = new TagCloudSettings
            {
                NumberOfWords = options.NumberOfWords,
                FontFamily = options.FontFamily,
                ImageOutputFile = options.ImageOutputFile,
                MaxFontSize = options.MaxFontSize,
                MinFontSize = options.MinFontSize,
                TextInputFile = options.TextInputFile,
                SettingsFilename = options.SettingsFilename
            };
            return settingsContainer;
        }

        protected override void PrintErrorMessage(string m)
        {
            Console.WriteLine(m);
        }
    }
}