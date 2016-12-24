using System.IO;

namespace TagsCloudVisualization.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public Result<string[]> GetFileLines(string filename)
        {
            var fileLines = Result.Of(() => File.ReadAllLines(filename));
                return fileLines
                .ReplaceError(m => m.StartsWith("Could not find file")
                ? "File was not found"
                : "File can't be read");
        }
    }
}