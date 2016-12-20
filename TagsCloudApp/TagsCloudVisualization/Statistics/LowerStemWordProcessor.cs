using System.Linq;
using NHunspell;

namespace TagsCloudVisualization.Statistics
{

    class LowerStemWordProcessor : IWordProcessor
    {
        private readonly Hunspell stemCreator;

        public LowerStemWordProcessor()
        {
            stemCreator = new Hunspell("NHunspellDictionaries\\en_US.aff",
                "NHunspellDictionaries\\en_US.dic");
        }

        public string ProcessWord(string word)
        {
            var res = Result.Of(() =>
            {
                    var stems = stemCreator.Stem(word);
                    if (stems.Count > 0)
                        word = stems.First();
                return word.ToLower();
            });
            return res.IsSuccess ? res.Value : "Error happened";
        }
    }
}
