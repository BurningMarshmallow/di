using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    class Program
    {
        public static Rectangle[] GenerateNewLayout(Layouter layouter, int numberOfRectangles)
        {
            var rnd = new Random();
            for (var i = 0; i < numberOfRectangles; i++)
                layouter.PutNextRectangle(new Size(rnd.Next(30, 50), rnd.Next(20, 40)));
            return layouter.Rectangles;
        }

        public static void Main()
        {
            var center = new Point(400, 400);
            var spiral = new CrossSpiral(center);
            var layouter = new Layouter(center, spiral);
            var visualiser = new Visualizer();
            var testData = GenerateNewLayout(layouter, 50);
            visualiser.Visualize(testData, "BigTest.bmp");
        }
    }
}