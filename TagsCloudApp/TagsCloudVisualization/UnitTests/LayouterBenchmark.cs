using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Spiral;

namespace TagsCloudVisualization.UnitTests
{
    [SimpleJob(1, 1, 1, "BasicBenchmark")]
    public class LayouterBench
    {
        [Params(1000)]
        public int NumberOfRectangles { get; set; }

        public static readonly Point Center = new Point(400, 400);
        public readonly ISpiral CircleSpiral = new CircleSpiral(Center);

        public LayouterWithEndlessSpiral LayouterWithEndlessSpiral { get; set; }
        public LayouterWithGeneratorSpiral LayouterWithGeneratorSpiral { get; set; }

        [Setup]
        public void Setup()
        {
            //LayouterWithEndlessSpiral = new /*LayouterWithEndlessSpiral*/(Center);
            LayouterWithGeneratorSpiral = new LayouterWithGeneratorSpiral(Center, CircleSpiral);
        }

        [Benchmark]
        public void PutManyRectanglesWithEndlessSpiral()
        {
            var rectangleSize = new Size(10, 10);
            for (var i = 0; i < NumberOfRectangles; i++)
            {
                LayouterWithEndlessSpiral.PutNextRectangle(rectangleSize);
            }
        }

        [Benchmark]
        public void PutManyRectanglesWithGeneratorSpiral()
        {
            var rectangleSize = new Size(10, 10);

            for (var i = 0; i < NumberOfRectangles; i++)
            {
                LayouterWithGeneratorSpiral.PutNextRectangle(rectangleSize);
            }
        }

    }

    public class EntryPoint
    {
        public static void RunBench()
        {
            BenchmarkRunner.Run<LayouterBench>();
        }
    }
}