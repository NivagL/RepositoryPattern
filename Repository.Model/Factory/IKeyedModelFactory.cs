namespace Repository.Model;

public interface IKeyedModelFactory<TKey, TValue> 
    : IValueModelFactory<TValue>
{
    Func<IServiceProvider, IKeyedModel<TKey, TValue>> KeyedModel { get; set; }
}
