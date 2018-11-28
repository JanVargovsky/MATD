using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MATD.Lesson5
{
    public class InvertedIndexOnDisc : IInvertedIndex
    {
        readonly string _keys;
        readonly OffsetValue[] _values;
        readonly Stream _documentsStream;

        public InvertedIndexOnDisc(IEnumerable<Document> documents)
        {
            (_keys, _values, _documentsStream) = Build(documents);
        }

        class OffsetValue
        {
            public int KeyOffset { get; set; }
            public long FileOffset { get; set; }
        }

        (string key, OffsetValue[], Stream) Build(IEnumerable<Document> documents)
        {
            var sb = new StringBuilder();
            var stems = documents.SelectMany(t => t.Stems).Distinct().OrderBy(t => t);
            var offsets = new List<OffsetValue>();
            foreach (var stem in stems)
            {
                offsets.Add(new OffsetValue
                {
                    KeyOffset = sb.Length,
                });
                sb.Append(stem);
            }
            offsets.Add(new OffsetValue
            {
                KeyOffset = sb.Length,
                FileOffset = -1,
            });

            var tmp = new Dictionary<string, HashSet<Document>>();
            foreach (var document in documents)
                foreach (var term in document.Stems)
                {
                    if (!tmp.TryGetValue(term, out var docs))
                    {
                        tmp[term] = docs = new HashSet<Document>();
                    }
                    docs.Add(document);
                }

            var stream = File.Open("inverted-index.bin", FileMode.Create, FileAccess.ReadWrite);

            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
                for (int i = 0; i < offsets.Count - 1; i++)
                {
                    offsets[i].FileOffset = stream.Position;
                    var startIndex = offsets[i].KeyOffset;
                    var length = offsets[i+ 1].KeyOffset - offsets[i].KeyOffset;
                    var stem = sb.ToString(startIndex, length);
                    writer.Write(tmp[stem].Count);
                    foreach (var document in tmp[stem])
                        writer.Write(document.Id);
                }

            return (sb.ToString(), offsets.ToArray(), stream);
        }

        string GetKey(int index)
        {
            int from = _values[index].KeyOffset;
            int to = _values[index + 1].KeyOffset;
            return _keys.Substring(from, to - from);
        }

        int GetIndex(string stem)
        {
            int left = 0;
            int right = _values.Length - 2;

            while (left <= right)
            {
                var middleIndex = (left + right) / 2;
                var middle = GetKey(middleIndex);
                int compareValue = middle.CompareTo(stem);
                if (compareValue == -1)
                {
                    left = middleIndex + 1;
                }
                else if (compareValue == 1)
                {
                    right = middleIndex - 1;
                }
                else
                    return middleIndex;
            }

            return -1;
        }

        IEnumerable<int> LoadDocumentIds(long offset)
        {
            _documentsStream.Position = offset;

            using (var reader = new BinaryReader(_documentsStream, Encoding.UTF8, true))
            { 
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    var documentId = reader.ReadInt32();
                    yield return documentId;
                }
            }
        }

        public HashSet<Document> Get(string stem)
        {
            var index = GetIndex(stem);
            if (index == -1)
                return new HashSet<Document>();

            var value = _values[index];
            var documentIds = LoadDocumentIds(value.FileOffset);
            var documents = documentIds.Select(id => new Document { Id = id });
            var result = new HashSet<Document>(documents);
            return result;
        }
    }
}
