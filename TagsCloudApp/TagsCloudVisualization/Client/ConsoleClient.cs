using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Windsor;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization.Client
{
    class ConsoleClient : IClient
    {
        public void Run(IWindsorContainer container, Options options)
        {
            var layouter = container.Resolve<ILayouter>();
            var statistics = GenerateFrequencyStatisticsFromTextFile(options.TextInputFile);
            var tags = LayoutTags(statistics, layouter, options);
            var visualizer = container.Resolve<BaseVisualizer>();
            visualizer.Visualize(options.ImageOutputFile, tags);

        }

        private static IEnumerable<Tag> LayoutTags(Dictionary<string, int> statistics, ILayouter layouter, Options options)
        {
            var mostPopularWords = statistics
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key)
                .Take(options.NumberOfWords)
                .ToArray();


            var minTagWeight = mostPopularWords.Last().Value;
            var maxTagWeight = mostPopularWords.First().Value;

            foreach (var pair in mostPopularWords)
            {
                var tag = new Tag(pair.Key, Program.BuildFontFromWeight(pair.Value, minTagWeight, maxTagWeight, options));
                tag.Place = layouter.PutNextRectangle(tag.TagSize);
                yield return tag;
            }
        }


        private static Dictionary<string, int> GenerateFrequencyStatisticsFromTextFile(string textFile)
        {
            var frequencyDictionary = new Dictionary<string, int>();

            var text = File.ReadAllLines(textFile);
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

    }
}
