using Newtonsoft.Json.Serialization;

namespace Repository.Serialiser;

public class JsonLowercaseSerializer : DefaultContractResolver
{
    protected override string ResolvePropertyName(string propertyName)
    {
        return propertyName.ToLower();
    }
}
