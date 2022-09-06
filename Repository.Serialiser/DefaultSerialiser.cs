using Newtonsoft.Json;

namespace Model.Serialiser;

public class DefaultSerialiser<T> : JsonSerialiser<T>
{
    public DefaultSerialiser(JsonSerializerSettings settings = null)
        : base(settings)
    {
    }
}
