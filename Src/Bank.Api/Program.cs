using System.Reflection;
using System.Text.Json.Serialization;
using Bank.Api;
using FluentValidation;
using MassTransit;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Ignore null values
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.ExampleFilters();
    var xmlCommentsFile = "Bank.Api.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    s.IncludeXmlComments(xmlCommentsFullPath);
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMassTransit(x =>
{
    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");
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
app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();

app.Run();

/// <summary>
/// Added for accessibility in tests
/// </summary>
public partial class Program
{
}