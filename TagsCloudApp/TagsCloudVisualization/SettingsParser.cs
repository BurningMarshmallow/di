using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.FileReader;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization
{
    public class SettingsParser
    {
        public static Result<ImageSettings> ParseImageSettings(string settingsFilename)
        {
            var imageSettings = GetImageSettings(settingsFilename);
            if (imageSettings == null)
                return Result.Fail<ImageSettings>("Filename can't be accessed");

            return Result.Of(() =>
            {
                var areCorrectSettings = Result.Of(() => AreCorrectSettings(imageSettings));
                if (!areCorrectSettings.IsSuccess)
                {
                    throw new Exception(areCorrectSettings.Error);
                }
                
                var imageHeight = int.Parse(imageSettings["ImageHeight"]);
                var imageWidth = int.Parse(imageSettings["ImageWidth"]);
                var backgroundColor = GetColor(imageSettings["BackgroundColor"].Split(' '));
                var tagColor = GetColor(imageSettings["TagColor"].Split(' '));
                return new ImageSettings(imageHeight, imageWidth, backgroundColor, tagColor);
            });
        }

        private static bool AreCorrectSettings(IReadOnlyDictionary<string, string> imageSettings)
        {
            string[] fieldsToGet = { "ImageHeight", "ImageWidth", "BackgroundColor", "TagColor" };
            foreach (var field in fieldsToGet)
            {
                if (imageSettings.ContainsKey(field))
                    continue;
                throw new KeyNotFoundException($"{field} was not found in settings");
            }
            return false;
        }

        private static Dictionary<string, string> GetImageSettings(string settingsFilename)
        {
            var fileReader = new TxtFileReader();
            var fileLines = fileReader
                .GetFileLines(settingsFilename);
            return fileLines?.Where(line => line.Contains(':'))
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
