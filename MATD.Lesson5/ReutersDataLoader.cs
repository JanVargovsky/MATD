using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MATD.Lesson5
{
    public class ReutersDataLoader
    {
        static readonly Regex TextRegex;
        static readonly Regex TitleRegex;
        static readonly Regex BodyRegex;

        static ReutersDataLoader()
        {
            var options = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase;
            string GetTagRegex(string name) => $"<{name}*>(.*?)<\\/{name}>";
            TextRegex = new Regex(GetTagRegex("TEXT"), options);
            TitleRegex = new Regex(GetTagRegex("TITLE"), options);
            BodyRegex = new Regex(GetTagRegex("BODY"), options);
        }

        public async Task<IEnumerable<Document>> ParseAsync(string path)
        {
            var text = await File.ReadAllTextAsync(path);
            return Parse(text);
        }

        public IEnumerable<Document> Parse(string inputText)
        {
            var textMatches = TextRegex.Matches(inputText);
            foreach (Match textMatch in textMatches)
            {
                var text = textMatch.Groups[1].Value;
                var titleMatch = TitleRegex.Match(text);
                var bodyMatch = BodyRegex.Match(text);
                var title = titleMatch.Groups[1].Value;
                var body = bodyMatch.Groups[1].Value;
                yield return new Document
                {
                    RawData = $"{title} {body}"
                };
            }

            //var xmlDocument = new XmlDocument();
            //xmlDocument.LoadXml(xml);

            //foreach (XmlNode reuters in xmlDocument.GetElementsByTagName("REUTERS"))
            //{
            //    var text = reuters["TEXT"];
            //    var title = text.Attributes["TITLE"];
            //    var body = text.Attributes["BODY"];
            //    yield return new Document(title.Value + body.Value);
            //}
        }
    }
}
