namespace TagsCloudVisualization.Statistics
{
    class LongWordsSelector : IWordSelector
    {
        public bool IsWordAcceptable(string word)
        {
            return word.Length > 4;
        }
    }
}
