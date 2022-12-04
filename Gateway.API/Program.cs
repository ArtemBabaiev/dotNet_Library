using Gateway.API.Services;
using Gateway.API.Services.Interfaces;
using Ocelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.Middleware;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Values;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Gateway.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("ocelot.json");
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            

            var authenticationProviderKey = "IdentityApiKey";
            builder.Services.AddAuthentication()
                 .AddJwtBearer(authenticationProviderKey, options =>
                 {
                     options.Authority = "https://localhost:7167";
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateAudience = false
                     };
                     options.RequireHttpsMetadata = false;
                 });

            builder.Services.AddOcelot().AddCacheManager(x =>
            {
                x.WithDictionaryHandle();
            });

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

            /*builder.Services.AddHttpClient<IWrittenOffService, WrittenOffService>(c =>
                    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:WrittenOffUrl"]));

            builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
                    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]));*/


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseOcelot().Wait();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.Run();
        }
    }
}