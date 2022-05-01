using Repository.Serialiser;

namespace Repository.Model;

public class DefaultValueModel<TValue> : IValueModel<TValue>
{
    public string ValueTypeName { get; set; }
    public Func<TValue> CreateValue { get; set; }
    public Func<TValue, TValue, bool> ValuesEqual { get; set; }
    public Action<TValue, TValue> Assign { get; set; }
    public Func<TValue, TValue, bool> Differ { get; set; }
    public ISerialiser<TValue> ValueSerialiser { get; set; }

    public DefaultValueModel()
    {
        ValueTypeName = typeof(TValue).Name;
        CreateValue = CreateImpl<TValue>;
        ValuesEqual = DefaultEqual<TValue>;
        ValueSerialiser = new DefaultSerialiser<TValue>();
    }

    public T CreateImpl<T>()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        return (T)Activator.CreateInstance(typeof(T));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

    public bool DefaultEqual<T>(T left, T right)
    {
        if (left is IEquatable<T>)
        {
            return Equals(left, right);
        }

        if (left is IEqualityComparer<T>)
        {
            var equals = left as IEqualityComparer<T>;
            if (equals == null) return false;
            return equals.Equals(left, right);
        }

        //test for operator ==?

        throw new InvalidCastException("Model does not support equality");
    }
}
