using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class LayouterWithEndlessSpiral : ILayouter
    {
        private readonly List<Rectangle> rectangles;
        public Point Center { get; }
        private readonly EndlessSpiral spiral;
        public LayouterWithEndlessSpiral(Point center)
        {
            rectangles = new List<Rectangle>();
            Center = center;
            spiral = new EndlessSpiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle newRectangle;
            if (rectangles.Count == 0)
                newRectangle = RectangleFactory.CreateFromCenterAndSize(Center, rectangleSize);
            else
            {
                var currentPoint = GetNextPoint(rectangleSize);

                newRectangle = RectangleFactory.CreateFromCenterAndSize(currentPoint, rectangleSize);
            }
            rectangles.Add(newRectangle);
            return newRectangle;
        }

        private Point GetNextPoint(Size rectangleSize)
        {
            foreach (var point in spiral)
            {
                if (CanBePlaced(point, rectangleSize))
                    return point;
            }
            return Center;
        }

        private bool CanBePlaced(Point point, Size rectangleSize)
        {
            var rect = RectangleFactory.CreateFromCenterAndSize(point, rectangleSize);
            return !rectangles.Any(rect.IntersectsWith);
        }

        public Rectangle[] Rectangles => rectangles.ToArray();
    }
}

