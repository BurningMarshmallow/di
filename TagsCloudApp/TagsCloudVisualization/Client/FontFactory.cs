using System;
using System.Drawing;

namespace TagsCloudVisualization.Client
{
    public class FontFactory
    {
        private readonly int minFontSize;
        private readonly int maxFontSize;
        public readonly FontFamily FontFamily;

        public FontFactory(int optionsMinFontSize, int optionsMaxFontSize, string optionsFontFamily)
        {
            minFontSize = optionsMinFontSize;
            maxFontSize = optionsMaxFontSize;
            var result = CreateFontFamilyFromName(optionsFontFamily);
            if (result.IsSuccess)
            {
                 FontFamily = result.Value;
            }
            else
            {
                Console.WriteLine("Font name can't be recognised, going to use Verdana");
                FontFamily = new FontFamily("Verdana");
            }
        }

        private static Result<FontFamily> CreateFontFamilyFromName(string name)
        {
            return Result.Of(() => new FontFamily(name));
        }

        public Font CreateFontFromWeight(int tagWeight, int minTagWeight, int maxTagWeight)
        {
            var fontSize = minFontSize +
               (tagWeight - minTagWeight) * (maxFontSize - minFontSize) /
               (maxTagWeight - minTagWeight);
            return new Font(FontFamily, fontSize);
        }
    }
}