using System.Linq.Expressions;

namespace Repository.Files;

public interface IFileDeleter<TKey, TValue>
{
    TValue Delete(TKey key, string subFolder = "");
    int DeleteAll(string subFolder = "");
    IEnumerable<TValue> DeleteQuery(Expression<Func<TValue, bool>> expression, string subFolder = "");
}
