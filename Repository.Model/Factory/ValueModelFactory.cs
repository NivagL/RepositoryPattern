using Microsoft.Extensions.DependencyInjection;

namespace Repository.Model;

public class ValueModelFactory<TValue>
    : IValueModelFactory<TValue>
{
    private readonly object ValueMeta;
    public Func<IServiceProvider, IValueModel<TValue>> ValueModel { get; set; }
    public Func<IServiceProvider, IValue<TValue>> Value { get; set; }
    public Func<IServiceProvider, IValueAssign<TValue>> ModelAssign { get; set; }
    public Func<IServiceProvider, IValueDiffer<TValue>> ModelDiffer { get; set; }

    public ValueModelFactory(object valueMeta)
    {
        ValueMeta = valueMeta;
        ValueModel = ValueModelImpl;
        Value = ValueImpl;
        ModelAssign = ModelAssignImpl;
        ModelDiffer = ModelDifferImpl;
    }

    private IValueModel<TValue> ValueModelImpl(IServiceProvider provider)
    {
        return ValueMeta as IValueModel<TValue>;
    }

    private IValue<TValue> ValueImpl(IServiceProvider provider)
    {
        return ValueMeta as IValue<TValue>;
    }

    private IValueAssign<TValue> ModelAssignImpl(IServiceProvider provider)
    {
        return ValueMeta as IValueAssign<TValue>;
    }

    private IValueDiffer<TValue> ModelDifferImpl(IServiceProvider provider)
    {
        return ValueMeta as IValueDiffer<TValue>;
    }

    public void RegisterTypes(IServiceCollection services)
    {
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
