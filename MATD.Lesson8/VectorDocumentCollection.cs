using MATD.Lesson5;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MATD.Lesson8
{
    public class VectorDocumentCollection
    {
        readonly List<VectorDocument> _documents;
        readonly string[] _stems;

        public VectorDocumentCollection(IEnumerable<Document> documents)
        {
            _stems = GetStems(documents);
            _documents = Build(documents);
            ComputeInverseDocumentFrequency();
            Normalize();
        }

        string[] GetStems(IEnumerable<Document> documents) => documents
                .SelectMany(t => t.Stems)
                .Distinct()
                .OrderBy(t => t)
                .ToArray();

        List<VectorDocument> Build(IEnumerable<Document> documents)
        {
            var vectorDocuments = new List<VectorDocument>();

            foreach (var document in documents)
            {
                var vectorDocument = new VectorDocument
                {
                    Id = document.Id,
                    Values = new double[_stems.Length],
                };

                foreach (var stem in document.Stems)
                {
                    var index = GetIndexOfStem(stem);
                    vectorDocument.Values[index]++;
                }

                vectorDocuments.Add(vectorDocument);
            }

            return vectorDocuments;
        }

        void ComputeInverseDocumentFrequency()
        {
            // Compute inverse document frequency idf_t = log(N/df_t) where N is a number of documents in a collection
            // and 'df_t' is the number of documents that contain a term 't'.

            var N = _documents.Count;

            for (int i = 0; i < _documents[0].Values.Length; i++)
            {
                int df_t = NumberOfDocumentsForTerm(i);
                double idf_t = Math.Log10(N / df_t);

                foreach (var document in _documents)
                {
                    document.Values[i] *= idf_t;
                }
            }

            int NumberOfDocumentsForTerm(int index)
            {
                int count = 0;
                foreach (var document in _documents)
                {
                    if (document.Values[index] > 0)
                        count++;

                }
                return count;
            }
        }

        void Normalize()
        {
            foreach (var document in _documents)
            {
                VectorModelMath.Normalize(document.Values, true);
            }
        }

        public IEnumerable<VectorDocument> GetDocuments() => _documents;

        public VectorDocument GetDocument(int i) => _documents[i];

        public int GetIndexOfStem(string stem) => Array.BinarySearch(_stems, stem);

        public int LengthOfVector => _documents[0].Values.Length;
    }
}
