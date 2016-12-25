using System.Linq;
using NHunspell;

namespace TagsCloudVisualization.Statistics
{
    public class LowerStemWordProcessor : IWordProcessor
    {
        private readonly Hunspell stemCreator;

        public LowerStemWordProcessor()
        {
            stemCreator = new Hunspell(
                "NHunspellDictionaries\\en_US.aff",
                "NHunspellDictionaries\\en_US.dic");
        }

        public string ProcessWord(string word)
        {
            var stems = stemCreator?.Stem(word);
            if (stems?.Count > 0)
                word = stems.First();
            return word.ToLower();
        }
    }
}
