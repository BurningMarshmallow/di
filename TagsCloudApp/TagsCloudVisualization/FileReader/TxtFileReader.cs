using System.IO;

namespace TagsCloudVisualization.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public Result<string[]> GetFileLines(string filename)
        {
            if (!File.Exists(filename))
                return Result.Fail<string[]>("File was not found");
            var fileLines = Result.Of(() => File.ReadAllLines(filename));
            return fileLines.ReplaceError(m => "File can't be read");
        }
    }
}