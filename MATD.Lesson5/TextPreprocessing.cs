using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MATD.Lesson5
{
    public class TextPreprocessing
    {
        readonly static char[] NotDivisors = new[] { '\'' };
        readonly StopList _stopList;
        readonly Stemmer _porterStemmer;

        public TextPreprocessing(StopList stopList, Stemmer porterStemmer)
        {
            _stopList = stopList;
            _porterStemmer = porterStemmer;
        }

        public void RemoveInvalidCharacters(IEnumerable<Document> documents)
        {
            foreach (var doc in documents)
            {
                var stringBuilder = new StringBuilder();
                foreach (var c in doc.RawData)
                {
                    var upper = char.ToUpper(c);
                    if ((upper >= 'A' && upper <= 'Z') || (upper >= '0' && upper <= '9'))
                        stringBuilder.Append(upper);
                    else if (NotDivisors.Contains(c))
                    {
                        // do nothing e.g. it's
                    }
                    else
                        stringBuilder.Append(' ');
                }

                doc.Data = stringBuilder.ToString();
            }
        }

        public void SplitToTokens(IEnumerable<Document> documents)
        {
            foreach (var doc in documents)
                doc.Words = doc.Data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        public void ApplyStopList(IEnumerable<Document> documents)
        {
            foreach (var doc in documents)
                doc.FilteredWords = doc.Words.Where(_stopList.IsNotSkipped).ToArray();
        }

        public void ApplyStemming(IEnumerable<Document> documents)
        {
            foreach (var doc in documents)
                doc.Stems = doc.FilteredWords.Select(_porterStemmer.WordToStem).ToArray();
        }
    }
}
