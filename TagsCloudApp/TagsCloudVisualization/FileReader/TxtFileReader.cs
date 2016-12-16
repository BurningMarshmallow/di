using System;
using System.IO;

namespace TagsCloudVisualization
{
    public class TxtFileReader : IFileReader
    {
        public string[] GetFileLines(string filename)
        {
            try
            {
                return File.ReadAllLines(filename);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File was not found");
                return null;
            }
        }
    }
}