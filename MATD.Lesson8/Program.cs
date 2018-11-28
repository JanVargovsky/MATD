using MATD.Lesson5;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MATD.Lesson8
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loader = new ReutersDataLoader();
            List<Document> documents = new List<Document>();
            Stopwatch sw = Stopwatch.StartNew();
            var files = Directory.GetFiles("Data/reuters21578/", "*.sgm");
            foreach (var file in files)
            {
                documents.AddRange(await loader.ParseAsync(file));
            }
            var stemmer = new Stemmer();
            var textPreprocessing = new TextPreprocessing(new StopList(), stemmer);
            textPreprocessing.RemoveInvalidCharacters(documents);
            textPreprocessing.SplitToTokens(documents);
            textPreprocessing.ApplyStopList(documents);
            textPreprocessing.ApplyStemming(documents);
            Console.WriteLine(sw.Elapsed);

            var vectorDocuments = new VectorDocumentCollection(documents);
            var querySystem = new VectorQuerySystem(vectorDocuments, new Stemmer());

            Console.Write("Enter query:");
            string input;
            while ((input = Console.ReadLine()) != "")
            {
                var words = input.ToUpperInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                sw = Stopwatch.StartNew();
                var queryResults = querySystem.Query(words);
                sw.Stop();
                foreach (var queryResult in queryResults.Take(10))
                {
                    Console.WriteLine($"ID={queryResult.Document.Id}, Score={queryResult.Score}");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Results found in {sw.ElapsedMilliseconds}ms");
                Console.ResetColor();

                Console.Write("Enter query:");
            }
        }
    }
}
