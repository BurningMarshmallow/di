using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.FileReader;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization
{
    class SettingsParser
    {
        public static ImageSettings ParseImageSettings(string settingsFilename)
        {
            var imageSettings = GetImageSettings(settingsFilename);
            if (imageSettings.Count == 0)
            {
                Console.WriteLine("ImageSettings is empty.");
            }
            else
            {
                if (AreIncorrectSettings(imageSettings))
                    return null;
                
                var imageHeight = int.Parse(imageSettings["ImageHeight"]);
                var imageWidth = int.Parse(imageSettings["ImageWidth"]);
                var backgroundColor = GetColor(imageSettings["BackgroundColor"].Split(' '));
                var tagColor = GetColor(imageSettings["TagColor"].Split(' '));
                return new ImageSettings(imageHeight, imageWidth, backgroundColor, tagColor);
            }
            return null;
        }

        private static bool AreIncorrectSettings(IReadOnlyDictionary<string, string> imageSettings)
        {
            string[] fieldsToGet = { "ImageWidth", "ImageWidth", "BackgroundColor", "TagColor" };
            foreach (var field in fieldsToGet)
            {
                if (imageSettings[field] != null)
                    continue;
                Console.WriteLine("{0} was not found in settings", field);
                return false;
            }
            return true;
        }

        private static Dictionary<string, string> GetImageSettings(string settingsFilename)
        {
            var fileReader = new TxtFileReader();
            return fileReader
                .GetFileLines(settingsFilename)
                .Where(line => line.Contains(':'))
                .Select(x => x.Split(':'))
                .ToDictionary(pair => pair[0], pair => pair[1]);
        }

        private static Color GetColor(IReadOnlyCollection<string> channels)
        {
            if (channels.Count != 4)
                throw new ArgumentException("Was expecting 4 channels of color: alpha, red, green, blue");
            var channelsValues = channels.Select(int.Parse).ToArray();
            return Color.FromArgb(channelsValues[0], channelsValues[1], channelsValues[2], channelsValues[3]);
        }
    }
}
