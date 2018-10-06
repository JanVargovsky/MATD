using System.Collections.Generic;
using System.Linq;

namespace MATD.Lesson1
{
    public class NFA<TSymbol, TState>
    {
        readonly TState _startState;
        readonly HashSet<TState> _endStates;
        readonly Dictionary<(TState, TSymbol), IEnumerable<TState>> _transitionFunctions;

        public NFA(TState start, IEnumerable<TState> end, params (TState state, TSymbol symbol, IEnumerable<TState> newStates)[] transitionFunctions)
        {
            _startState = start;
            _endStates = end.ToHashSet();

            _transitionFunctions = transitionFunctions.ToDictionary(t => (t.state, t.symbol), t => t.newStates);
        }

        public bool IsAccepted(IEnumerable<TSymbol> input)
        {
            var currentStates = new HashSet<TState>() { _startState };

            foreach (var symbol in input)
            {
                var newStates = new HashSet<TState>();

                foreach (var currentState in currentStates)
                {
                    if (_transitionFunctions.TryGetValue((currentState, symbol), out var states))
                    {
                        newStates.UnionWith(states);
                    }
                }

                currentStates = newStates;
            }

            return currentStates.Any(_endStates.Contains);
        }
    }
}
