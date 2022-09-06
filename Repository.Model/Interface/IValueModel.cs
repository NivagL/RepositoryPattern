namespace Model.Abstraction;

/// <summary>
/// Use this if the class has no 'key' and only a value
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IValueModel<TValue>
    : IValue<TValue>
    , IValueAssign<TValue>
    , IValueDiffer<TValue> 
{
}
