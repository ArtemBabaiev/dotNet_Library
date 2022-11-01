using Gateway.API.Services;
using Gateway.API.Services.Interfaces;
using Ocelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.Middleware;

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
            builder.Services.AddOcelot().AddCacheManager(x =>
            {
                x.WithDictionaryHandle();
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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}