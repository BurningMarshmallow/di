using System;
using System.Drawing;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommandLine;

namespace TagsCloudVisualization
{
    class Program
    {
        public static Rectangle[] GenerateNewLayout(ILayouter layouter, int numberOfRectangles)
        {
            var rnd = new Random();
            for (var i = 0; i < numberOfRectangles; i++)
                layouter.PutNextRectangle(new Size(rnd.Next(30, 50), rnd.Next(20, 40)));
            return layouter.Rectangles;
        }

        public static void Main(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine("Can't parse these args, check them");
                return;
            }

            var container = new WindsorContainer();

            RegisterComponentsForVisualizer(container, options);
            RegisterComponentsForLayouter(container);

            var layouter = container.Resolve<ILayouter>();
            var visualizer = container.Resolve<IVisualizer>();
            var testData = GenerateNewLayout(layouter, 50);
            visualizer.Visualize(testData, "BigTest.png");
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
                        Dependency.OnValue("imageHeight", options.ImageHeight),
                        Dependency.OnValue("imageWidth", options.ImageWidth),
                        Dependency.OnValue("rectangleColor", rectangleColor),
                        Dependency.OnValue("backgroundColor", backgroundColor)));
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
    }
}