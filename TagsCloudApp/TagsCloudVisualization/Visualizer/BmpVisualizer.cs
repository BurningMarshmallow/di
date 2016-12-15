using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Visualizer
{
    class BmpVisualizer : BaseVisualizer
    {
        public override void SaveImage(Image bitmap, string filename)
        {
            bitmap.Save(filename, ImageFormat.Bmp);
        }

        public BmpVisualizer()
        {
        }

        public BmpVisualizer(Color tagColor, Color backgroundColor, int imageWidth, int imageHeight)
            : base(tagColor, backgroundColor, imageWidth, imageHeight)
        {
        }
    }
}