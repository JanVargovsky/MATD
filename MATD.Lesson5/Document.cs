namespace MATD.Lesson5
{
    public class Document
    {
        public string RawData { get; set; }

        public string Data { get; set; }

        public string[] Words { get; set; }

        public string[] FilteredWords { get; set; }

        public string[] Stems { get; set; }
    }
}
