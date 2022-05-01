using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Model;
using System.Linq.Expressions;

namespace Repository.Files;

public class FileDeleter<TKey, TValue> 
    : FileBase<TKey, TValue>
    , IKeyFileDeleter<TKey, TValue>
    where TValue : class
    where TKey : notnull
{
    public IKeyFileReader<TKey, TValue> FileReader { get; set; }

    public FileDeleter(IConfiguration configuration, ILoggerFactory loggerFactory,
        IKeyModel<TKey, TValue> model, IKeyFileReader<TKey, TValue> fileReader,
        string dataDirectory)
        : base(configuration, loggerFactory, model, dataDirectory)
    {
        FileReader = fileReader;
    }

    public Tuple<TKey, TValue> Delete(TKey key, string subFolder = "")
    {
        var fileDesc = GetFileName(key, subFolder);
        CopyFile(fileDesc, "Deleted");
        if(File.Exists(fileDesc.Item3))
        { 
            File.Delete(fileDesc.Item3);
        }
        return Tuple.Create(key, default(TValue));
    }

    public int DeleteAll(string subFolder = "")
    {
        int counter = 0;
        var values = FileReader.ReadAll(subFolder);
        foreach(var item in values)
        {
            Delete(item.Key, subFolder);
            counter++;
        }
        return counter;
    }

    public IDictionary<TKey, TValue> DeleteQuery(Expression<Func<TValue, bool>> expression, string subFolder = "")
    {
        var deleted = new Dictionary<TKey, TValue>();
        var values = FileReader.ReadAll(subFolder);
        foreach (var item in values)
        {
            var func = expression.Compile();
            if (func(item.Value))
            {
                deleted.Add(item.Key, item.Value);
                Delete(item.Key, subFolder);
            }
        }
        return deleted;
    }
}
