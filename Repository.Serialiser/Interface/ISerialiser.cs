namespace Model.Serialiser;

public interface ISerialiser<T> : IDeserialiser<T>
{
    Func<T, string> CreateString { get; set; }
    Func<IEnumerable<T>, string> CreateStrings { get; set; }
}
