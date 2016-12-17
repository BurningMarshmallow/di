using System.Drawing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using TagsCloudVisualization.Client;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Spiral;
using TagsCloudVisualization.Visualization;
using TagsCloudVisualization.WordProcessor;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new WindsorContainer();
            var imageSettings = SettingsParser.ParseImageSettings(@"..\..\Settings.config");
            if (imageSettings == null)
                return;

            container.Register(
                Component
                    .For<IFileReader>()
                    .ImplementedBy<TxtFileReader>());
            container.Register(
                Component
                    .For<IWordProcessor>()
                    .ImplementedBy<LowerCaseWordProcessor>());
            container.Register(
                Component
                    .For<BaseClient>()
                    .ImplementedBy<ConsoleClient>());

            RegisterComponentsForVisualizer(container, imageSettings);
            RegisterComponentsForLayouter(container);

            var client = container.Resolve<BaseClient>();
            client.Run(container, args);
        }

        private static void RegisterComponentsForVisualizer(IWindsorContainer container, ImageSettings imageSettings)
        {
            container.Register(
                Component
                    .For<Visualizer>()
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