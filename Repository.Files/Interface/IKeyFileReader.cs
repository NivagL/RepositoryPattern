using System.Linq.Expressions;

namespace Repository.Files;

public interface IKeyFileReader<TKey, TValue>
{
    Tuple<TKey, TValue> ReadOne(TKey id, string subFolder = "");
    IDictionary<TKey, TValue> ReadAll(string subFolder = "");
    IDictionary<TKey, TValue> Read(Expression<Func<TValue, bool>> expression, string subFolder = "");
}
