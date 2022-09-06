using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model.Abstraction;

namespace Repository.Files;

public class FileWriter<TKey, TValue> 
    : FileBase<TKey, TValue>
    , IFileWriter<TKey, TValue>
    where TValue : class
    where TKey : notnull
{
    public FileWriter(IConfiguration configuration, ILoggerFactory loggerFactory,
        IKeyModel<TKey, TValue> model, string dataDirectory
        )
        : base(configuration, loggerFactory, model, dataDirectory)
    {
    }

    public void WriteAll(IEnumerable<TValue> list, string subFolder = "")
    {
        foreach(var item in list)
        {
            Write(item, subFolder);
        }
    }

    public TKey Write(TValue value, string subFolder = "")
    {
        var fileDesc = GetFileName(value, subFolder);
        if (!Directory.Exists(fileDesc.Item1))
        {
            Directory.CreateDirectory(fileDesc.Item1);
        }

        CopyFile(fileDesc, "Replaced");

        using (StreamWriter file = File.CreateText(fileDesc.Item3))
        {
            string s = KeyModel.ValueSerialiser.CreateString(value);
            file.Write(s);
            return KeyModel.GetKey(value);
        }
    }
}
