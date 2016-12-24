namespace TagsCloudVisualization.FileReader
{
    public interface IFileReader
    {
        Result<string[]> GetFileLines(string filename);
    }
}