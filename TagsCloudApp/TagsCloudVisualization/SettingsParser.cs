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
            if (!imageSettings.IsSuccess)
            {
                return Result.Fail<ImageSettings>("Settings can't be found");
            }

            var ims = imageSettings.Value;
            return AreAllFieldsInSettings(ims)
                .Then(_ => new ImageSettings(
                        int.Parse(ims["ImageHeight"]),
                        int.Parse(ims["ImageWidth"]),
                        GetColor(ims["BackgroundColor"].Split(' ')),
                        GetColor(ims["TagColor"].Split(' ')))
                );
        }

        private static Result<bool> AreAllFieldsInSettings(IReadOnlyDictionary<string, string> imageSettings)
        {
            string[] fieldsToGet = {"ImageHeight", "ImageWidth", "BackgroundColor", "TagColor"};
            foreach (var field in fieldsToGet)
            {
                if (imageSettings.ContainsKey(field))
                    continue;
                return Result.Fail<bool>($"{field} was not found in settings");
            }
            return new Result<bool>(); 
        }

        private static Result<Dictionary<string, string>> GetImageSettings(string settingsFilename)
        {
            var fileReader = new TxtFileReader();
            var fileLines = fileReader
                .GetFileLines(settingsFilename);
            return fileLines.Then(
                lines => lines.Where(line => line.Contains(':'))
                .Select(x => x.Split(':'))
                .ToDictionary(pair => pair[0], pair => pair[1]));
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