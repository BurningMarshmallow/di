namespace TagsCloudVisualization
{
    public interface IFileReader
    {
        string[] GetFileLines(string filename);
    }
}