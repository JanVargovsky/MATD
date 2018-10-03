using System;
using Xunit;

namespace MATD.Lesson1.Test
{
    public class DFATest
    {
        [Fact]
        public void EvenNumbersTest()
        {
            var dfa = new DFA<char, int>(1, 2,
                (1, '0', 2),
                (1, '1', 1),
                (2, '0', 1),
                (2, '1', 2));

            Assert.True(dfa.IsAccepted("0".ToCharArray()));
            Assert.True(dfa.IsAccepted("000".ToCharArray()));
            Assert.True(dfa.IsAccepted("101".ToCharArray()));
            Assert.True(dfa.IsAccepted("1010101".ToCharArray()));
            Assert.True(dfa.IsAccepted("01010".ToCharArray()));

            Assert.False(dfa.IsAccepted("".ToCharArray()));
            Assert.False(dfa.IsAccepted("1".ToCharArray()));
            Assert.False(dfa.IsAccepted("00".ToCharArray()));
            Assert.False(dfa.IsAccepted("10101".ToCharArray()));

            Assert.ThrowsAny<Exception>(() => dfa.IsAccepted("invalid alphabet".ToCharArray()));
        }
    }
}
