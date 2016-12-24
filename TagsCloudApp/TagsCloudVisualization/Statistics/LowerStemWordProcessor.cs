using System;
using System.Linq;
using NHunspell;

namespace TagsCloudVisualization.Statistics
{

    class LowerStemWordProcessor : IWordProcessor
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
                Console.WriteLine("Dictionaries for NHunspell were not found," +
                                  " please check that they are placed in NHunspellDictionaries directory");
                Console.WriteLine("Tag cloud will be created without getting word stem");
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
