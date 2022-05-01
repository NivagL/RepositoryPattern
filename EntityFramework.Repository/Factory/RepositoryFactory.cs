using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using System.Collections.Generic;

namespace EntityFramework.Repository;

public class RepositoryFactory<TContext>
    where TContext : DbContext
{
    private readonly IContextFactory Context;
    public List<IFactory> KeyModels { get; } = new List<IFactory>();
    public List<IFactory> KeyRepositories { get; } = new List<IFactory>();
    public List<IFactory> ValueModels { get; } = new List<IFactory>();
    public List<IFactory> ValueRepositories { get; } = new List<IFactory>();

    public RepositoryFactory(IContextFactory context)
    {
        Context = context;
    }

    public void RegisterTypes(IServiceCollection services)
    {
        Context.RegisterTypes(services);

        foreach (var keyModel in KeyModels)
            keyModel.RegisterTypes(services);

        foreach (var keyRepository in KeyRepositories)
            keyRepository.RegisterTypes(services);

        foreach (var valueModel in ValueModels)
            valueModel.RegisterTypes(services);

        foreach (var valueRepository in ValueRepositories)
            valueRepository.RegisterTypes(services);
    }
}
