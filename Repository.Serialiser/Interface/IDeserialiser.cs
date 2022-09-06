namespace Model.Serialiser;

public interface IDeserialiser<T>
{
    Func<string, T> CreateObject { get; set; }
    Func<string, IEnumerable<T>> CreateObjects { get; set; }
}
