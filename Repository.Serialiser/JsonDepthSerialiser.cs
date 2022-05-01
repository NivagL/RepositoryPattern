using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Repository.Serialiser;

public class CustomJsonTextWriter : JsonTextWriter
{
    public CustomJsonTextWriter(TextWriter textWriter) : base(textWriter) { }

    public int CurrentDepth { get; private set; }

    public override void WriteStartObject()
    {
        CurrentDepth++;
        base.WriteStartObject();
    }

    public override void WriteEndObject()
    {
        CurrentDepth--;
        base.WriteEndObject();
    }
}

public class CustomContractResolver : DefaultContractResolver
{
    private readonly Func<bool> _includeProperty;

    public CustomContractResolver(Func<bool> includeProperty)
    {
        _includeProperty = includeProperty;
    }

    protected override JsonProperty CreateProperty(
        MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        var shouldSerialize = property.ShouldSerialize;
        property.ShouldSerialize = obj => _includeProperty() &&
                                          (shouldSerialize == null ||
                                           shouldSerialize(obj));
        return property;
    }
}

public class JsonDepthSerialiser<T> : ISerialiser<T>
{
    public int MaxDepth { get; private set; }
    public JsonSerializerSettings Settings { get; private set; }
    public Func<T, string> CreateString { get; set; }
    public Func<IEnumerable<T>, string> CreateStrings { get; set; }
    public Func<string, T> CreateObject { get; set; }
    public Func<string, IEnumerable<T>> CreateObjects { get; set; }

    public JsonDepthSerialiser(int maxDepth = 1, JsonSerializerSettings settings = null)
    {
        MaxDepth = maxDepth;
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
        using (var strWriter = new StringWriter())
        {
            using (var jsonWriter = new CustomJsonTextWriter(strWriter))
            {
                Func<bool> include = () => jsonWriter.CurrentDepth <= MaxDepth;
                var resolver = new CustomContractResolver(include);
                JsonSerializer serializer = null;
                if (Settings == null)
                {
                    serializer = new JsonSerializer() 
                    { 
                        ContractResolver = resolver,
                        Formatting = Formatting.Indented
                    };
                }
                else
                {
                    serializer = new JsonSerializer()
                    {
                        ContractResolver = resolver,
                        Formatting = Settings.Formatting,
                    };
                }
                serializer.Serialize(jsonWriter, item);
            }
            return strWriter.ToString();
        }
    }

    private string GetStringImpl(IEnumerable<T> list)
    {
        using (var strWriter = new StringWriter())
        {
            using (var jsonWriter = new CustomJsonTextWriter(strWriter))
            {
                Func<bool> include = () => jsonWriter.CurrentDepth <= MaxDepth;
                var resolver = new CustomContractResolver(include);
                JsonSerializer serializer = null;
                if (Settings == null)
                {
                    serializer = new JsonSerializer()
                    {
                        ContractResolver = resolver,
                        Formatting = Formatting.Indented
                    };
                }
                else
                {
                    serializer = new JsonSerializer()
                    {
                        ContractResolver = resolver,
                        Formatting = Settings.Formatting,
                    };
                }
                serializer.Serialize(jsonWriter, list);
            }
            return strWriter.ToString();
        }
    }
}
