using Microsoft.Extensions.DependencyInjection;

namespace Repository.Abstraction;

public interface IContextFactory : IFactory
{
    string Application { get; set; }
    string Database { get; set; }
    string Connection { get; set; }
    int Timeout { get; set; }
    bool Pool { get; set; }
}
