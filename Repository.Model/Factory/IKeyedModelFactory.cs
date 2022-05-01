namespace Repository.Model;

public interface IKeyedModelFactory<TKey, TValue> 
    : IValueModelFactory<TValue>
{
    Func<IServiceProvider, IKeyModel<TKey, TValue>> KeyedModel { get; set; }
}
