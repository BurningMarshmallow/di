using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using Castle.Windsor;
using TagsCloudVisualization.FileReader;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Statistics;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.Client
{
    public abstract class BaseClient
    {
        public void Run(IWindsorContainer container, string[] args)
        {
            var settings = GetTagCloudSettings(args);
            var layouter = container.Resolve<ILayouter>();

            var fontFactory = new FontFactory(settings.MinFontSize, settings.MaxFontSize, settings.FontFamily);
            GetStatisticsFromTextFile(container, settings.TextInputFile)
                .Then(statistics => LayoutTags(statistics, layouter, settings.NumberOfWords, fontFactory))
                .Then(tags => SettingsParser.ParseImageSettings(settings.SettingsFilename)
                    .OnFail(PrintErrorMessage)
                    .Then(imageSettings => new Visualizer(imageSettings))
                    .Then(visualizer =>
                            visualizer.VisualizeTags(settings.ImageOutputFile, tags, ImageFormat.Bmp)));
        }

        private Result<Dictionary<string, int>> GetStatisticsFromTextFile(IWindsorContainer container,
            string textInputFilename)
        {
            var fileReader = container.Resolve<IFileReader>();
            var wordSelector = container.Resolve<IWordSelector>();

            //var wordProcessor = container.Resolve<IWordProcessor>();

            var textLines = fileReader.GetFileLines(textInputFilename)
                .OnFail(PrintErrorMessage);
            var wordProcessor = ResultClassFactory<LowerStemWordProcessor>.Create(
                    "Dictionaries for NHunspell were not found" +
                    ",\nTag cloud will be created without getting word stem")
                   .OnFail(PrintErrorMessage);

            return WordStatistics.GenerateFrequencyStatisticsFromTextLines(
                textLines.Value, wordProcessor.Value, wordSelector);
        }

        protected abstract TagCloudSettings GetTagCloudSettings(string[] args);

        protected abstract void PrintErrorMessage(string message);

        protected static IEnumerable<Tag> LayoutTags(Dictionary<string, int> statistics, ILayouter layouter,
            int numberOfWords, FontFactory fontFactory)
        {
            var mostPopularWords = WordStatistics.GetMostPopularWords(statistics, numberOfWords);

            var minTagWeight = mostPopularWords.Last().Value;
            var maxTagWeight = mostPopularWords.First().Value;
            if (maxTagWeight == minTagWeight)
                minTagWeight -= 1;

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