using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
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

            // Add services to the container.
            builder.Services.AddDbContext<dotNet_WrittenOffManagmentContext>(options =>
            {
                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

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


            app.MapControllers();

            app.Run();
        }
    }
}