using Agregator.Services;
using Agregator.Services.Interfaces;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHttpClient<IWrittenOffService, WrittenOffService>(c =>
                    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:WrittenOffUrl"]))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        builder.Services.AddHttpClient<IRecordMngmtService, RecordMngmtService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["ApiSettings:RecordUrl"]))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy()); ;

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
            {
                IndexFormat = $"applogs-{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-logs-{DateTime.UtcNow:yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1
            })
            .Enrich.WithProperty("Enviroment", context.HostingEnvironment.EnvironmentName)
            .ReadFrom.Configuration(context.Configuration);
        });

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
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
            retryCount: 5,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (exception, retryCount, context) =>
            {
                Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
            });
    }
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );
    }
}