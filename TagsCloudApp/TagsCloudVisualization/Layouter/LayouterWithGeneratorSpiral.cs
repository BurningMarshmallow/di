using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Spiral;

namespace TagsCloudVisualization.Layouter
{
    public class LayouterWithGeneratorSpiral : ILayouter
    {
        private readonly List<Rectangle> rectangles;
        public Point Center { get; }
        private ISpiral Spiral { get; }

        public LayouterWithGeneratorSpiral(Point center, ISpiral spiral)
        {
            rectangles = new List<Rectangle>();
            Center = center;
            Spiral = spiral;
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
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
            if (!IsInTagCloud(newRectangle))
                return Result.Fail<Rectangle>("Tag cloud doesn't fit in image");
            rectangles.Add(newRectangle);
            return newRectangle;
        }

        private bool IsInTagCloud(Rectangle rectangle)
        {
            return rectangle.Bottom > 0 && rectangle.Bottom < 2 * Center.Y
                   && rectangle.Top > 0 && rectangle.Top < 2 * Center.Y
                   && rectangle.Left > 0 && rectangle.Left < 2 * Center.X
                   && rectangle.Right > 0 && rectangle.Right < 2 * Center.X;
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
                point = Spiral.GenerateNextPoint();
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