using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization.Statistics
{
    class WordStatistics
    {
        public static Dictionary<string, int> GenerateFrequencyStatisticsFromTextFile(string[] textLines, IWordProcessor wordProcessor, IWordSelector wordSelector)
        {
            var frequencyDictionary = new Dictionary<string, int>();
            var words = textLines
                .SelectMany(line => Regex.Split(line, @"\W+"))
                .Where(wordSelector.IsWordAcceptable)
                .Select(wordProcessor.ProcessWord)
                .ToArray();

            var uniqueWords = words.Distinct();

            foreach (var uniqueWord in uniqueWords)
            {
                var wordCount = words.Count(word => word == uniqueWord);
                frequencyDictionary.Add(uniqueWord, wordCount);
            }

            return frequencyDictionary;
        }

        public static Font BuildFontFromWeight(int tagWeight, int minTagWeight, int maxTagWeight, int minFontSize, int maxFontSize, string fontFamily)
        {
            var fontSize = minFontSize +
                           (tagWeight - minTagWeight) * (maxFontSize - minFontSize) /
                           (maxTagWeight - minTagWeight);
            return new Font(fontFamily, fontSize);
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
