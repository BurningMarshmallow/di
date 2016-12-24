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
            var config = Result.Of(() => File.ReadAllText(settingsFilename));
            if (!config.IsSuccess)
            {
                return Result.Fail<ImageSettings>("Settings can't be found");
            }
#pragma warning disable 618
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
#pragma warning restore 618

            var settings = Result.Of(() => deserializer.Deserialize<ImageSettings>(config.Value));
            return settings.ReplaceError(m => "Settings are incorrect");
        }
    }
}
