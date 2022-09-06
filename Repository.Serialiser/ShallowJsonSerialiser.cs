using Newtonsoft.Json;

namespace Model.Serialiser;

public class ShallowJsonSerialiser<T> : ISerialiser<T>
{
    public JsonSerializerSettings Settings { get; private set; }

    public virtual Func<T, string> CreateString { get; set; }
    public virtual Func<IEnumerable<T>, string> CreateStrings { get; set; }
    public virtual Func<string, T> CreateObject { get; set; }
    public virtual Func<string, IEnumerable<T>> CreateObjects { get; set; }

    public ShallowJsonSerialiser(JsonSerializerSettings settings = null)
    {
        Settings = settings;

        CreateString = GetStringImpl;
        CreateStrings = GetStringsImpl;
        CreateObject = GetObjectImpl;
        CreateObjects = GetObjectsImpl;
    }

    public T GetObjectImpl(string json)
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

    public IEnumerable<T> GetObjectsImpl(string json)
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

    public string GetStringImpl(T item)
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

    public string GetStringsImpl(IEnumerable<T> list)
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

