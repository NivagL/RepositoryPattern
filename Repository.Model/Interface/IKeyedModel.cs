namespace Repository.Model;

/// <summary>
/// Register this interface for a model class
/// It allows tools to access the model for standard operations
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IKeyedModel<TKey, TValue> 
    : IKey<TKey, TValue>
    , IValue<TValue>
    , IValueAssign<TValue>
    , IValueDiffer<TValue>
{
}
