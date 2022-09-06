using Newtonsoft.Json.Serialization;

namespace Model.Serialiser;

public class JsonLowercaseSerializer : DefaultContractResolver
{
    protected override string ResolvePropertyName(string propertyName)
    {
        return propertyName.ToLower();
    }
}
