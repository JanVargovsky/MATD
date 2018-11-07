namespace MATD.Lesson5
{
    public class Stemmer
    {
        readonly PorterStemmerUpper _porterStemmer;

        // e.g.
        // TROUBLE
        // CCVVCCV
        // join same symbols
        // CVCV
        // [C] (VC)^m [V] ... m = 1

        // TROUBLES ... m = 2

        public Stemmer()
        {
            _porterStemmer = new PorterStemmerUpper();
        }

        // word = term
        public string WordToStem(string word) => _porterStemmer.StemWord(word);

        // regex "([^"]*)" -> to upper
        // Source: https://tartarus.org/martin/PorterStemmer/
    }
}
