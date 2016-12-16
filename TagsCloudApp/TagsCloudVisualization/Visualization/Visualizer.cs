using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Visualization
{
    public class Visualizer
    {
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly Pen tagPen;
        private readonly Color backgroundColor;

        public Visualizer()
        {
            backgroundColor = Color.Purple;
            tagPen = new Pen(Color.Orange, 3);
            imageWidth = 800;
            imageHeight = 800;
        }

        public Visualizer(ImageSettings imageSettings)
        {
            tagPen = new Pen(imageSettings.TagColor, 3);
            backgroundColor = imageSettings.BackgroundColor;
            imageWidth = imageSettings.ImageWidth;
            imageHeight = imageSettings.ImageHeight;
        }

        public Visualizer(Color tagColor, Color backgroundColor, int imageWidth, int imageHeight)
        {
            tagPen = new Pen(tagColor, 3);
            this.backgroundColor = backgroundColor;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }

        public void VisualizeTags(string filename, IEnumerable<Tag> tags, ImageFormat format)
        {
            var bitmap = new Bitmap(imageWidth, imageHeight);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(backgroundColor);
            DrawTags(tags, graphics);
            SaveImage(bitmap, filename, format);
        }

        private static void SaveImage(Image bitmap, string filename, ImageFormat format)
        {
            bitmap.Save(filename, format);
        }

        private void DrawTags(IEnumerable<Tag> tags, Graphics graphics)
        {
            var brush = new SolidBrush(tagPen.Color);
            foreach (var tag in tags)
            {
                graphics.DrawString(tag.Text, tag.TagFont, brush, tag.Place.Location);
            }
        }

        public void VisualizeRectangles(Rectangle[] tags, string filename)
        {
            var bitmap = new Bitmap(imageWidth, imageHeight);
            var graphics = Graphics.FromImage(bitmap);
            DrawRectangles(tags, graphics);
            SaveImage(bitmap, filename, ImageFormat.Bmp);
        }

        private void DrawRectangles(IEnumerable<Rectangle> tags, Graphics graphics)
        {
            graphics.Clear(backgroundColor);
            foreach (var tag in tags)
                graphics.DrawRectangle(tagPen, tag);
            graphics.Save();
        }
    }
}
