using Microsoft.Extensions.DependencyInjection;

namespace Repository.Model;

public class KeyedModelFactory<TKey, TValue>
    : ValueModelFactory<TValue>
    , IKeyedModelFactory<TKey, TValue>
{
    private readonly object KeyedMeta;
    public Func<IServiceProvider, IKeyModel<TKey, TValue>> KeyedModel { get; set; }

    public KeyedModelFactory(object keyedMeta)
        : base(keyedMeta)
    {
        KeyedMeta = keyedMeta;
        KeyedModel = KeyedModelImpl;
    }

    private IKeyModel<TKey, TValue> KeyedModelImpl(IServiceProvider provider)
    {
        return KeyedMeta as IKeyModel<TKey, TValue>;
    }

    public new void RegisterTypes(IServiceCollection services)
    {
        if (KeyedModel != null)
            services.AddScoped(KeyedModel);

        base.RegisterTypes(services);
    }
}
