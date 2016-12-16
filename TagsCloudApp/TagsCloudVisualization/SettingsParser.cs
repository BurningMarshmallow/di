using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization
{
    class SettingsParser
    {
        public static ImageSettings ParseImageSettings(string settingsFilename)
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines(settingsFilename);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error reading settings");
                return null;
            }
            var appSettings = lines
                .Where(line => line.Contains(':'))
                .Select(x => x.Split(':'))
                .ToDictionary(pair => pair[0], pair => pair[1]);
            if (appSettings.Count == 0)
            {
                Console.WriteLine("AppSettings is empty.");
            }
            else
            {
                string[] fieldsToGet = { "ImageWidth", "ImageWidth", "BackgroundColor", "TagColor" };
                foreach (var field in fieldsToGet)
                {
                    if (appSettings[field] != null)
                        continue;
                    Console.WriteLine("{0} was not found in settings", field);
                    return null;
                }
                var imageHeight = int.Parse(appSettings["ImageHeight"]);
                var imageWidth = int.Parse(appSettings["ImageWidth"]);
                var backgroundColor = GetColor(appSettings["BackgroundColor"].Split(' '));
                var tagColor = GetColor(appSettings["TagColor"].Split(' '));
                return new ImageSettings(imageHeight, imageWidth, backgroundColor, tagColor);
            }
            return null;
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
