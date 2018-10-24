using System.Collections.Generic;
using System.Text;

namespace MATD.Lesson4
{
    public class RegexPostfixConverter
    {
        public string Convert(string regex)
        {
            var priority = new Dictionary<char, int>
            {
                ['('] = 1,
                [')'] = 1,
                ['*'] = 2,
                ['.'] = 3,
                ['|'] = 4,
            };

            var operators = new Stack<char>();
            var concats = new Stack<int>();
            int concat = 0;

            var postfix = new StringBuilder();
            foreach (var c in regex)
            {
                switch (c)
                {
                    case '(':
                        if (concat > 1)
                        {
                            concat--;
                            postfix.Append('.');
                        }
                        operators.Push(c);
                        concats.Push(concat);
                        concat = 0;
                        break;
                    case ')':
                        while (concat > 1)
                        {
                            concat--;
                            postfix.Append('.');
                        }

                        var tmp = operators.Pop();
                        while (tmp != '(')
                        {
                            postfix.Append(tmp);
                            tmp = operators.Pop();
                        }
                        concat = concats.Pop();
                        concat++;
                        break;
                    case '*':
                        postfix.Append(c);
                        break;
                    case '|':
                        while (--concat > 0)
                            postfix.Append('.');
                        var p = priority[c];
                        while (operators.Count > 0)
                        {
                            var op = operators.Peek();
                            if (p > priority[op])
                                break;
                            concat--;
                            postfix.Append(op);
                            operators.Pop();
                        }
                        operators.Push(c);
                        break;
                    default:
                        if (concat > 1)
                        {
                            concat--;
                            postfix.Append('.');
                        }
                        postfix.Append(c);
                        concat++;
                        break;
                }
            }

            while (concat-- > 1)
                postfix.Append('.');

            while (operators.Count > 0)
                postfix.Append(operators.Pop());

            return postfix.ToString();
        }
    }
}
