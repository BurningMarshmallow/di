using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Spiral;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization
{
    public class CastleWindsorRecorder
    {
        public static void RegisterComponentsForVisualizer(IWindsorContainer container, Options options)
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

        private static Color GetColor(IReadOnlyCollection<string> channels)
        {
            if (channels.Count != 4)
                throw new ArgumentException("Was expecting 4 channels of color: alpha, red, green, blue");
            var channelsValues = channels.Select(int.Parse).ToArray();
            return Color.FromArgb(channelsValues[0], channelsValues[1], channelsValues[2], channelsValues[3]);
        }

        public static void RegisterComponentsForLayouter(IWindsorContainer container)
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