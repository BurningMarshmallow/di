using System;
using System.Drawing;

namespace TagsCloudVisualization.Client
{
    public class FontFactory
    {
        private readonly int minFontSize;
        private readonly int maxFontSize;

        public FontFactory(int optionsMinFontSize, int optionsMaxFontSize, string optionsFontFamily)
        {
            minFontSize = optionsMinFontSize;
            maxFontSize = optionsMaxFontSize;
        }

        public Font CreateFontFromWeight(int tagWeight, int minTagWeight, int maxTagWeight, FontFamily fontFamily)
        {
            var fontSize = minFontSize +
               (tagWeight - minTagWeight) * (maxFontSize - minFontSize) /
               (maxTagWeight - minTagWeight);
            return new Font(fontFamily, fontSize);
        }
    }
}