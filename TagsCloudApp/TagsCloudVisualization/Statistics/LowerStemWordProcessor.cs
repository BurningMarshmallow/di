using System;
using System.Linq;
using NHunspell;

namespace TagsCloudVisualization.Statistics
{
    public class LowerStemWordProcessor : IWordProcessor
    {
        private readonly Hunspell stemCreator;

        public LowerStemWordProcessor()
        {
            var result = Result.Of(() => new Hunspell("NHunspellDictonaries\\en_US.aff",
                    "NHunspellDictionaries\\en_US.dic"));
            if (result.IsSuccess)
                stemCreator = result.Value;
            else
            {
                throw new ArgumentException("Dictionaries for NHunspell were not found" +
                                            ", Tag cloud will be created without getting word stem");
            }
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
