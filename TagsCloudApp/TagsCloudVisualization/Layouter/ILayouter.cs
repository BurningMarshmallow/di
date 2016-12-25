using System.Drawing;

namespace TagsCloudVisualization.Layouter
{
    public interface ILayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
        Rectangle[] Rectangles { get; }
    }
}