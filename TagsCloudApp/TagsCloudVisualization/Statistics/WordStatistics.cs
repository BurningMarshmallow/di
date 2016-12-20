using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization.Statistics
{
    class WordStatistics
    {
        public static Dictionary<string, int> GenerateFrequencyStatisticsFromTextLines(string[] textLines, IWordProcessor wordProcessor, IWordSelector wordSelector)
        {
            var frequencyDictionary = new Dictionary<string, int>();
            var words = GetWordsFromLines(textLines)
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

        public static IEnumerable<string> GetWordsFromLines(string[] lines)
        {
            return lines
                .SelectMany(line => Regex.Split(line, @"\W+"));
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
