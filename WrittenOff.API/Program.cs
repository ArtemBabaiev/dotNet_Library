using EventBus.Messages.Common;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using WrittenOffManagement.API.EventBusConsumer;
using WrittenOffManagement.Application.CQRS.Command;
using WrittenOffManagement.Application.CQRS.CommandHadler;
using WrittenOffManagement.Application.CQRS.Query;
using WrittenOffManagement.Application.CQRS.QueryHandler;
using WrittenOffManagement.Domain.Entities;
using WrittenOffManagement.Domain.Interface;
using WrittenOffManagement.Infrastructure.Data.Repository;
using WrittenOffManagement.Infrastructure.Persistence;

namespace WrittenOffManagement.API
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
            builder.Services.AddDbContext<dotNet_WrittenOffManagmentContext>(options =>
            {
                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            //gRPC
            builder.Services.AddGrpc();


            // MassTransit-RabbitMQ Configuration
            builder.Services.AddMassTransit(config => {
                config.AddConsumer<WriteOffExemplarConsumer>();
                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

                    cfg.ReceiveEndpoint(EventBusConstants.WriteOffExemplarQueue, c => {
                        c.ConfigureConsumer<WriteOffExemplarConsumer>(ctx);
                    });
                });
            });
            //builder.Services.AddMassTransitHostedService();

            #region repositories
            builder.Services.AddScoped<IWrittenOffRepository, WrittenOffRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            #endregion

            #region handlers
            #region writtenOffs
            builder.Services.AddTransient<IRequestHandler<CreateWrittenOffCommand, Unit>, CreateWrittenOffCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<UpdateWrittenOffCommand, Unit>, UpdateWrittenOffCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<DeleteWrittenOffCommand, Unit>, DeleteWrittenOffCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<GetAllWrittenOffsQuery, IEnumerable<WrittenOff>>, GetAllWrittenOffsQueryHandler>();
            builder.Services.AddTransient<IRequestHandler<GetWrittenOffByIdQuery, WrittenOff>, GetWrittenOffByIdQueryHandler>();
            #endregion

            #region employyes
            builder.Services.AddTransient<IRequestHandler<CreateEmployeeCommand, Unit>, CreateEmployeeCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<UpdateEmployeeCommand, Unit>, UpdateEmployeeCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<DeleteEmployeeCommand, Unit>, DeleteEmployeeCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<GetAllEmployeesQuery, IEnumerable<Employee>>, GetAllEmployeesQueryHandler>();
            builder.Services.AddTransient<IRequestHandler<GetEmployeeByIdQuery, Employee>, GetEmployeeByIdQueryHandler>();
            #endregion

            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Services.WritenOffGrpcService>();
            });

            app.MapControllers();

            app.Run();
        }
    }
}