using System.Linq.Expressions;

namespace Repository.Files;

public interface IValueFileDeleter<TValue>
{
    Task<bool> Delete(TValue value, string subFolder = "");
    int DeleteAll(string subFolder = "");
    IEnumerable<TValue> DeleteQuery(Expression<Func<TValue, bool>> expression, string subFolder = "");
}
