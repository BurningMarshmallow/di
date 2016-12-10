using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    public class Layouter : ILayouter
    {
        private readonly List<Rectangle> rectangles;
        public Point Center { get; }
        private readonly ISpiral spiral;
        public Layouter(Point center, ISpiral spiral)
        {
            rectangles = new List<Rectangle>();
            Center = center;
            this.spiral = spiral;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle newRectangle;
            if (rectangles.Count == 0)
            {
                newRectangle = RectangleFactory.CreateFromCenterAndSize(Center, rectangleSize);
            }
            else
            {
                var currentPoint = GetNextPoint(rectangleSize);
                newRectangle = RectangleFactory.CreateFromCenterAndSize(currentPoint, rectangleSize);
            }
            rectangles.Add(newRectangle);
            return newRectangle;
        }

        private static Point GetRectangleUpperLeftPoint(Rectangle rect)
        {
            return new Point(rect.Left, rect.Top);
        }

        private Point GetNextPoint(Size rectangleSize)
        {
            var lastPoint = GetRectangleUpperLeftPoint(rectangles[rectangles.Count - 1]);
            var point = lastPoint;
            while (CantBePlaced(point, rectangleSize))
                point = spiral.GenerateNextPoint();
            return point;
        }

        private bool CantBePlaced(Point point, Size rectangleSize)
        {
            var rect = RectangleFactory.CreateFromCenterAndSize(point, rectangleSize);
            return rectangles.Any(rect.IntersectsWith);
        }

        public Rectangle[] Rectangles => rectangles.ToArray();
    }
}
