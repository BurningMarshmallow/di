using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Spiral;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.UnitTests
{
    [TestFixture]
    class LayouterTests
    {
        private readonly Point center = new Point(400, 400);
        private LayouterWithGeneratorSpiral cloudLayouter;

        [SetUp]
        public void SetUp()
        {
            var spiral = new CircleSpiral(center);
            cloudLayouter = new LayouterWithGeneratorSpiral(center, spiral);
        }

        [Test]
        public void FirstRectangleCorrectlyPlaced()
        {
            var newRect = cloudLayouter.PutNextRectangle(new Size(200, 100));

            Assert.AreEqual(new Rectangle(300, 350, 200, 100), newRect);
            Assert.AreEqual(center, newRect.GetCenterOfRectangle());
        }

        [TestCase(200)]
        [TestCase(1)]
        [TestCase(101)]
        [TestCase(2)]
        public void AllRectanglesSaved_AfterAdding(int numberOfRectangles)
        {
            var rectangleSize = new Size(20, 20);

            for (var i = 0; i < numberOfRectangles; i++)
            {
                cloudLayouter.PutNextRectangle(rectangleSize);
            }

            Assert.AreEqual(numberOfRectangles, cloudLayouter.Rectangles.Length);
        }

        [TestCase(800, 40)]
        [TestCase(1337, 22)]
        [TestCase(111, 444)]
        [TestCase(300, 300)]
        public void TwoConsequentlyAddedRectangles_DoNotIntersect(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var first = cloudLayouter.PutNextRectangle(rectangleSize);
            var second= cloudLayouter.PutNextRectangle(rectangleSize);

            Assert.False(first.IntersectsWith(second));
        }

        [TestCase(240)]
        [TestCase(33)]
        [TestCase(2)]
        public void ManyAddedRectangles_DoNotIntersect(int numberOfRectangles)
        {
            var rectangleSize = new Size(10, 10);

            for (var i = 0; i < numberOfRectangles; i++)
            {
                cloudLayouter.PutNextRectangle(rectangleSize);
            }
            var rectangles = cloudLayouter.Rectangles;

            for (var i = 0; i < numberOfRectangles; i++)
            {
                for (var j = i + 1; j < numberOfRectangles; j++)
                {
                    Assert.False(rectangles[i].IntersectsWith(rectangles[j]));
                }
            }
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        private static bool IsInsideCircle(Point point, Point circleCenter, int radius)
        {
            return GetDistance(point.X, point.Y, circleCenter.X, circleCenter.Y) < radius;
        }

        private static double GetOuterRectangles(Rectangle[] rectangles)
        {
            var rectanglesArea = rectangles.Sum(x => x.Width * x.Height);
            var circleArea = rectanglesArea * Math.PI / 2;
            var radius = (int)Math.Ceiling(Math.Sqrt(circleArea / Math.PI));
            var circleCenter = rectangles[0].GetCenterOfRectangle();
            double outerRectanglesCount = rectangles.Count(
                x => !IsInsideCircle(x.GetCenterOfRectangle(), circleCenter, radius));
            return outerRectanglesCount;
        }

        [TestCase(3, 228, 1337)]
        [TestCase(90, 1001, 20)]
        [TestCase(5, 5, 555)]
        [TestCase(33, 44, 55)]
        [TestCase(1, 1, 1)]
        [TestCase(789, 1, 3)]
        [TestCase(300, 100, 100)]

        public void CloudForm_IsSimilarToCircle(int numberOfRectangles, int width, int height)
        {
            var rectangleSize = new Size(width, height);

            for (var i = 0; i < numberOfRectangles; i++)
            {
                cloudLayouter.PutNextRectangle(rectangleSize);
            }
            var rectangles = cloudLayouter.Rectangles;
            const double eps = 0.1;

            var outerRectanglesCount = GetOuterRectangles(rectangles);
            var outerRectanglesCoefficent = outerRectanglesCount / rectangles.Length;

            Assert.Less(outerRectanglesCoefficent, eps);
        }

        [TearDown]
        public void DrawOnFailure()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;

            var visualizator = new Visualization.Visualizer();
            var dir = TestContext.CurrentContext.TestDirectory;
            var testName = TestContext.CurrentContext.Test.Name;
            var path = dir + testName + ".bmp";
            visualizator.VisualizeRectangles(cloudLayouter.Rectangles, path);
            Console.WriteLine("Tag cloud visualization saved to file " + path);
        }
    }
}
 