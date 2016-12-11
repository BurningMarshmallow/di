using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace TagsCloudVisualization
{
    public class EndlessSpiral : ISpiral, IEnumerable<Point>
    {
        private readonly Point spiralCenter;
        private IEnumerator<Point> enumerator;
        private IEnumerator<Point> Enumerator => enumerator ?? (enumerator = GetEnumerator());

        public EndlessSpiral(Point spiralCenter)
        {
            this.spiralCenter = spiralCenter;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return new EndlessSpiralEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Point GenerateNextPoint()
        {
            
            if (Enumerator.MoveNext())
                return Enumerator.Current;
            throw new InvalidOperationException("Can't MoveNext");
        }

        private class EndlessSpiralEnumerator : IEnumerator<Point>
        {
            private readonly EndlessSpiral endlessSpiral;
            private int currentIteration;
            private const double StartRadius = 0.01;
            private const double StartAngle = 50;

            public EndlessSpiralEnumerator(EndlessSpiral endlessSpiral)
            {
                this.endlessSpiral = endlessSpiral;
                currentIteration = 1;
            }

            public void Dispose()
            {

            }

            public bool MoveNext()
            {
                var radius = StartRadius * currentIteration;
                var angle = StartAngle * currentIteration;
                var x = (int)(radius * Math.Cos(angle));
                var y = (int)(radius * Math.Sin(angle));
                currentIteration++;
                Current = new Point(endlessSpiral.spiralCenter.X + x, endlessSpiral.spiralCenter.Y + y);
                return true;
            }

            public void Reset()
            {

            }

            public Point Current { get; private set; }

            object IEnumerator.Current => Current;
        }
    }

    [TestFixture]
    public class TestSpiralImplementations
    {
        [Test]
        public void EndlessAndCircleSpiralsShouldGenerateSamePoints()
        {
            var fixture = new Fixture();

            var x = new Generator<int>(fixture).First();
            var y = new Generator<int>(fixture).First();
            fixture.Customize<Point>(c => c
                .FromFactory(() => new Point(x, y)));
            var anyCenterPoint = fixture.Create<Point>();

            var endlessSpiral = new EndlessSpiral(anyCenterPoint);
            var circleSpiral = new CircleSpiral(anyCenterPoint);

            for (var i = 0; i < 100; i++)
            {
                var point1 = endlessSpiral.GenerateNextPoint();
                var point2 = circleSpiral.GenerateNextPoint();
                point1.ShouldBeEquivalentTo(point2);
            }
        }
    }
}
