using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ILayouter
    {
        //Point GetNextPoint(Size rectangleSize);
        Rectangle PutNextRectangle(Size rectangleSize);
        Rectangle[] Rectangles { get; }
    }
}