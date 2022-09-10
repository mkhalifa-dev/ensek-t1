namespace Lib.Models
{
    public class ParsedItemCollection<T> : List<ParsedItem<T>>
    {
        public ParsedItemCollection(IEnumerable<ParsedItem<T>> items):base(items)
        {

        }
        public ParsedItemCollection()
        {

        }
    }
}
