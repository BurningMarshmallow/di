using System;
using System.IO;

namespace TagsCloudVisualization.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public Result<string[]> GetFileLines(string filename)
        {
            try
            {
                var fileLines = Result.Of(() => File.ReadAllLines(filename));
                return fileLines;
            }
            catch (FileNotFoundException)
            {
                return Result.Fail<string[]>("File was not found");
            }
            catch (Exception)
            {
                return Result.Fail<string[]>("File can't be read");
            }
        }
    }
}