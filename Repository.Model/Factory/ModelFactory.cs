using Microsoft.Extensions.DependencyInjection;

namespace Repository.Model;

public class ModelFactory<TKey, TValue>
    : IModelFactory<TKey, TValue>
{
    private readonly object Model;
    public Func<IServiceProvider, IKeyModel<TKey, TValue>> KeyModel { get; set; }
    public Func<IServiceProvider, IValueModel<TValue>> ValueModel { get; set; }
    public Func<IServiceProvider, IValue<TValue>> Value { get; set; }
    public Func<IServiceProvider, IValueAssign<TValue>> ModelAssign { get; set; }
    public Func<IServiceProvider, IValueDiffer<TValue>> ModelDiffer { get; set; }

    public ModelFactory(object model)
    {
        Model = model;
        KeyModel = KeyModelImpl;
        ValueModel = ValueModelImpl;
        Value = ValueImpl;
        ModelAssign = ModelAssignImpl;
        ModelDiffer = ModelDifferImpl;
    }

    private IKeyModel<TKey, TValue> KeyModelImpl(IServiceProvider provider)
    {
        return Model as IKeyModel<TKey, TValue>;
    }

    private IValueModel<TValue> ValueModelImpl(IServiceProvider provider)
    {
        return Model as IValueModel<TValue>;
    }

    private IValue<TValue> ValueImpl(IServiceProvider provider)
    {
        return Model as IValue<TValue>;
    }

    private IValueAssign<TValue> ModelAssignImpl(IServiceProvider provider)
    {
        return Model as IValueAssign<TValue>;
    }

    private IValueDiffer<TValue> ModelDifferImpl(IServiceProvider provider)
    {
        return Model as IValueDiffer<TValue>;
    }

    public void RegisterTypes(IServiceCollection services)
    {
        if (KeyModel != null)
            services.AddScoped(KeyModel);

        if (ValueModel != null)
            services.AddScoped(ValueModel);

        if (Value != null)
            services.AddScoped(Value);

        if (ModelAssign != null)
            services.AddScoped(ModelAssign);

        if (ModelDiffer != null)
            services.AddScoped(ModelDiffer);
    }
}
