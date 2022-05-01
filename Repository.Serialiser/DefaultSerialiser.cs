using Newtonsoft.Json;

namespace Repository.Serialiser
{
    public class DefaultSerialiser<T> : JsonSerialiser<T>
    {
        public DefaultSerialiser(JsonSerializerSettings settings = null)
            : base(settings)
        {
        }
    }
}
