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
            Console.WriteLine($"Load took {sw.Elapsed}");

            var vectorDocuments = new VectorDocumentCollection(documents);
            var querySystem = new VectorQuerySystem(vectorDocuments, new Stemmer());

            Console.WriteLine("Enter mode (q = query mode | s = similar documents | c = documents comparison):");
            string mode = Console.ReadLine();
            if (mode == "q")
            {
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
            else if (mode == "s")
            {
                Console.Write("Enter document id:");
                string input;
                while ((input = Console.ReadLine()) != "")
                {
                    if (!int.TryParse(input, out var id))
                    {
                        Console.WriteLine("Invalid document id");
                        continue;
                    }

                    sw = Stopwatch.StartNew();
                    var queryResults = querySystem.QuerySimilarDocuments(id);
                    sw.Stop();
                    foreach (var queryResult in queryResults.Take(10))
                    {
                        Console.WriteLine($"ID={queryResult.Document.Id}, Score={queryResult.Score}");
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Results found in {sw.ElapsedMilliseconds}ms");
                    Console.ResetColor();

                    Console.Write("Enter document id:");
                }
            }
            else if (mode == "c")
            {
                Console.Write("Enter at least two document ids:");
                string input;
                while ((input = Console.ReadLine()) != "")
                {
                    string[] rawIds = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (rawIds.Length < 2)
                    {
                        Console.WriteLine("Invalid document ids");
                        continue;
                    }

                    var ids = new int[rawIds.Length];
                    for (int i = 0; i < rawIds.Length; i++)
                    {
                        if (!int.TryParse(rawIds[i], out var id))
                        {
                            Console.WriteLine($"'{rawIds[i]}' is invalid document id");
                            continue;
                        }
                        ids[i] = id;
                    }

                    sw = Stopwatch.StartNew();
                    var queryResults = querySystem.QueryScoreDocuments(ids);
                    sw.Stop();
                    foreach (var queryResult in queryResults.Take(10))
                    {
                        Console.WriteLine($"D({queryResult.Document1.Id}, {queryResult.Document2.Id})={queryResult.Score}");
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Calculated in {sw.ElapsedMilliseconds}ms");
                    Console.ResetColor();

                    Console.Write("Enter at least two document ids:");
                }
            }
        }
    }
}
