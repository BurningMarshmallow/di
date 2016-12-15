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
    }
}