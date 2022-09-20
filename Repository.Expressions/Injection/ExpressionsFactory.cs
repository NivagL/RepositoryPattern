using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Repository.Expressions
{
    public class ExpressionsFactory<TValue> : IExpressionsFactory<TValue>
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<IExpressionsFactory<TValue>> Logger;
        public Func<IServiceProvider, IQueryExpression<TValue>> QueryExpressionBuilder { get; set; }
        public Func<IServiceProvider, IOrderExpression<TValue>> OrderExpressionBuilder { get; set; }

        public ExpressionsFactory(IConfiguration configuration, ILogger<IExpressionsFactory<TValue>> logger)
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

        public virtual IQueryExpression<TValue> QueryExpressionBuilderImpl(IServiceProvider serviceProvider)
        {
            try
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<IQueryExpression<TValue>>();

                var instance = new QueryExpression<TValue>(configuration, logger);
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

        public virtual IOrderExpression<TValue> OrderExpressionBuilderImpl(IServiceProvider serviceProvider)
        {
            try
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<IOrderExpression<TValue>>();

                var instance = new OrderExpression<TValue>(configuration, logger);
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
