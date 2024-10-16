using Bank.Worker;
using FluentMigrator.Runner;


var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var connectionString = builder.Configuration.GetConnectionString("BankDb")
                       ?? throw new ArgumentException("Missing connection string");

builder.Services.AddDbMigrations(connectionString);
builder.Services.AddDb(connectionString);
builder.Services.AddMessaging(builder.Configuration);

var host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

host.Run();