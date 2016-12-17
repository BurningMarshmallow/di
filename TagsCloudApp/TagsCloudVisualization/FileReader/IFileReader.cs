namespace TagsCloudVisualization.FileReader
{
    public interface IFileReader
    {
        string[] GetFileLines(string filename);
    }
}