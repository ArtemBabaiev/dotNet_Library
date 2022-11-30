using RecordManagment.DAL.Repository;
using RecordManagment.DAL.Repository.Interface;
using RecordManagment.DAL.UnitOfWork.Interface;
using RecordManagment.DAL.UnitOfWork;
using System.Data;
using System.Data.SqlClient;
using RecordManagment.BLL.Service.Interface;
using RecordManagment.BLL.Service;
using RecordManagment.BLL.MapperConfig;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace RecordManagment.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region connection
            builder.Services.AddScoped((s) => new SqlConnection(
                "Data Source=DESKTOP-FJFPA91;Initial Catalog=dotNet_RecordManagment;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
                ));
            builder.Services.AddScoped<IDbTransaction>(s =>
            {
                SqlConnection conn = s.GetRequiredService<SqlConnection>();
                conn.Open();
                return conn.BeginTransaction();
            });
            #endregion

            #region repositories
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IExemplarRepository, ExemplarRepository>();
            builder.Services.AddScoped<ILiteratureRepository, LiteratureRepository>();
            builder.Services.AddScoped<IReaderRepository, ReaderRepository>();
            builder.Services.AddScoped<IRecordRepository, RecordRepository>();
            #endregion

            #region unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<MapConfig, MapConfig>();
            #endregion

            #region services
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IExemplarService, ExemplarService>();
            builder.Services.AddScoped<ILiteratureService, LiteratureService>();
            builder.Services.AddScoped<IReaderService, ReaderService>();
            builder.Services.AddScoped<IRecordService, RecordService>();
            #endregion

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
    }
}