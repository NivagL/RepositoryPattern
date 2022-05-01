using Repository.Serialiser;
using Repository.Utils;

namespace Repository.Model;

public class DefaultKeyModel<TKey, TValue> 
    : DefaultValueModel<TValue>
    , IKeyModel<TKey, TValue>
{
    public Func<TKey> NewKey { get; set; }
    public bool IsKeyTuple { get; set; }
    public string KeyTypeName { get; set; }
    public Func<TValue, TKey> GetKey { get; set; }
    public Action<TValue, TKey> SetKey { get; set; }
    public Func<TKey, TKey, bool> KeysEqual { get; set; }
    public ISerialiser<TKey> KeySerialiser { get; set; }

    public DefaultKeyModel()
    {
        KeyTypeName = typeof(TKey).Name;
        NewKey = CreateImpl<TKey>;
        IsKeyTuple = TupleUtils.IsTuple(typeof(TKey));
        KeysEqual = KeysEqualImpl;
        KeySerialiser = new DefaultSerialiser<TKey>();
    }

    public bool KeysEqualImpl(TKey x, TKey y)
    {
        if (x == null || y == null)
            return false;

        if(IsKeyTuple)
        {
            var xItems = TupleUtils.TupleToEnumerable(x);
            var yItems = TupleUtils.TupleToEnumerable(y);
            return xItems.SequenceEqual(yItems);
        }
        else
        {
            return DefaultEqual(x, y);
        }
    }
}
