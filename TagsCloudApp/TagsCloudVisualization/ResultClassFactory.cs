namespace TagsCloudVisualization
{
    public class ResultClassFactory<T> where T : new()
    {
        public static Result<T> Create(string errorMessage)
        {
            return Result.Of(() => new T(), errorMessage);
        }
    }
}
