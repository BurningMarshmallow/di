using System.Drawing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using TagsCloudVisualization.Client;
using TagsCloudVisualization.FileReader;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Spiral;
using TagsCloudVisualization.Statistics;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new WindsorContainer();

            container.Register(
                Component
                    .For<BaseClient>()
                    .ImplementedBy<ConsoleClient>());

            RegisterComponentsForStatistics(container);
            RegisterComponentsForLayouter(container);

            var client = container.Resolve<BaseClient>();
            client.Run(container, args);
        }


        private static void RegisterComponentsForStatistics(IWindsorContainer container)
        {
            container.Register(
                Component
                    .For<IFileReader>()
                    .ImplementedBy<TxtFileReader>());
            container.Register(
                Component
                    .For<IWordProcessor>()
                    .ImplementedBy<LowerStemWordProcessor>());
            container.Register(
                Component
                    .For<IWordSelector>()
                    .ImplementedBy<LongWordsSelector>());
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