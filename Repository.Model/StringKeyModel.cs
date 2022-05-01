using Repository.Serialiser;

namespace Repository.Model;

public class StringKeyModel<TValue> : DefaultKeyModel<string, TValue>
{
    public StringKeyModel()
    {
        KeysEqual = (x, y) => x == y;
        IsKeyTuple = false;
        NewKey = () => Guid.NewGuid().ToString();
        KeySerialiser = new StringSerialiser();
    }
}
