using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Repository.Expressions
{
    public class RepositoryExpressionsFactory<TValue> : IRepositoryExpressionsFactory<TValue>
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<IRepositoryExpressionsFactory<TValue>> Logger;
        public Func<IServiceProvider, IQueryExpressionBuilder<TValue>> QueryExpressionBuilder { get; set; }
        public Func<IServiceProvider, IOrderExpressionBuilder<TValue>> OrderExpressionBuilder { get; set; }

        public RepositoryExpressionsFactory(IConfiguration configuration, ILogger<IRepositoryExpressionsFactory<TValue>> logger)
        {
            Configuration = configuration;
            Logger = logger;
            Logger.LogInformation(Configuration.GetValue<string>("Service"));
            QueryExpressionBuilder = QueryExpressionBuilderImpl;
            OrderExpressionBuilder = OrderExpressionBuilderImpl;
        }

        public void RegisterTypes(IServiceCollection services)
        {
            if (QueryExpressionBuilder != null)
                services.AddScoped(QueryExpressionBuilder);

            if (OrderExpressionBuilder != null)
                services.AddScoped(OrderExpressionBuilder);
        }

        public virtual IQueryExpressionBuilder<TValue> QueryExpressionBuilderImpl(IServiceProvider serviceProvider)
        {
            try
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<IQueryExpressionBuilder<TValue>>();

                var instance = new QueryExpressionBuilder<TValue>(configuration, logger);
                return instance;
            }
            catch (Exception ex)
            {
                if (Logger.IsEnabled(LogLevel.Error))
                {
                    Logger.LogError("Tools Factory Creating Query Expression Builder {0} {1} {2}",
                        typeof(TValue).Name, ex.Message, ex.StackTrace);
                }
                throw;
            }
        }

        public virtual IOrderExpressionBuilder<TValue> OrderExpressionBuilderImpl(IServiceProvider serviceProvider)
        {
            try
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<IOrderExpressionBuilder<TValue>>();

                var instance = new OrderExpressionBuilder<TValue>(configuration, logger);
                return instance;
            }
            catch (Exception ex)
            {
                if (Logger.IsEnabled(LogLevel.Error))
                {
                    Logger.LogError("Tools Factory Creating Order Expression Builder {0} {1} {2}",
                    typeof(TValue).Name, ex.Message, ex.StackTrace);
                }
                throw;
            }
        }
    }
}
