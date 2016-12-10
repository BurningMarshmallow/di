using System;
using System.Drawing;
// ReSharper disable InconsistentNaming

namespace TagsCloudVisualization
{
    class CircleSpiral : ISpiral
    {
        private readonly Point spiralCenter;
        private int currentIteration;
        private const double StartRadius = 0.01;
        private const double StartAngle = 50;

        public CircleSpiral(Point spiralCenter)
        {
            this.spiralCenter = spiralCenter;
            currentIteration = 1;
        }

        public Point GenerateNextPoint()
        {
            var radius = StartRadius * currentIteration;
            var angle = StartAngle * currentIteration;
            var x = (int)(radius * Math.Cos(angle));
            var y = (int)(radius * Math.Sin(angle));
            currentIteration++;
            return new Point(spiralCenter.X + x, spiralCenter.Y + y);
        }
    }
}
