using Xunit;

namespace MATD.Lesson1.Test
{
    public class NFATest
    {
        [Fact]
        public void EndingWith01Test()
        {
            var nfa = new NFA<char, int>(1, new[] { 3 },
                (1, '0', new[] { 1, 2 }),
                (1, '1', new[] { 1 }),
                (2, '1', new[] { 3 }));

            Assert.True(nfa.IsAccepted("01".ToCharArray()));
            Assert.True(nfa.IsAccepted("001".ToCharArray()));
            Assert.True(nfa.IsAccepted("101".ToCharArray()));
            Assert.True(nfa.IsAccepted("10101010101".ToCharArray()));
            Assert.True(nfa.IsAccepted("00000000001".ToCharArray()));

            Assert.False(nfa.IsAccepted("".ToCharArray()));
            Assert.False(nfa.IsAccepted("0".ToCharArray()));
            Assert.False(nfa.IsAccepted("1".ToCharArray()));
            Assert.False(nfa.IsAccepted("00".ToCharArray()));
            Assert.False(nfa.IsAccepted("10".ToCharArray()));
            Assert.False(nfa.IsAccepted("11".ToCharArray()));
            Assert.False(nfa.IsAccepted("00000000000".ToCharArray()));
            Assert.False(nfa.IsAccepted("11111111111".ToCharArray()));
        }
    }
}
