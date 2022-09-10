namespace Lib.Models
{
    public class ParsedItem<T>
    {
        public T Item { get; set; }
        public string RawLine { get; set; }
        public bool IsNotUnique { get; set; }
        public string ErrorMessage { get; set; }

    }
}
