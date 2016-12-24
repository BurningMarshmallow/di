using System.IO;
using TagsCloudVisualization.Visualization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TagsCloudVisualization
{
    class YamlParser
    {
        public static Result<ImageSettings> ParseImageSettings(string settingsFilename)
        {
            var document = File.ReadAllText(settingsFilename);
#pragma warning disable 618
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
#pragma warning restore 618

            var settings = deserializer.Deserialize<ImageSettings>(document);
            return Result.Ok(settings);
            //Console.WriteLine(settings.BackgroundColor);
            //Console.WriteLine(settings.TagColor);
            //Console.WriteLine(settings.ImageHeight);
            //Console.WriteLine(settings.ImageWidth);
        }
    }
}
