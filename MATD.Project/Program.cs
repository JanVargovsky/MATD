using MATD.Lesson5;
using MATD.Lesson8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MATD.Project
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loader = new ReutersDataLoader();
            var documents = new List<Document>();
            Console.WriteLine("Loading documents.");
            var files = Directory.GetFiles("Data/reuters21578/", "*.sgm");
            foreach (var file in files)
            {
                documents.AddRange(await loader.ParseAsync(file));
            }
            Console.WriteLine("Loading documents done.");

            Console.WriteLine("Preprocessing documents.");
            var stemmer = new Stemmer();
            var stopList = new StopList();
            var textPreprocessing = new TextPreprocessing(stopList, stemmer);
            textPreprocessing.RemoveInvalidCharacters(documents);
            textPreprocessing.SplitToTokens(documents);
            textPreprocessing.ApplyStopList(documents);
            textPreprocessing.ApplyStemming(documents);
            Console.WriteLine("Preprocessing documents done.");

            const string directory = "Data/reuters21578-preprocessed";
            var documentSaver = new DocumentSaver();

            Console.WriteLine("Saving documents.");
            await documentSaver.SaveAsync(directory, documents);
            Console.WriteLine("Saving documents done.");

            Console.WriteLine("Creating vector documents.");
            var vectorDocuments = new VectorDocumentCollection(documents);
            Console.WriteLine("Creating vector documents done.");

            Console.WriteLine("Saving vector documents.");
            await documentSaver.SaveAsync(directory, vectorDocuments, documents.Count);
            Console.WriteLine("Saving vector documents done.");

            Console.WriteLine("Done");
        }
    }
}
