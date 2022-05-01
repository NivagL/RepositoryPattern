using System.Linq.Expressions;

namespace Repository.Files;

public interface IKeyFileDeleter<TKey, TValue>
{
    Tuple<TKey, TValue> Delete(TKey key, string subFolder = "");
    int DeleteAll(string subFolder = "");
    IDictionary<TKey, TValue> DeleteQuery(Expression<Func<TValue, bool>> expression, string subFolder = "");
}
