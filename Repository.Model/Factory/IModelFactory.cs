namespace Repository.Model;

public interface IModelFactory<TKey, TValue>
{
    Func<IServiceProvider, IKeyModel<TKey, TValue>> KeyModel { get; set; }
    Func<IServiceProvider, IValueModel<TValue>> ValueModel { get; set; }
    Func<IServiceProvider, IValue<TValue>> Value { get; set; }
    Func<IServiceProvider, IValueAssign<TValue>> ModelAssign { get; set; }
    Func<IServiceProvider, IValueDiffer<TValue>> ModelDiffer { get; set; }
}
