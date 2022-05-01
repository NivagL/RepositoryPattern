using Repository.Serialiser;

namespace Repository.Model;

/// <summary>
/// Operations that apply to a model
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IValue<TValue> 
    : IValueAssign<TValue>
    , IValueDiffer<TValue>
{
    /// <summary>
    /// Create a new value type
    /// </summary>
    Func<TValue> CreateValue { get; }
    /// <summary>
    /// Tests if the values are equal
    /// </summary>
    Func<TValue, TValue, bool> ValuesEqual { get; }
    /// <summary>
    /// The name to be used for the value type
    /// </summary>
    string ValueTypeName { get; }
    /// <summary>
    /// Defines how to serialiser a value
    /// </summary>
    ISerialiser<TValue> ValueSerialiser { get; }
}
