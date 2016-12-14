using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public interface IVisualizer
    {
        void Visualize(string filename, IEnumerable<Tag> tags);
    }
}