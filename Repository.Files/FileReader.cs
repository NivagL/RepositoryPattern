using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Model;
using System.Linq.Expressions;

namespace Repository.Files;

public class FileReader<TKey, TValue> 
    : FileBase<TKey, TValue>
    , IKeyFileReader<TKey, TValue>
    where TValue : class
    where TKey : notnull
{
    public FileReader(IConfiguration configuration, ILoggerFactory loggerFactory,
        IKeyModel<TKey, TValue> keyModel, string dataDirectory
        )
        : base(configuration, loggerFactory, keyModel, dataDirectory)
    {
    }

    public IDictionary<TKey, TValue> ReadAll(string subFolder = "")
    {
        var read = new Dictionary<TKey, TValue>();

        var directory = GetDirectory(subFolder);
        if (!Directory.Exists(directory))
            return read;

        foreach (var fileName in Directory.GetFiles(directory))
        {
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var r = new StreamReader(file);
                string s = r.ReadToEnd();
                var value = KeyModel.ValueSerialiser.CreateObject(s);
                var key = KeyModel.GetKey(value);
                read.Add(key, value);
            }
        }
        return read;
    }

    public IDictionary<TKey, TValue> Read(Expression<Func<TValue, bool>> expression, string subFolder = "")
    {
        var read = new Dictionary<TKey, TValue>();
        
        var directory = GetDirectory(subFolder);
        if (!Directory.Exists(directory))
            return read;

        foreach (var fileName in Directory.GetFiles(directory))
        {
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var r = new StreamReader(file);
                string s = r.ReadToEnd();
                var value = KeyModel.ValueSerialiser.CreateObject(s);
                var key = KeyModel.GetKey(value);

                var func = expression.Compile();
                if (func(value))
                {
                    read.Add(key, value);
                }
            }
        }
        return read;
    }
}
