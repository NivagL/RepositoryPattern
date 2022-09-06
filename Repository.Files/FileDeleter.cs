using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model.Abstraction;
using System.Linq.Expressions;

namespace Repository.Files;

public class FileDeleter<TKey, TValue> 
    : FileBase<TKey, TValue>
    , IFileDeleter<TKey, TValue>
    where TValue : class
    where TKey : notnull
{
    public IFileReader<TKey, TValue> FileReader { get; set; }

    public FileDeleter(IConfiguration configuration, ILoggerFactory loggerFactory,
        IKeyModel<TKey, TValue> model, IFileReader<TKey, TValue> fileReader,
        string dataDirectory)
        : base(configuration, loggerFactory, model, dataDirectory)
    {
        FileReader = fileReader;
    }

    public TValue Delete(TKey key, string subFolder = "")
    {
        var value = FileReader.Read(key, subFolder);
        if(value != null)
        {
            var fileName = GetFileName(key, subFolder);
            File.Delete(fileName.Item3);
        }
        return value;
    }

    public int DeleteAll(string subFolder = "")
    {
        int counter = 0;
        var values = FileReader.ReadAll(subFolder);
        foreach(var item in values)
        {
            var key = KeyModel.GetKey(item);
            Delete(key, subFolder);
            counter++;
        }
        return counter;
    }

    public IEnumerable<TValue> DeleteQuery(Expression<Func<TValue, bool>> expression, string subFolder = "")
    {
        var deleted = new List<TValue>();
        var values = FileReader.ReadAll(subFolder);
        foreach (var item in values)
        {
            var func = expression.Compile();
            if (func(item))
            {
                deleted.Add(item);
                var key = KeyModel.GetKey(item);
                Delete(key, subFolder);
            }
        }
        return deleted;
    }
}
