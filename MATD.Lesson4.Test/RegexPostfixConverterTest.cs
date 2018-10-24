using Xunit;

namespace MATD.Lesson4.Test
{
    public class RegexPostfixConverterTest
    {
        readonly RegexPostfixConverter converter = new RegexPostfixConverter();

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        public void EdgeCasesTest(string regex, string postfix)
        {
            var result = converter.Convert(regex);
            Assert.Equal(postfix, result);
        }

        [Theory]
        [InlineData("ab", "ab.")]
        [InlineData("abc", "ab.c.")]
        public void ConcatenationTest(string regex, string postfix)
        {
            var result = converter.Convert(regex);
            Assert.Equal(postfix, result);
        }

        [Theory]
        [InlineData("a|b", "ab|")]
        [InlineData("a|b|c", "ab|c|")]
        public void UnionTest(string regex, string postfix)
        {
            var result = converter.Convert(regex);
            Assert.Equal(postfix, result);
        }

        [Theory]
        [InlineData("a*", "a*")]
        [InlineData("a*b*", "a*b*.")]
        public void KleeneStarTest(string regex, string postfix)
        {
            var result = converter.Convert(regex);
            Assert.Equal(postfix, result);
        }

        [Theory]
        [InlineData("a|b*c", "ab*c.|")]
        [InlineData("ab|cd", "ab.cd.|")]
        public void CombinedTest(string regex, string postfix)
        {
            var result = converter.Convert(regex);
            Assert.Equal(postfix, result);
        }

        [Theory]
        [InlineData("(a)", "a")]
        [InlineData("((a))", "a")]
        [InlineData("((a)(b))", "ab.")]
        [InlineData("(a*)", "a*")]
        [InlineData("(a)*", "a*")]
        [InlineData("(ab)", "ab.")]
        [InlineData("(a|b)", "ab|")]
        [InlineData("(a*|b*)", "a*b*|")]
        [InlineData("(a|b)*", "ab|*")]
        [InlineData("((ab)|(cd))*", "ab.cd.|*")]
        [InlineData("((ab)*(cd)*)", "ab.*cd.*.")]
        public void ParenthesisTest(string regex, string postfix)
        {
            var result = converter.Convert(regex);
            Assert.Equal(postfix, result);
        }
    }
}
