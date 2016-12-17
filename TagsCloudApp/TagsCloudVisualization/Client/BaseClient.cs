using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using Castle.Windsor;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Statistics;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.Client
{
    public abstract class BaseClient
    {
        public void Run(IWindsorContainer container, string[] args)
        {
            var settings = GetSettingsContainer(args);

            var layouter = container.Resolve<ILayouter>();
            var fileReader = container.Resolve<IFileReader>();
            var wordProcessor = container.Resolve<IWordProcessor>();
            var wordSelector = container.Resolve<IWordSelector>();

            var textLines = fileReader.GetFileLines(settings.TextInputFile);
            var statistics = WordStatistics.GenerateFrequencyStatisticsFromTextFile(textLines, wordProcessor, wordSelector);
            var fontFactory = new FontFactory(settings.MinFontSize, settings.MaxFontSize, settings.FontFamily);

            var tags = LayoutTags(statistics, layouter, settings.NumberOfWords, fontFactory);
            var visualizer = container.Resolve<Visualizer>();
            visualizer.VisualizeTags(settings.ImageOutputFile, tags, ImageFormat.Bmp);
        }

        protected abstract SettingsContainer GetSettingsContainer(string[] args);

        protected static IEnumerable<Tag> LayoutTags(Dictionary<string, int> statistics, ILayouter layouter,
            int numberOfWords, FontFactory fontFactory)
        {
            var mostPopularWords = WordStatistics.GetMostPopularWords(statistics, numberOfWords);

            var minTagWeight = mostPopularWords.Last().Value;
            var maxTagWeight = mostPopularWords.First().Value;

            foreach (var pair in mostPopularWords)
            {
                var tag = new Tag(pair.Key,
                    fontFactory.CreateFontFromWeight(pair.Value, minTagWeight, maxTagWeight));
                tag.Place = layouter.PutNextRectangle(tag.TagSize);
                yield return tag;
            }
        }

    }
}
