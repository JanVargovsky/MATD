using System.Collections.Generic;
using System.Linq;

namespace MATD.Lesson5
{
    public class QuerySystem
    {
        readonly InvertedIndex _invertedIndex;
        readonly Stemmer _stemmer;

        public QuerySystem(InvertedIndex invertedIndex, Stemmer stemmer)
        {
            _invertedIndex = invertedIndex;
            _stemmer = stemmer;
        }

        public ISet<Document> Query(string[] words)
        {
            var stems = words.Select(_stemmer.WordToStem);

            HashSet<Document> documents = null;
            foreach (var stem in stems)
            {
                if (documents == null)
                {
                    documents = new HashSet<Document>(_invertedIndex.Get(stem));
                    continue;
                }

                var newDocuments = _invertedIndex.Get(stem);
                documents.IntersectWith(newDocuments);
            }

            return documents;
        }
    }
}
