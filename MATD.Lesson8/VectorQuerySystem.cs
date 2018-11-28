using MATD.Lesson5;
using System.Collections.Generic;
using System.Linq;

namespace MATD.Lesson8
{
    public class VectorQuerySystem
    {
        readonly VectorDocumentCollection _documentCollection;
        readonly Stemmer _stemmer;

        public VectorQuerySystem(VectorDocumentCollection documentCollection, Stemmer stemmer)
        {
            _documentCollection = documentCollection;
            _stemmer = stemmer;
        }

        double Score(int[] q, double[] d)
        {
            double value = 0;
            for (int i = 0; i < q.Length; i++)
                value += q[i] * d[i];
            return value;
        }

        public IEnumerable<QueryResult> Query(string[] words)
        {
            var stems = words.Select(_stemmer.WordToStem);

            var freeQuery = new int[_documentCollection.LengthOfVector];
            foreach (var stem in stems)
            {
                var index = _documentCollection.GetIndexOfStem(stem);
                if (index < 0)
                    continue;

                freeQuery[index] = 1;
            }

            var scores = _documentCollection
                .GetDocuments()
                .Select(doc => new QueryResult
                {
                    Document = doc,
                    Score = Score(freeQuery, doc.Values),
                })
                .OrderByDescending(t => t.Score)
                .ToArray();

            return scores;
        }
    }

    public class QueryResult
    {
        public VectorDocument Document { get; set; }
        public double Score { get; set; }
    }
}
