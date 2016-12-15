using System.Collections.Generic;

namespace TagsCloudVisualization.Visualizer
{
    public interface IVisualizer
    {
        void Visualize(string filename, IEnumerable<Tag> tags);
    }
}