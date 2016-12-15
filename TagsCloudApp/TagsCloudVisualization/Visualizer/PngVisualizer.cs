using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Visualizer
{
    class PngVisualizer : BaseVisualizer
    {
        public override void SaveImage(Image bitmap, string filename)
        {
            bitmap.Save(filename, ImageFormat.Png);
        }

        public PngVisualizer()
        {
        }

        public PngVisualizer(Color tagColor, Color backgroundColor, int imageWidth, int imageHeight)
            : base(tagColor, backgroundColor, imageWidth, imageHeight)
        {
        }
    }
}