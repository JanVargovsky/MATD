using Xunit;

namespace MATD.Lesson2.Test
{
    public class PatternMatchingTest
    {
        class PatternMatchingTestData : TheoryData<string, string, int[]>
        {
            public PatternMatchingTestData()
            {
                Add("a", "b");
                Add("aaaa", "aa", 0, 1, 2);
                Add("adabcdabcab", "abc", 2, 6);
                Add("abcdabbdb", "bdb", 6);
                Add(new string('a', 999) + "b", new string('a', 99) + "b", 900);
            }

            new void Add(string t, string p, params int[] expected)
            {
                base.Add(t, p, expected);
            }
        }

        [Theory]
        [ClassData(typeof(PatternMatchingTestData))]
        public void AllIndexesOfTest(string t, string p, params int[] expected)
        {
            var actual = PatternMatching.AllIndexesOf(t, p);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(PatternMatchingTestData))]
        public void AllMatchesTest(string t, string p, params int[] expected)
        {
            var actual = PatternMatching.AllMatches(t, p);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(PatternMatchingTestData))]
        public void NaiveTest(string t, string p, params int[] expected)
        {
            var actual = PatternMatching.Naive(t, p);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(PatternMatchingTestData))]
        public void BoyerMooreHorspoolTest(string t, string p, params int[] expected)
        {
            var actual = PatternMatching.BoyerMooreHorspool(t, p);
            Assert.Equal(expected, actual);
        }
    }
}
