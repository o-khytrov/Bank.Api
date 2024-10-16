using Bank.Data;
using Bank.Data.Repositories;
using Bank.Worker.Consumers;
using FluentMigrator.Runner;
using MassTransit;

namespace Bank.Worker;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection AddDbMigrations(this IServiceCollection services, string connectionString)
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

    private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitMQ");
        services.AddMassTransit(x =>
        {
            x.AddConsumer<SubmitOrderConsumer>();
            x.AddConsumer<SearchOrderConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host: rabbitMqConfig["Host"], port: Convert.ToUInt16(rabbitMqConfig["Port"]) , virtualHost: rabbitMqConfig["VirtualHost"], h =>
                {
                    h.Username(rabbitMqConfig["Username"] ?? string.Empty);
                    h.Password(rabbitMqConfig["Password"] ?? string.Empty);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    private static IServiceCollection AddDb(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IOrderRepository>(new OrderRepository(connectionString));
        return services;
    }


    public static IServiceCollection AddWorker(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("BankDb")
                               ?? throw new ArgumentException("Missing connection string");

        services.AddDbMigrations(connectionString);
        services.AddDb(connectionString);
        services.AddMessaging(configuration);
        return services;
    }

    public static void MigrateDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}