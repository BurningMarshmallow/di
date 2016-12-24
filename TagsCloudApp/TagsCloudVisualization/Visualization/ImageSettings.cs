using System.Drawing;

namespace TagsCloudVisualization.Visualization
{
    public class ImageSettings
    {
        public ImageSettings()
        {
            
        }

        public ImageSettings(int imageHeight, int imageWidth, Color backgroundColor, Color tagColor)
        {
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
            BackgroundColor = backgroundColor;
            TagColor = tagColor;
        }

        public Color TagColor { get; set; }

        public Color BackgroundColor { get; set; }
        
        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }
    }
}
