namespace TagsCloudVisualization
{
    public class ResultClassFactory<T> where T : new()
    {
        public static Result<T> Create()
        {
            return Result.Of(() => new T());
        }
    }
}
