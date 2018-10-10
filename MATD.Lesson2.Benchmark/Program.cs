using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Linq;

namespace MATD.Lesson2.Benchmark
{
    public class PatternMatchingBenchmark
    {
        readonly string text;
        readonly string pattern;

        public PatternMatchingBenchmark()
        {
            text = new string('a', 100000) + "b";
            pattern = new string('a', 50) + "b";
        }

        [Benchmark]
        public int[] StringIndexOf() => text.AllIndexesOf(pattern).ToArray();

        [Benchmark]
        public int[] RegexMatches() => text.AllMatches(pattern).ToArray();

        [Benchmark]
        public int[] Naive() => text.Naive(pattern).ToArray();

        [Benchmark]
        public int[] BoyerMooreHorspool() => text.BoyerMooreHorspool(pattern).ToArray();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PatternMatchingBenchmark>();
        }
    }
}
