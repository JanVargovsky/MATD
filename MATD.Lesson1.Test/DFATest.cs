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

            Assert.True(dfa.IsAccepted("0"));
            Assert.True(dfa.IsAccepted("000"));
            Assert.True(dfa.IsAccepted("101"));
            Assert.True(dfa.IsAccepted("1010101"));
            Assert.True(dfa.IsAccepted("01010"));

            Assert.False(dfa.IsAccepted(""));
            Assert.False(dfa.IsAccepted("1"));
            Assert.False(dfa.IsAccepted("00"));
            Assert.False(dfa.IsAccepted("10101"));

            Assert.ThrowsAny<Exception>(() => dfa.IsAccepted("invalid alphabet"));
        }
    }
}
