using System.Reflection;
using FluentValidation;
using MassTransit;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s => s.ExampleFilters());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddMassTransit(x =>
{
    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(host: rabbitMqConfig["Host"], port: Convert.ToUInt16(rabbitMqConfig["Port"]), virtualHost: rabbitMqConfig["VirtualHost"], h =>
        {
            h.Username(rabbitMqConfig["Username"] ?? string.Empty);
            h.Password(rabbitMqConfig["Password"] ?? string.Empty);
        });
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}