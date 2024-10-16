using Bank.Data;
using Bank.Data.Repositories;
using Bank.Worker.Consumers;
using FluentMigrator.Runner;
using MassTransit;

namespace Bank.Worker;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbMigrations(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DbProvider).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
        return services;
    }

    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<SubmitOrderConsumer>();
            x.AddConsumer<SearchOrderConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("user");
                    h.Password("password");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    public static IServiceCollection AddDb(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IOrderRepository>(new OrderRepository(connectionString));
        return services;
    }
}