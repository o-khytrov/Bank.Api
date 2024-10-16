namespace Bank.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateWorkerHost(args);
        host.Services.MigrateDb();

        host.Run();
    }

    public static IHost CreateWorkerHost(string[] strings)
    {
        var builder = Host.CreateApplicationBuilder(strings);
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddWorker(builder.Configuration);


        var host1 = builder.Build();
        return host1;
    }
}