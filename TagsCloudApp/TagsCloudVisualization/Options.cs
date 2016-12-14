using CommandLine;
using CommandLine.Text;

namespace TagsCloudVisualization
{
    public class Options
    {
        [OptionArray('r', "rectangle_color", DefaultValue = new[] { "255", "255", "0", "0" },
        HelpText = "ARGB channels of rectangle color")]
        public string[] RectangleColor { get; set; }

        [OptionArray('b', "background_color", DefaultValue = new[] { "255", "0", "0", "0" },
            HelpText = "ARGB channels of background color")]
        public string[] BackgroundColor { get; set; }

        [Option('w', "image_width", DefaultValue = 800, HelpText = "Width of resulting image")]
        public int ImageWidth { get; set; }

        [Option('h', "image_height", DefaultValue = 800, HelpText = "Height of resulting image")]
        public int ImageHeight { get; set; }

        [Option('i', "image_file", DefaultValue = "Cloud.png", HelpText = "Name of resulting image file")]
        public string ImageOutputFile { get; set; }

        [Option("min_font_size", DefaultValue = 10, HelpText = "Minimal font size of word")]
        public int MinFontSize { get; set; }

        [Option("max_font_size", DefaultValue = 20, HelpText = "Maximal font size of word")]
        public int MaxFontSize { get; set; }

        [Option('f', "font", DefaultValue = "Verdana", HelpText = "Font of words in the image")]
        public string FontFamily { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
