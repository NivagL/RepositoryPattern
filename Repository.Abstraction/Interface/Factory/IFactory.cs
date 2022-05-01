using Microsoft.Extensions.DependencyInjection;

namespace Repository.Abstraction
{
    public interface IFactory
    {
        void RegisterTypes(IServiceCollection services);
    }
}
