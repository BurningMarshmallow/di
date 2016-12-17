using CommandLine;
using CommandLine.Text;

namespace TagsCloudVisualization
{
    public class Options : TagCloudSettings
    {
        [OptionArray('n', "number_of_words", DefaultValue = 100,
        HelpText = "Number of words to layout")]
        public new int NumberOfWords { get; set; }

        [Option('i', "text_input_file", DefaultValue = "README.md",
            HelpText = "Name of text file containing words")]
        public new string TextInputFile { get; set; }

        [Option('o', "image_file", DefaultValue = "Cloud.png", HelpText = "Name of resulting image file")]
        public new string ImageOutputFile { get; set; }

        [Option("min_font_size", DefaultValue = 10, HelpText = "Minimal font size of word")]
        public new int MinFontSize { get; set; }

        [Option("max_font_size", DefaultValue = 50, HelpText = "Maximal font size of word")]
        public new int MaxFontSize { get; set; }

        [Option('f', "font", DefaultValue = "Verdana", HelpText = "Font of words in the image")]
        public new string FontFamily { get; set; }

        [Option('w', "settings_filename", DefaultValue = "Settings.config", HelpText = "Tag cloud settings filename")]
        public new string SettingsFilename { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
