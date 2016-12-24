﻿using System;
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

            var statistics = GetStatisticsFromTextFile(container, settings.TextInputFile);
            var fontFactory = new  FontFactory(settings.MinFontSize, settings.MaxFontSize, settings.FontFamily);
            var tags = LayoutTags(statistics, layouter, settings.NumberOfWords, fontFactory);

            SettingsParser.ParseImageSettings(settings.SettingsFilename)
                .OnFail(PrintErrorMessage)
                .Then(imageSettings => new Visualizer(imageSettings))
                .Then(visualizer =>
                        visualizer.VisualizeTags(settings.ImageOutputFile, tags, ImageFormat.Bmp));
        }

        private Dictionary<string, int> GetStatisticsFromTextFile(IWindsorContainer container, string textInputFilename)
        {
            var fileReader = container.Resolve<IFileReader>();
            //var wordProcessor = container.Resolve<IWordProcessor>();
            var wordProcessor = ResultClassFactory<LowerStemWordProcessor>.Create();
            if (!wordProcessor.IsSuccess)
            {
                Console.WriteLine(wordProcessor.Error);
                Console.WriteLine(wordProcessor.Error);

                return null;
            }
            var wordSelector = container.Resolve<IWordSelector>();

            var textLines = fileReader.GetFileLines(textInputFilename)
                .OnFail(PrintErrorMessage);
            return WordStatistics.GenerateFrequencyStatisticsFromTextLines(textLines.Value, wordProcessor.Value, wordSelector);
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
