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

        public IEnumerable<QueryScoreResult> QueryScoreDocuments(int[] ids)
        {
            var result = new List<QueryScoreResult>();
            for (int i = 0; i < ids.Length; i++)
            {
                for (int j = i + 1; j < ids.Length; j++)
                {
                    var document1 = _documentCollection.GetDocument(ids[i]);
                    var document2 = _documentCollection.GetDocument(ids[j]);
                    var score = Score(document1.Values, document2.Values);
                    result.Add(new QueryScoreResult
                    {
                        Document1 = document1,
                        Document2 = document2,
                        Score = score,
                    });
                }
            }
            return result.OrderByDescending(t => t.Score);
        }
    }

    public class QueryResult
    {
        public VectorDocument Document { get; set; }
        public double Score { get; set; }
    }

    public class QueryScoreResult
    {
        public VectorDocument Document1 { get; set; }
        public VectorDocument Document2 { get; set; }
        public double Score { get; set; }
    }
}
