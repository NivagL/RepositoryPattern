using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Model;

namespace Repository.Files;

public abstract class FileBase<TKey, TValue>
    where TValue : class
    where TKey : notnull
{
    protected readonly IKeyModel<TKey, TValue> KeyModel;
    protected readonly string DataDirectory;

    public FileBase(IConfiguration configuration
        , ILoggerFactory loggerFactory
        , IKeyModel<TKey, TValue> keyModel
        , string dataDirectory
        )
    {
        KeyModel = keyModel;
        DataDirectory = dataDirectory;
    }

    public string GetDirectory(string subFolder)
    {
        if (string.IsNullOrEmpty(subFolder))
            return DataDirectory;
        else
            return "{Directory}\\{subFolder}";
    }

    public Tuple<string, string, string> GetFileName(TKey key, string subFolder)
    {
        var directory = GetDirectory(subFolder);
        var file = GetFileName(key);
        string fullPath = $"{directory}\\{GetFileName(key)}";
        return new Tuple<string, string, string>(directory, file, fullPath);
    }

    private string GetFileName(TKey key)
    {
        var keyStr = KeyModel.KeySerialiser.CreateString(key);
        var fileStr = $"{KeyModel.ValueTypeName}_{keyStr}.json";
        return CleanFileName(fileStr);
    }

    public void CopyFile(Tuple<string, string, string> fileDesc, string subFolder)
    {
        if (File.Exists(fileDesc.Item3))
        {
            var replacedFolder = string.Format("{0}\\{1}", fileDesc.Item1, subFolder);
            if (!Directory.Exists(replacedFolder))
            {
                Directory.CreateDirectory(replacedFolder);
            }
            var replacedFile = string.Format("{0}\\{1}_{2}", replacedFolder, DateTime.Now.Ticks, fileDesc.Item2);
            File.Copy(fileDesc.Item3, replacedFile);
            File.Delete(fileDesc.Item3);
        }
    }

    public Tuple<TKey, TValue> ReadOne(TKey key, string subFolder = "")
    {
        var fileDesc = GetFileName(key, subFolder);
        var fullPath = fileDesc.Item3;

        if (!File.Exists(fullPath))
            throw new Exception("Could not find file");

        using (var file = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {

            var r = new StreamReader(file);
            string s = r.ReadToEnd();
            var value = KeyModel.ValueSerialiser.CreateObject(s);
            return new Tuple<TKey, TValue>(key, value);
        }
    }

    public Tuple<TKey, TValue> ReadOne(string fullPath)
    {
        using (var file = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            var r = new StreamReader(file);
            string s = r.ReadToEnd();
            TValue value = KeyModel.ValueSerialiser.CreateObject(s);
            TKey key = KeyModel.GetKey(value);
            return new Tuple<TKey, TValue>(key, value);
        }
    }

    public Tuple<string, string, string> GetFileName(TValue value, string subFolder)
    {
        var key = KeyModel.GetKey(value);
        return GetFileName(key, subFolder);
    }

    public string GetSearch()
    {
        string search = string.Format("{0}*{1}", KeyModel.ValueTypeName, "json");
        return search;
    }

    public string CleanFileName(string fileName)
    {
        return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
    }
}
