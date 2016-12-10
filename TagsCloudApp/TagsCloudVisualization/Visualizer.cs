using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;


namespace TagsCloudVisualization
{
    class Visualizer
    {
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly Pen rectangleColor;
        private readonly Color backgroundColor;

        public Visualizer()
        {
            backgroundColor = Color.SeaShell;
            rectangleColor = new Pen(Color.Tomato, 3);
            imageWidth = 800;
            imageHeight = 800;
        }

        public Visualizer(Pen rectangleColor, Color backgroundColor, int imageWidth, int imageHeight)
        {
            this.rectangleColor = rectangleColor;
            this.backgroundColor = backgroundColor;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }

        public void Visualize(Rectangle[] rectangles, string filename)
        {
            var bitmap = new Bitmap(imageWidth, imageHeight);
            var graphics = Graphics.FromImage(bitmap);
            DrawRectangles(rectangles, graphics);
            bitmap.Save(filename, ImageFormat.Bmp);
        }

        private void DrawRectangles(IEnumerable<Rectangle> rectangles, Graphics graphics)
        {
            graphics.Clear(backgroundColor);
            foreach (var rect in rectangles)
                graphics.DrawRectangle(rectangleColor, rect);
            graphics.Save();
        }
    }
}