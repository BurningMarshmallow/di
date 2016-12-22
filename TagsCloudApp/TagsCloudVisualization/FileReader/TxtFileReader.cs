using System;
using System.IO;

namespace TagsCloudVisualization.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public string[] GetFileLines(string filename)
        {
            Result<string[]> fileLines;
            try
            {
                fileLines = Result.Of(() => File.ReadAllLines(filename));
                return fileLines.Value;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File was not found");
            }
            catch (Exception)
            {
                Console.WriteLine("File can't be read");
            }
            return null;
        }
    }
}