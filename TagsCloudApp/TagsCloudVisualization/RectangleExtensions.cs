using System.Drawing;


namespace TagsCloudVisualization
{
    static class RectangleExtensions
    {
        public static Point GetCenterOfRectangle(this Rectangle rectangle)
        {
            return new Point((rectangle.Left + rectangle.Right) / 2, (rectangle.Top + rectangle.Bottom) / 2);
        }
    }
}