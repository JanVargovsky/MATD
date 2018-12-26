using MATD.Lesson5;
using MATD.Lesson8;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MATD.Project
{
    public class DocumentSaver
    {
        public async Task SaveAsync(string directory, IEnumerable<Document> documents)
        {
            foreach (var document in documents)
            {
                await SaveRawAsync(directory, document);
                await SaveStemsAsync(directory, document);
            }
        }

        async Task SaveRawAsync(string directory, Document document)
        {
            string fullPath = Path.GetFullPath(directory) + $"/{document.Id}.raw.txt";
            using (var writer = new StreamWriter(fullPath))
            {
                await writer.WriteAsync(document.RawData);
                await writer.FlushAsync();
            }
        }

        async Task SaveStemsAsync(string directory, Document document)
        {
            string fullPath = Path.GetFullPath(directory) + $"/{document.Id}.stems.txt";
            using (var writer = new StreamWriter(fullPath))
            {
                foreach (var item in document.Stems)
                {
                    await writer.WriteLineAsync(item);
                }
                await writer.FlushAsync();
            }
        }

        public async Task SaveAsync(string directory, VectorDocumentCollection documents, int? count)
        {
            string fileName = !count.HasValue ? "vector-documents.csv" : $"vector-documents-{count}.csv";
            string fullPath = Path.GetFullPath(directory) + '/' + fileName;
            using (var writer = new StreamWriter(fullPath))
            {
                const char separator = ';';
                string header = string.Join(separator, documents.Stems);
                await writer.WriteLineAsync(header);

                foreach (var document in documents.GetDocuments())
                {
                    string row = string.Join(separator, document.Values);
                    await writer.WriteLineAsync(row);
                }

                await writer.FlushAsync();
            }
        }
    }
}
