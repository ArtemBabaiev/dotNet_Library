using Catalog.DAL;
using Catalog.DAL.Repository;
using Catalog.DAL.Repository.Interface;
using Catalog.DAL.UOW.Interface;
using Catalog.DAL.UOW;
using Microsoft.EntityFrameworkCore;
using Catalog.BLL.Service;
using Catalog.BLL.Service.Interface;
using MassTransit;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddEnvironmentVariables();

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

            // Add services to the container.
            builder.Services.AddDbContext<dotNet_CatalogContext>(options =>
            {
                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            //gRPC
            builder.Services.AddGrpc();

            builder.Services.AddMemoryCache();
            //Redis
            builder.Services.AddStackExchangeRedisCache(options => {
                options.Configuration = builder.Configuration["ConnectionStrings:Redis"];
                options.InstanceName = "dotNet_Catalog";
            });

            // MassTransit-RabbitMQ Configuration
            builder.Services.AddMassTransit(config => {
                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                });
            });


            #region repositories
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IExemplarRepository, ExemplarRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<ILiteratureRepository, LiteratureRepository>();
            builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
            builder.Services.AddScoped<ITypeRepository, TypeRepository>();
            builder.Services.AddScoped<IWritingRepository, WritingRepository>();
            #endregion

            #region unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            #region services
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<IExemplarService, ExemplarService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<ILiteratureService, LiteratureService>();
            builder.Services.AddScoped<IPublisherService, PublisherService>();
            builder.Services.AddScoped<ITypeService, TypeService>();
            builder.Services.AddScoped<IWritingService, WritingService>();
            #endregion
            builder.Services.AddAuthentication("Bearer")
                 .AddJwtBearer("Bearer", options =>
                 {
                     options.Audience = "catalogAPI";
                     options.Authority = builder.Configuration["Identity:Authority"];
                     options.MetadataAddress = builder.Configuration["Identity:MetadataAddress"];
                     options.RequireHttpsMetadata = false;
                 });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            /*foreach (var c in builder.Configuration.AsEnumerable())
            {
                Console.WriteLine(c.Key + " = " + c.Value);
            }*/

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Services.LiteratureGrpcService>();
                endpoints.MapGrpcService<Services.ExemplarGrpcService>();
            });
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<dotNet_CatalogContext>();
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }catch(Exception e) { }
                
            }

            app.Run();
        }
    }
}