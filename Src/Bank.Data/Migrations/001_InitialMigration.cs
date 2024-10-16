using FluentMigrator;
using Microsoft.Extensions.Configuration;

namespace Bank.Data.Migrations;

[Migration(2024101501)]
public class InitialMigration : Migration
{
    private DbProvider _provider = DbProvider.PostgreSql;
    public InitialMigration(IConfiguration configuration)
    {
        if (Enum.TryParse<DbProvider>(configuration["DbProvider"], out var provider))
            _provider = provider;
    }
    public override void Up()
    {
        var script = Helper.LoadSqlStatement($"Migrations.{_provider}.{nameof(InitialMigration)}.Up.sql");
        Execute.Sql(script);
    }

    public override void Down()
    {
        var script = Helper.LoadSqlStatement($"Migrations.{_provider}.{nameof(InitialMigration)}.Down.sql");
        Execute.Sql(script);
    }
}