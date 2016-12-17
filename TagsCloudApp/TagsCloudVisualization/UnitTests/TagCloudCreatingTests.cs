using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NUnit.Framework;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.UnitTests
{
    [TestFixture]
    class TagCloudCreatingTests
    {
        private Visualizer visualizer;

        [SetUp]
        public void SetUp()
        {   
            var imageSettings = new ImageSettings(400, 400, Color.AliceBlue, Color.AliceBlue);
            visualizer = new Visualizer(imageSettings);
        }

        [Test]
        public void VisualizingTags_CreatesImageInSameDirectory()
        {
            var font = new Font(FontFamily.GenericMonospace, 12);
            var tags = new[] {new Tag("abc", font)};
            
            visualizer.VisualizeTags("Test.bmp", tags, ImageFormat.Bmp);
            
            Assert.True(File.Exists("Test.bmp"));
        }

        [Test]
        public void VisualizingTags_CreatesImageInBmpFormat()
        {
            var font = new Font(FontFamily.GenericMonospace, 12);
            var tags = new[] { new Tag("abc", font) };

            visualizer.VisualizeTags("Test.bmp", tags, ImageFormat.Bmp);

            using (var img = Image.FromFile("Test.bmp"))
            {
                Assert.True(img.RawFormat.Equals(ImageFormat.Bmp));
            }
        }

        [Test]
        public void VisualizingTags_CreatesImageInPngFormat()
        {
            var font = new Font(FontFamily.GenericMonospace, 12);
            var tags = new[] { new Tag("abc", font) };

            visualizer.VisualizeTags("Test.png", tags, ImageFormat.Png);

            using (var img = Image.FromFile("Test.png"))
            {
                Assert.True(img.RawFormat.Equals(ImageFormat.Png));
            }
        }

    }
}
