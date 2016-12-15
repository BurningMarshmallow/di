using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization.Visualizer
{
    public class Tag
    {
        public string Text { get; }
        public Size TagSize { get; }
        public Font TagFont { get; }
        public Rectangle Place { get; set; }

        public Tag(string text, Font font)
        {
            Text = text;
            TagFont = font;
            TagSize = TextRenderer.MeasureText(text, font);
        }
    }
}
