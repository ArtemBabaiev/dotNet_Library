using Catalog.DAL;
using Catalog.DAL.Repository;
using Catalog.DAL.Repository.Interface;
using Catalog.DAL.UOW.Interface;
using Catalog.DAL.UOW;
using Microsoft.EntityFrameworkCore;
using Catalog.BLL.Service;
using Catalog.BLL.Service.Interface;
using MassTransit;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<dotNet_CatalogContext>(options =>
            {
                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            //gRPC
            builder.Services.AddGrpc();

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
                endpoints.MapGrpcService<Services.LiteratureGrpcService>();
                endpoints.MapGrpcService<Services.ExemplarGrpcService>();
            });
            app.MapControllers();

            app.Run();
        }
    }
}