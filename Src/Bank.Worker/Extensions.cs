using Bank.Data;
using Bank.Data.Repositories;
using Bank.Worker.Consumers;
using FluentMigrator.Runner;
using MassTransit;
using static System.Enum;

namespace Bank.Worker;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection AddDbMigrations(this IServiceCollection services, string connectionString, DbProvider dbProvider)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                switch (dbProvider)
                {
                    case DbProvider.MsSql:
                        rb.AddSqlServer();
                        break;
                    case DbProvider.PostgreSql:
                        rb.AddPostgres();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dbProvider), dbProvider, null);
                }

                rb.WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(DbProvider).Assembly).For.Migrations();
            })
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
                cfg.Host(rabbitMqConfig["Host"], Convert.ToUInt16(rabbitMqConfig["Port"]), rabbitMqConfig["VirtualHost"], h =>
                {
                    h.Username(rabbitMqConfig["Username"] ?? string.Empty);
                    h.Password(rabbitMqConfig["Password"] ?? string.Empty);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    private static IServiceCollection AddDb(this IServiceCollection services, string connectionString, DbProvider dbProvider)
    {
        services.AddSingleton<IOrderRepository>(dbProvider == DbProvider.PostgreSql ? new NpgsqlOrderRepository(connectionString) : new MssqlOrderRepository(connectionString));
        return services;
    }


    public static IServiceCollection AddWorker(this IServiceCollection services, IConfiguration configuration)
    {
        var dbProvider = GetDbProvider(configuration);

        var connectionString = configuration.GetConnectionString(dbProvider == DbProvider.MsSql ? "BankDbMssql" : "BankDbPostgresql")
                               ?? throw new ArgumentException("Missing connection string");

        services.AddDbMigrations(connectionString, dbProvider);
        services.AddDb(connectionString, dbProvider);
        services.AddMessaging(configuration);
        return services;
    }

    public static DbProvider GetDbProvider(IConfiguration configuration)
    {
        TryParse<DbProvider>(configuration["DbProvider"], out var dbProvider);
        return dbProvider;
    }

    public static void MigrateDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}