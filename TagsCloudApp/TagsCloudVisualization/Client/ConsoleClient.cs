using CommandLine;

namespace TagsCloudVisualization.Client
{
    class ConsoleClient : BaseClient
    {
        protected override TagCloudSettings GetSettingsContainer(string[] args)
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
                TextInputFile = options.TextInputFile
            };
            return settingsContainer;
        }
    }
}