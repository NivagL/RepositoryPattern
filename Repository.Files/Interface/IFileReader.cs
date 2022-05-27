using System.Linq.Expressions;

namespace Repository.Files;

public interface IFileReader<TKey, TValue>
{
    bool Any(string subFolder = "");
    TValue Read(TKey id, string subFolder = "");
    IEnumerable<TValue> ReadAll(string subFolder = "");
    IEnumerable<TValue> ReadQuery(Expression<Func<TValue, bool>> expression, string subFolder = "");
    //IDictionary<TKey, TValue> ReadAll(string subFolder = "");
    //IDictionary<TKey, TValue> ReadAll(Expression<Func<TValue, bool>> expression, string subFolder = "");
}
