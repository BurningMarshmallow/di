using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IVisualizer
    {
        void Visualize(Rectangle[] rectangles, string filename);
    }
}