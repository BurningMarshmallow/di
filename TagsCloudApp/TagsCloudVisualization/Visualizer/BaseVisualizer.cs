using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Visualizer
{
    abstract class BaseVisualizer
    {
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly Pen tagPen;
        private readonly Color backgroundColor;

        protected BaseVisualizer()
        {
            backgroundColor = Color.Purple;
            tagPen = new Pen(Color.Orange, 3);
            imageWidth = 800;
            imageHeight = 800;
        }

        protected BaseVisualizer(Color tagColor, Color backgroundColor, int imageWidth, int imageHeight)
        {
            tagPen = new Pen(tagColor, 3);
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
            SaveImage(bitmap, filename);
        }

        public abstract void SaveImage(Image bitmap, string filename);

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
            SaveImage(bitmap, filename);
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
