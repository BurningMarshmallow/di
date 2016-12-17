using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Statistics;

namespace TagsCloudVisualization.UnitTests
{
    [TestFixture]
    class StatisticsTests
    {
        private IWordProcessor wordProcessor;
        private IWordSelector wordSelector;

        [SetUp]
        public void SetUp()
        {
            wordProcessor = new LowerCaseWordProcessor();
            wordSelector = new LongWordsSelector();
        }
        
        [Test]
        public void GetWordsFromSingleWord_IsSingleWord()
        {
            var lines = new[] {"abe"};

            WordStatistics.GetWordsFromLines(lines)
                .Should().BeEquivalentTo("abe");
        }

        [Test]
        public void GetWordsFromTwoWordsSeparatedBySpaces_ShouldBeTwoWords()
        {
            var lines = new[] {"cat dog"};

            WordStatistics.GetWordsFromLines(lines)
                .Should().BeEquivalentTo("cat", "dog");
        }

        [Test]
        public void GetWordsFromThreeWordsSeparatedBySpaces_ShouldBeThreeWords()
        {
            var lines = new[] {"ab c    de"};

            WordStatistics.GetWordsFromLines(lines)
                .Should().BeEquivalentTo("ab", "c", "de");
        }

        [Test]
        public void GetWordsFromTwoWordsSeparatedByDelimiters_ShouldBeTwoWords()
        {
            var lines = new[] {"first:/+&   -%#second"};

            WordStatistics.GetWordsFromLines(lines)
                .Should().BeEquivalentTo("first", "second");
        }

        [Test]
        public void GetStatisticsFromEmptyLine_ShouldBeEmpty()
        {
            var lines = new[] { "" };

            var statistics = WordStatistics
                .GenerateFrequencyStatisticsFromTextLines(lines, wordProcessor, wordSelector);
            statistics.Should().BeEmpty();
        }

        [Test]
        public void GetStatisticsFromSpecialSymbols_ShouldBeEmpty()
        {
            var lines = new[] { "+-+  :) \\" };

            var statistics = WordStatistics
                .GenerateFrequencyStatisticsFromTextLines(lines, wordProcessor, wordSelector);
            statistics.Should().BeEmpty();
        }

        [Test]
        public void GetStatisticsFromShortWords_ShouldBeEmpty()
        {
            var lines = new[] { "a b cd xyz" };

            var statistics = WordStatistics
                .GenerateFrequencyStatisticsFromTextLines(lines, wordProcessor, wordSelector);
            statistics.Should().BeEmpty();
        }

        [Test]
        public void GetStatisticsFromLongWords_ShouldContainLongWords()
        {
            var lines = new[] { "verylong orbital" };

            var statistics = WordStatistics
                .GenerateFrequencyStatisticsFromTextLines(lines, wordProcessor, wordSelector);
            statistics.Should().ContainKeys("verylong", "orbital");
        }

        [Test]
        public void GetStatisticsFromLongWords_ShouldNotContainShortWords()
        {
            var lines = new[] { "i am it" };

            var statistics = WordStatistics
                .GenerateFrequencyStatisticsFromTextLines(lines, wordProcessor, wordSelector);
            statistics.Should().BeEmpty();
        }

        [Test]
        public void GetStatisticsFromDifferentLengthWords_ShouldContainLongWords()
        {
            var lines = new[] { "If you want to know the answer, look deeply" };

            var statistics = WordStatistics
                .GenerateFrequencyStatisticsFromTextLines(lines, wordProcessor, wordSelector);
            statistics.Count.Should().Be(2);
            statistics.Should().ContainKeys("answer", "deeply");
        }


        [Test]
        public void GetStatisticsFromSomeWords_ShouldNotBeEmpty()
        {
            var lines = new[] {"first second"};

            var statistics = WordStatistics
                .GenerateFrequencyStatisticsFromTextLines(lines, wordProcessor, wordSelector);
            statistics.Should().NotBeEmpty();
        }
    }
}