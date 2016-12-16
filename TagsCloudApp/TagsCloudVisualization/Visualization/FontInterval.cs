namespace TagsCloudVisualization.Visualization
{
    struct FontInterval
    {
        public readonly int MinFontSize;
        public readonly int MaxFontSize;

        public FontInterval(int minFontSize, int maxFontSize)
        {
            MinFontSize = minFontSize;
            MaxFontSize = maxFontSize;
        }
    }
}
