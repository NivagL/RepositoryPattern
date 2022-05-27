using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Model;
using System.Linq.Expressions;

namespace Repository.Files;

public class FileReader<TKey, TValue> 
    : FileBase<TKey, TValue>
    , IFileReader<TKey, TValue>
    where TValue : class
    where TKey : notnull
{
    public FileReader(IConfiguration configuration, ILoggerFactory loggerFactory,
        IKeyModel<TKey, TValue> keyModel, string dataDirectory
        )
        : base(configuration, loggerFactory, keyModel, dataDirectory)
    {
    }

    public bool Any(string subFolder = "")
    {
        var directory = GetDirectory(subFolder);
        var files = GetFileNameWildcard();
        var path = $"{directory}\\{files}";
        return Directory.GetFiles(path).Any();
    }

    public TValue Read(TKey key, string subFolder = "")
    {
        var value = default(TValue);

        var directory = GetDirectory(subFolder);
        var fileName = GetFileName(key, subFolder);

        using (var file = new FileStream(fileName.Item3, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            var r = new StreamReader(file);
            string s = r.ReadToEnd();
            value = KeyModel.ValueSerialiser.CreateObject(s);
        }

        return value;
    }

    public IEnumerable<TValue> ReadAll(string subFolder = "")
    {
        var read = new List<TValue>();

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
                read.Add(value);
            }
        }
        return read;
    }

    public IEnumerable<TValue> ReadQuery(Expression<Func<TValue, bool>> expression, string subFolder = "")
    {
        var read = new List<TValue>();
        
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
                    read.Add(value);
                }
            }
        }
        return read;
    }
}
