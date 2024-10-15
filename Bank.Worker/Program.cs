using Bank.Data;
using Bank.Data.Repositories;
using Bank.Worker;
using Bank.Worker.Consumers;
using FluentMigrator.Runner;
using MassTransit;

var connectionString = "Server=localhost;Database=bankdb;User Id=postgres;Password=postgres";
var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(DbProvider).Assembly).For.Migrations())
    // Enable logging to console in the FluentMigrator way
    .AddLogging(lb => lb.AddFluentMigratorConsole())
    .BuildServiceProvider(false);
builder.Services.AddSingleton<IOrderRepository>(new OrderRepository(connectionString));
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SubmitOrderConsumer>();

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
// Build the service provider
var host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

host.Run();