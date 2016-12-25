using System.Drawing;

namespace TagsCloudVisualization
{
    public class LayouterFactory<TL, TS> where TL : new() where TS : new()
    {
        public static TL Create(Point centerPoint)
        {
            return new TL();
        }
    }
}
