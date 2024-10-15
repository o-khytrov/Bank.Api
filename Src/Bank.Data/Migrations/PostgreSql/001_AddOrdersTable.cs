using FluentMigrator;

namespace Bank.Data.Migrations.PostgreSql;

[Migration(2024101501)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        var script = Helper.LoadSqlStatement($"Migrations.{nameof(InitialMigration)}.Up.sql");
        Execute.Sql(script);
    }

    public override void Down()
    {
        var script = Helper.LoadSqlStatement($"Migrations.{nameof(InitialMigration)}.Down.sql");
        Execute.Sql(script);
    }
}