using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization.Statistics
{
    class WordStatistics
    {
        public static Result<Dictionary<string, int>> GenerateFrequencyStatisticsFromTextLines(string[] textLines, IWordProcessor wordProcessor, IWordSelector wordSelector)
        {
            var frequencyDictionary = new Dictionary<string, int>();
            var words = GetWordsFromLines(textLines)
                .Where(wordSelector.IsWordAcceptable)
                .Select(word => wordProcessor == null
                ? word
                : wordProcessor.ProcessWord(word));

            foreach (var word in words)
            {
                if (frequencyDictionary.ContainsKey(word))
                    frequencyDictionary[word]++;
                else
                    frequencyDictionary[word] = 1;
            }

            return Result.Ok(frequencyDictionary);
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
