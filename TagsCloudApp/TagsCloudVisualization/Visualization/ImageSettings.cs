using System.Drawing;

namespace TagsCloudVisualization.Visualization
{
    public class ImageSettings
    {
        public ImageSettings(int imageHeight, int imageWidth, Color backgroundColor, Color tagColor)
        {
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
            BackgroundColor = backgroundColor;
            TagColor = tagColor;
        }

        public Color TagColor { get; }

        public Color BackgroundColor { get; }
        
        public int ImageWidth { get; }

        public int ImageHeight { get; }
    }
}
