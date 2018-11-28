using System.Collections.Generic;

namespace MATD.Lesson5
{
    public class InvertedIndex : IInvertedIndex
    {
        // <term, docs>
        readonly Dictionary<string, HashSet<Document>> _values;

        public InvertedIndex(IEnumerable<Document> documents)
        {
            _values = Build(documents);
        }

        Dictionary<string, HashSet<Document>> Build(IEnumerable<Document> documents)
        {
            var result = new Dictionary<string, HashSet<Document>>();

            foreach (var document in documents)
                foreach (var term in document.Stems)
                {
                    if (!result.TryGetValue(term, out var docs))
                    {
                        result[term] = docs = new HashSet<Document>();
                    }
                    docs.Add(document);
                }

            return result;
        }

        public HashSet<Document> Get(string stem)
        {
            if (_values.TryGetValue(stem, out var documents))
                return documents;

            return new HashSet<Document>();
        }
    }
}
