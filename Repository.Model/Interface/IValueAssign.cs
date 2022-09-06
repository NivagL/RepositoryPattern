namespace Model.Abstraction;

/// <summary>
/// Given an object assign it's values from another
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IValueAssign<TValue>
{
    Action<TValue, TValue> Assign { get; }
}
