using Newtonsoft.Json;

namespace Model.Serialiser;

public class JsonSerialiser<T> : ISerialiser<T>
{
    public JsonSerializerSettings Settings { get; private set; }
    
    public Func<T, string> CreateString { get; set; }
    public Func<IEnumerable<T>, string> CreateStrings { get; set; }
    public Func<string, T> CreateObject { get; set; }
    public Func<string, IEnumerable<T>> CreateObjects { get; set; }

    public JsonSerialiser(JsonSerializerSettings settings = null)
    {
        Settings = settings;
        CreateObject = CreateObjectImpl;
        CreateObjects = CreateObjectsImpl;
        CreateString = GetStringImpl;
        CreateStrings = GetStringImpl;
    }

    private T CreateObjectImpl(string json)
    {
        if (Settings == null)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        else
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }

    private IEnumerable<T> CreateObjectsImpl(string json)
    {
        if (Settings == null)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        else
        {
            return JsonConvert.DeserializeObject<List<T>>(json, Settings);
        }
    }

    private string GetStringImpl(T item)
    {
        if (Settings == null)
        {
            return JsonConvert.SerializeObject(item, Formatting.Indented);
        }
        else
        {
            return JsonConvert.SerializeObject(item, /*Formatting.Indented,*/ Settings);
        }
    }

    private string GetStringImpl(IEnumerable<T> list)
    {
        if (Settings == null)
        {
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }
        else
        {
            return JsonConvert.SerializeObject(list, /*Formatting.Indented,*/ Settings);
        }
    }
}
