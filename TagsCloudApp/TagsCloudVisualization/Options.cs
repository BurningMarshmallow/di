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

        [Option('w', "image_width", DefaultValue = 800, HelpText = "Width of resulting image with tags cloud")]
        public int ImageWidth { get; set; }

        [Option('h', "image_height", DefaultValue = 800, HelpText = "Height of resulting image with tags cloud")]
        public int ImageHeight { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
