﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;


namespace TagsCloudVisualization
{
    class BmpVisualizer
    {
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly Pen rectanglePen;
        private readonly Color backgroundColor;

        public BmpVisualizer()
        {
            backgroundColor = Color.SeaShell;
            rectanglePen = new Pen(Color.Tomato, 3);
            imageWidth = 800;
            imageHeight = 800;
        }

        public BmpVisualizer(Color rectangleColor, Color backgroundColor, int imageWidth, int imageHeight)
        {
            rectanglePen = new Pen(rectangleColor, 3);
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
            foreach (var rectangle in rectangles)
                graphics.DrawRectangle(rectanglePen, rectangle);
            graphics.Save();
        }
    }
}