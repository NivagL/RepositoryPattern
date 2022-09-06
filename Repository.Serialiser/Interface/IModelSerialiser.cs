namespace Model.Serialiser;

public interface IModelSerialiser<TKey, TValue>
{
    ISerialiser<TKey> KeySerialiser { get; set; }
    ISerialiser<TValue> ValueSerialiser { get; set; }
}
