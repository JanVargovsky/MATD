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

        double Score(double[] q, double[] d) => VectorModelMath.DotProduct(q, d);

        public IEnumerable<QueryResult> Query(string[] words)
        {
            var stems = words.Select(_stemmer.WordToStem);

            var freeQuery = new double[_documentCollection.LengthOfVector];
            foreach (var stem in stems)
            {
                var index = _documentCollection.GetIndexOfStem(stem);
                if (index < 0)
                    continue;

                freeQuery[index] = 1;
            }
            VectorModelMath.Normalize(freeQuery, true);

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

        public IEnumerable<QueryResult> QuerySimilarDocuments(int index)
        {
            var document = _documentCollection.GetDocument(index);
            var freeQuery = document.Values;

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
