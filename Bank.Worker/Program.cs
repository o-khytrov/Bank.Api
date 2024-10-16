using Bank.Worker;
using FluentMigrator.Runner;


var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BankDb")
                       ?? throw new ArgumentException("Missing connection string");

builder.Services.AddDbMigrations(connectionString);
builder.Services.AddDb(connectionString);
builder.Services.AddMessaging();

var host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

host.Run();