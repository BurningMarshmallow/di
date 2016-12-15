using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Statistics
{
    class WordStatistics
    {
        public static Dictionary<string, int> GenerateFrequencyStatisticsFromTextFile(string[] text)
        {
            var frequencyDictionary = new Dictionary<string, int>();
            var words = text
                .SelectMany(line => Regex.Split(line, @"\W+"))
                .Where(word => word.Length > 4)
                .Select(word => word.ToLower())
                .ToArray();

            var uniqueWords = words.Distinct();

            foreach (var uniqueWord in uniqueWords)
            {
                var wordCount = words.Count(word => word == uniqueWord);
                frequencyDictionary.Add(uniqueWord, wordCount);
            }

            return frequencyDictionary;
        }

        public static Font BuildFontFromWeight(int tagWeight, int minTagWeight, int maxTagWeight, Options options)
        {
            var fontSize = options.MinFontSize +
                           (tagWeight - minTagWeight) * (options.MaxFontSize - options.MinFontSize) /
                           (maxTagWeight - minTagWeight);
            return new Font(options.FontFamily, fontSize);
        }

        public static KeyValuePair<string, int>[]  GetMostPopularWords(Dictionary<string, int> statistics, int numberOfWords)
        {
            return statistics
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key)
                .Take(numberOfWords)
                .ToArray();
        }
    }
}
