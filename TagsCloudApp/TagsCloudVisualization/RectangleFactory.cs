using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectangleFactory
    {
        public static Rectangle CreateFromCenterAndSize(Point centerPoint, Size rectangleSize)
        {
            var upperLeftPoint = new Point(centerPoint.X - rectangleSize.Width / 2,
                centerPoint.Y - rectangleSize.Height / 2);
            return new Rectangle(upperLeftPoint, rectangleSize);
        }
    }
}