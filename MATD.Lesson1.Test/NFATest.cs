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

            Assert.True(nfa.IsAccepted("01"));
            Assert.True(nfa.IsAccepted("001"));
            Assert.True(nfa.IsAccepted("101"));
            Assert.True(nfa.IsAccepted("10101010101"));
            Assert.True(nfa.IsAccepted("00000000001"));

            Assert.False(nfa.IsAccepted(""));
            Assert.False(nfa.IsAccepted("0"));
            Assert.False(nfa.IsAccepted("1"));
            Assert.False(nfa.IsAccepted("00"));
            Assert.False(nfa.IsAccepted("10"));
            Assert.False(nfa.IsAccepted("11"));
            Assert.False(nfa.IsAccepted("00000000000"));
            Assert.False(nfa.IsAccepted("11111111111"));
        }
    }
}
