using System.Drawing;

namespace TagsCloudVisualization.Client
{
    public class FontFactory
    {
        public readonly int MinFontSize;
        public readonly int MaxFontSize;
        public readonly string FontFamily;

        public FontFactory(int optionsMinFontSize, int optionsMaxFontSize, string optionsFontFamily)
        {
            MinFontSize = optionsMinFontSize;
            MaxFontSize = optionsMaxFontSize;
            FontFamily = optionsFontFamily;
        }

        public Font CreateFontFromWeight(int tagWeight, int minTagWeight, int maxTagWeight)
        {
            var fontSize = MinFontSize +
               (tagWeight - minTagWeight) * (MaxFontSize - MinFontSize) /
               (maxTagWeight - minTagWeight);
            return new Font(FontFamily, fontSize);
        }
    }
}