using System.Collections.Generic;

namespace MATD.Lesson2
{
    public static class PatternMatching
    {
        public static IEnumerable<int> Naive(this string text, string pattern)
        {
            for (int i = 0; i < text.Length - pattern.Length + 1; i++)
            {
                bool found = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (text[i + j] != pattern[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    yield return i;
            }
        }

        public static IEnumerable<int> BoyerMooreHorspool(this string text, string pattern)
        {
            var T = new int[255];

            void Preprocess()
            {
                for (int j = 0; j < T.Length; j++)
                    T[j] = pattern.Length;

                for (int j = 0; j < pattern.Length - 1; j++)
                    T[pattern[j]] = pattern.Length - 1 - j;
            }

            Preprocess();

            int skip = 0;
            int i;
            while (text.Length - skip >= pattern.Length)
            {
                i = pattern.Length - 1;
                while (text[skip + i] == pattern[i])
                {
                    if (i == 0)
                    {
                        yield return skip;
                        break;
                    }
                    i--;
                }
                skip += T[text[skip + pattern.Length - 1]];
            }
        }
    }
}
