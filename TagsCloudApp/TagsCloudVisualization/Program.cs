using System;
using System.Drawing;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommandLine;
using TagsCloudVisualization.Client;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Spiral;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization
{
    public class Program
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

            container.Register(
                Component
                    .For<IClient>()
                    .ImplementedBy<ConsoleClient>());
            RegisterComponentsForVisualizer(container, options);
            RegisterComponentsForLayouter(container);

            var client = container.Resolve<IClient>();
            client.Run(container, options);
        }

        private static void RegisterComponentsForVisualizer(IWindsorContainer container, Options options)
        {
            var backgroundColor = GetColor(options.BackgroundColor);
            var tagColor = GetColor(options.TagColor);

            container.Register(
                Component
                    .For<BaseVisualizer>()
                    .ImplementedBy<PngVisualizer>()
                    .DependsOn(
                        Dependency.OnValue("tagColor", tagColor),
                        Dependency.OnValue("backgroundColor", backgroundColor),
                        Dependency.OnValue("imageHeight", options.ImageHeight),
                        Dependency.OnValue("imageWidth", options.ImageWidth))
            );
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
                    .ImplementedBy<LayouterWithGeneratorSpiral>()
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
            var fontSize = options.MinFontSize +
                           (tagWeight - minTagWeight)*(options.MaxFontSize - options.MinFontSize)/
                           (maxTagWeight - minTagWeight);
            return new Font(options.FontFamily, fontSize);
        }
    }
}