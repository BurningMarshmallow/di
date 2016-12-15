using System;
using System.Drawing;
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
            var imageSettings = SettingsParser.ParseImageSettings();
            if (imageSettings == null)
                return;

            container.Register(
                Component
                    .For<IClient>()
                    .ImplementedBy<ConsoleClient>());
            RegisterComponentsForVisualizer(container, imageSettings);
            RegisterComponentsForLayouter(container);

            var client = container.Resolve<IClient>();
            client.Run(container, options);
        }

        private static void RegisterComponentsForVisualizer(IWindsorContainer container, ImageSettings imageSettings)
        {
            container.Register(
                Component
                    .For<BaseVisualizer>()
                    .ImplementedBy<PngVisualizer>()
                    .DependsOn(Dependency.OnValue("imageSettings", imageSettings))
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
    }
}