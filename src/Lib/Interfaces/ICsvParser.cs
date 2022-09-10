using Lib.Models;

namespace Lib.Interfaces
{
    public interface ICsvParser<T>
    {
        ParsedItemCollection<T> GetItems(Stream stream);
    }
}
