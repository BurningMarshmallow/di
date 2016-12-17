namespace TagsCloudVisualization.WordProcessor
{
    class LowerCaseWordProcessor : IWordProcessor
    {
        public string ProcessWord(string word)
        {
            return word.ToLower();
        }
    }
}
