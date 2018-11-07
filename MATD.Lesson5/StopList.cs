using System.Collections.Generic;
using System.Linq;

namespace MATD.Lesson5
{
    public class StopList
    {
        readonly HashSet<string> _words = new HashSet<string>(new[] {
            "a", "an", "and", "are", "as", "at", "be", "by", "for", "from",
            "has", "he", "in", "is", "it", "its", "of", "on", "that", "the",
            "to", "was", "were", "will", "with",
        }.Select(t => t.ToUpper()));

        public bool IsNotSkipped(string @string) => !_words.Contains(@string);
    }
}
