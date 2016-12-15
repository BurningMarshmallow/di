using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Castle.Windsor;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Statistics;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization.Client
{
    class ConsoleClient : IClient
    {
        public void Run(IWindsorContainer container, Options options)
        {
            var layouter = container.Resolve<ILayouter>();
            string[] text;
            try
            {
                text = File.ReadAllLines(options.TextInputFile);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File was not found");
                return;
            }
            var statistics = WordStatistics.GenerateFrequencyStatisticsFromTextFile(text);
            var tags = LayoutTags(statistics, layouter, options);
            var visualizer = container.Resolve<BaseVisualizer>();
            visualizer.Visualize(options.ImageOutputFile, tags);
        }

        private static IEnumerable<Tag> LayoutTags(Dictionary<string, int> statistics, ILayouter layouter, Options options)
        {
            var mostPopularWords = WordStatistics.GetMostPopularWords(statistics, options.NumberOfWords);

            var minTagWeight = mostPopularWords.Last().Value;
            var maxTagWeight = mostPopularWords.First().Value;

            foreach (var pair in mostPopularWords)
            {
                var tag = new Tag(pair.Key, WordStatistics.BuildFontFromWeight(pair.Value, minTagWeight, maxTagWeight, options));
                tag.Place = layouter.PutNextRectangle(tag.TagSize);
                yield return tag;
            }
        }
    }
}
