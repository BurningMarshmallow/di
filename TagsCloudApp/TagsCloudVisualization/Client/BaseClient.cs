using System;
using System.Collections.Generic;
using System.Drawing;
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
                .Then(statistics => LayoutTags(statistics, layouter, settings.NumberOfWords,
                    fontFactory, settings.FontFamily))
                .Then(tags => YamlParser.ParseImageSettings(settings.SettingsFilename)
                    .Then(imageSettings => new Visualizer(imageSettings))
                    .Then(visualizer =>
                            visualizer.VisualizeTags(settings.ImageOutputFile, tags, ImageFormat.Bmp)))
                .OnFail(PrintErrorMessage);
        }

        private Result<Dictionary<string, int>> GetStatisticsFromTextFile(IWindsorContainer container,
            string textInputFilename)
        {
            var fileReader = container.Resolve<IFileReader>();
            var wordSelector = container.Resolve<IWordSelector>();
            var wordProcessor = Result.Of(container.Resolve<IWordProcessor>,
                    "Dictionaries for NHunspell were not found" +
                    ",\nTag cloud will be created without getting word stem")
                .OnFail(PrintErrorMessage);

            var textLines = fileReader
                .GetFileLines(textInputFilename);
            if (!textLines.IsSuccess)
                return Result.Fail<Dictionary<string, int>>(textLines.Error);

            return WordStatistics.GenerateFrequencyStatisticsFromTextLines(
                textLines.Value, wordProcessor.Value, wordSelector);
        }

        protected abstract TagCloudSettings GetTagCloudSettings(string[] args);

        protected abstract void PrintErrorMessage(string message);

        protected IEnumerable<Tag> LayoutTags(Dictionary<string, int> statistics, ILayouter layouter,
            int numberOfWords, FontFactory fontFactory, string fontFamilyName)
        {
            var mostPopularWords = WordStatistics.GetMostPopularWords(statistics, numberOfWords);

            var minTagWeight = mostPopularWords.Last().Value;
            var maxTagWeight = mostPopularWords.First().Value;
            if (maxTagWeight == minTagWeight)
                minTagWeight -= 1;
            var fontFamily = CreateFontFamilyFromName(fontFamilyName);

            foreach (var pair in mostPopularWords)
            {
                var tag = new Tag(pair.Key,
                    fontFactory.CreateFontFromWeight(pair.Value, minTagWeight, maxTagWeight, fontFamily));
                tag.Place = layouter.PutNextRectangle(tag.TagSize);
                yield return tag;
            }
        }

        private FontFamily CreateFontFamilyFromName(string name)
        {
            var result = Result.Of(() => new FontFamily(name));
            if (result.IsSuccess)
            {
                return result.Value;
            }
            PrintErrorMessage("Font name can't be recognised, going to use Verdana");
            return new FontFamily("Verdana");
        }
    }
}