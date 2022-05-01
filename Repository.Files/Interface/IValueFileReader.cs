using System.Linq.Expressions;

namespace Repository.Files;

public interface IValueFileReader<TValue>
{
    IEnumerable<TValue> Read(string subFolder = "");
    IEnumerable<TValue> Read(Expression<Func<TValue, bool>> expression, string subFolder = "");
}
