using System;
using System.IO;

namespace TagsCloudVisualization.FileReader
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
            }
            catch (Exception)
            {
                Console.WriteLine("File can't be read");
            }
            return null;
        }
    }
}