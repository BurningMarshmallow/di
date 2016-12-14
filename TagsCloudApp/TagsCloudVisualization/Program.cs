using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommandLine;

namespace TagsCloudVisualization
{
    class Program
    {
        public static void Main(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine("Can't parse these args, check them");
                return;
            }

            var container = new WindsorContainer();
            var statistics = GenerateFrequencyStatisticsFromTextFile(options.TextInputFile);

            RegisterComponentsForVisualizer(container, options);
            RegisterComponentsForLayouter(container);

            var layouter = container.Resolve<ILayouter>();
            var tags = LayoutTags(statistics, layouter, options);
            var visualizer = container.Resolve<IVisualizer>();
            visualizer.Visualize(options.ImageOutputFile, tags);
        }

        private static IEnumerable<Tag> LayoutTags(Dictionary<string, int> statistics, ILayouter layouter, Options options)
        {
            var mostPopularWords = statistics
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key)
                .Take(200)
                .ToArray();


            var minTagWeight = mostPopularWords.Last().Value;
            var maxTagWeight = mostPopularWords.First().Value;

            foreach (var pair in mostPopularWords)
            {
                var tag = new Tag(pair.Key, BuildFontFromWeight(pair.Value, minTagWeight, maxTagWeight, options));
                tag.Place = layouter.PutNextRectangle(tag.TagSize);
                yield return tag;
            }
        }

        private static void RegisterComponentsForVisualizer(IWindsorContainer container, Options options)
        {
            var backgroundColor = GetColor(options.BackgroundColor);
            var rectangleColor = GetColor(options.RectangleColor);

            container.Register(
                Component
                    .For<IVisualizer>()
                    .ImplementedBy<PngVisualizer>()
                    .DependsOn(
                        Dependency.OnValue("rectangleColor", rectangleColor),
                        Dependency.OnValue("backgroundColor", backgroundColor),
                        Dependency.OnValue("imageHeight", options.ImageHeight),
                        Dependency.OnValue("imageWidth", options.ImageWidth))
            );
        }

        public static Dictionary<string, int> GenerateFrequencyStatisticsFromTextFile(string textFile)
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


        private static void RegisterComponentsForLayouter(IWindsorContainer container)
        {
            var spiralCenter = new Point(400, 400);
            container.Register(
                Component
                    .For<ISpiral>()
                    .ImplementedBy<CircleSpiral>().Named("Spiral")
                    .DependsOn(Dependency.OnValue("spiralCenter", spiralCenter)));

            container.Register(
                Component
                    .For<ILayouter>()
                    .ImplementedBy<Layouter>()
                    .DependsOn(Dependency.OnValue("center", spiralCenter))
                    .DependsOn(Dependency.OnComponent("spiral", "Spiral")));
        }

        private static Color GetColor(string[] channels)
        {
            if (channels.Length != 4)
                throw new ArgumentException("Was expecting 4 channels of color: alpha, red, green, blue");
            var channelsValues = channels.Select(int.Parse).ToArray();
            return Color.FromArgb(channelsValues[0], channelsValues[1], channelsValues[2], channelsValues[3]);
        }

        public static Font BuildFontFromWeight(int tagWeight, int minTagWeight, int maxTagWeight, Options options)
        {
            var fontSize = options.MinFontSize + (tagWeight - minTagWeight) * (options.MaxFontSize - options.MinFontSize) / (maxTagWeight - minTagWeight);
            return new Font(options.FontFamily, fontSize);
        }
    }
}