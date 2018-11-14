using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MATD.Lesson5
{
    class Program
    {
        static async Task ConvertQuotesContentToUpperCaseAsync(string inputFile, string outputFile)
        {
            var quotesRegex = new Regex("\"([^\"\\\\]*(\\\\.)*)*\"");
            var apostrophesRegex = new Regex("'(.)'");
            string content = await File.ReadAllTextAsync(inputFile);
            var newContent = quotesRegex.Replace(content, m => m.Value.ToUpper());
            var newContent2 = apostrophesRegex.Replace(newContent, m => m.Value.ToUpper());

            await File.WriteAllTextAsync(outputFile, newContent2);
        }

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

            var invertedIndex = new InvertedIndex(documents);
            var querySystem = new QuerySystem(invertedIndex, stemmer);


            Console.Write("Enter query:");
            string input;
            while ((input = Console.ReadLine()) != "")
            {
                var words = input.ToUpperInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var result = querySystem.Query(words);
                foreach (var doc in result)
                {
                    Console.WriteLine($"{doc.RawData.Substring(0, 50)}...");
                }

                Console.ForegroundColor = result.Count != 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"{result.Count} results");
                Console.ResetColor();

                Console.Write("Enter query:");
            }
        }
    }
}
