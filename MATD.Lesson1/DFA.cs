using System;
using System.Collections.Generic;
using System.Linq;

namespace MATD.Lesson1
{
    public class DFA<TSymbol, TState>
    {
        readonly TState _startState;
        readonly TState _endState;
        readonly Dictionary<(TState, TSymbol), TState> _transitionFunctions;

        public DFA(TState start, TState end, params (TState state, TSymbol symbol, TState newState)[] transitionFunctions)
        {
            _startState = start;
            _endState = end;

            _transitionFunctions = transitionFunctions.ToDictionary(t => (t.state, t.symbol), t => t.newState);
        }

        public bool IsAccepted(IEnumerable<TSymbol> input)
        {
            var currentState = _startState;

            foreach (var symbol in input)
            {
                if (!_transitionFunctions.TryGetValue((currentState, symbol), out var newState))
                {
                    throw new Exception("Invalid DFA");
                    //return false;
                }

                currentState = newState;
            }

            return currentState.Equals(_endState);
        }
    }
}
