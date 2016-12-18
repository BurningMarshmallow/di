using System.Linq;
using NHunspell;

namespace TagsCloudVisualization.Statistics
{
    class LowerStemWordProcessor : IWordProcessor
    {
        public string ProcessWord(string word)
        {
            using (var hunspell = new Hunspell("NHunspellDictionaries\\en_US.aff",
                "NHunspellDictionaries\\en_US.dic"))
            {
                var stems = hunspell.Stem(word);
                if(stems.Count > 0)
                    word = stems.First();
            }
            return word.ToLower();
        }
    }
}
