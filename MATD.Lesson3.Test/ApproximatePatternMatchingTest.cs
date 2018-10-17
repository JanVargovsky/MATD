using Xunit;

namespace MATD.Lesson3.Test
{
    public class ApproximatePatternMatchingTest
    {
        readonly ApproximatePatternMatching approximatePatternMatching;

        public ApproximatePatternMatchingTest()
        {
            approximatePatternMatching = new ApproximatePatternMatching(new[]
            {
                "survey"
            });
        }

        [Theory]
        [InlineData("survey")]
        [InlineData("urvey")]
        [InlineData("rvey")]
        [InlineData("asurvey")]
        [InlineData("aasurvey")]
        [InlineData("surve")]
        [InlineData("surv")]
        [InlineData("surveya")]
        [InlineData("surveyaa")]
        [InlineData("Survey")]
        [InlineData("SUrvey")]
        [InlineData("suRvey")]
        [InlineData("suRVey")]
        [InlineData("survEy")]
        [InlineData("surveY")]
        [InlineData("survEY")]
        [InlineData("suvey")]
        [InlineData("suey")]
        public void TryFindSuccessTest(string input)
        {
            const string expected = "survey";
            var result = approximatePatternMatching.TryFind(input, out var word);
            Assert.True(result);
            Assert.Equal(expected, word);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("aaasurvey")]
        [InlineData("sur")]
        [InlineData("vey")]
        public void TryFindFailTest(string input)
        {
            var r = approximatePatternMatching.TryFind(input, out var w);
            Assert.False(r);
            Assert.Null(w);
        }
    }
}
