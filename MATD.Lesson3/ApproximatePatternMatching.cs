using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MATD.Lesson3
{
    public class ApproximatePatternMatching
    {
        readonly IEnumerable<string> _words;

        public ApproximatePatternMatching(IEnumerable<string> words)
        {
            _words = words;
        }

        [DebuggerDisplay("{C}")]
        struct State
        {
            public char C { get; set; }
            public bool IsEnd { get; set; }
        }

        bool IsWord(string input, string word)
        {
            const int E = 2;
            var states = new State[(word.Length + 1) * (E + 1)];
            for (int e = 0; e <= E; e++)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    states[i + word.Length * e + e + 1] = new State
                    {
                        C = word[i],
                        IsEnd = i == word.Length - 1
                    };
                }
            }
            var currentStates = new HashSet<int>();
            for (int e = 0; e <= E; e++)
            {
                currentStates.Add(e * (word.Length + 1) + e);
            }

            foreach (var c in input)
            {
                var newStates = new HashSet<int>();
                foreach (var state in currentStates)
                {
                    // right
                    if (state + 1 < states.Length && states[state + 1].C == c)
                    {
                        newStates.Add(state + 1);
                        for (int e = 1; e <= E; e++)
                        {
                            int index = state + 1 + e * (word.Length + 1) + e;
                            if (index < states.Length)
                                newStates.Add(index);
                        }
                    }

                    // bottom
                    for (int e = 0; e < E; e++)
                    {
                        int index = state + word.Length + 1 + e * (word.Length + 1) + e;
                        if (index < states.Length)
                        {
                            newStates.Add(index);
                        }
                    }

                    // bottom right (diagonal)
                    for (int e = 0; e < E; e++)
                    {
                        int index = state + word.Length + 2 + e * (word.Length + 1) + e;
                        if (index < states.Length)
                        {
                            newStates.Add(index);
                        }
                    }
                }
                currentStates = newStates;
            }
            bool isEnd = currentStates.Any(i => states[i].IsEnd);
            return isEnd;
        }

        public bool TryFind(string input, out string word)
        {
            foreach (var w in _words)
            {
                if (IsWord(input, w))
                {
                    word = w;
                    return true;
                }
            }

            word = null;
            return false;
        }
    }
}
