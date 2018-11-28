using System.Collections.Generic;

namespace MATD.Lesson5
{
    public interface IInvertedIndex
    {
        HashSet<Document> Get(string stem);
    }
}
