namespace Repository.Model;

/// <summary>
/// Given a model class, these lamdas define the operations available for the models key
/// They are lamdas so that they are not opinionated about how the functions are implemented
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IKey<TKey, TValue>
{
    /// <summary>
    /// Generate a new key if possible
    /// </summary>
    Func<TKey> NewKey { get; }
    /// <summary>
    /// Test for the key being a tuple
    /// </summary>
    bool IsKeyTuple { get; }
    /// <summary>
    /// Get the key from an object
    /// </summary>
    Func<TValue, TKey> GetKey { get; }
    /// <summary>
    /// Set the key on the object
    /// </summary>
    Action<TValue, TKey> SetKey { get; }
    /// <summary>
    /// Test the keys match, used fror tuple comparisions
    /// </summary>
    Func<TKey, TKey, bool> KeysEqual { get; }
    /// <summary>
    /// The name to be used for the key type
    /// </summary>
    string KeyTypeName { get; }
}
