using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    class PngVisualizer : IVisualizer
    {
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly Pen rectanglePen;
        private readonly Color backgroundColor;

        public PngVisualizer()
        {
            backgroundColor = Color.Purple;
            rectanglePen = new Pen(Color.Orange, 3);
            imageWidth = 800;
            imageHeight = 800;
        }

        public PngVisualizer(Color rectangleColor, Color backgroundColor, int imageWidth, int imageHeight)
        {
            rectanglePen = new Pen(rectangleColor, 3);
            this.backgroundColor = backgroundColor;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }

        public void Visualize(string filename, IEnumerable<Tag> tags)
        {
            var bitmap = new Bitmap(imageWidth, imageHeight);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(backgroundColor);
            DrawTags(tags, graphics);
            bitmap.Save(filename, ImageFormat.Png);
        }

        public void DrawTags(IEnumerable<Tag> tags, Graphics graphics)
        {
            var brush = new SolidBrush(rectanglePen.Color);
            foreach (var tag in tags)
            {
                graphics.DrawString(tag.Text, tag.TagFont, brush, tag.Place.Location);
            }
        }
    }
}
