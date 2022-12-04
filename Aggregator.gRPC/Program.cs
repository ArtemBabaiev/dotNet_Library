using Aggregator.gRPC.Services;
using Catalog.API.Protos;
using WrittenOffManagement.API.Protos;

namespace Aggregator.gRPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //GRPC config
            builder.Services.AddGrpc(
                opt => opt.EnableDetailedErrors = true
                );
            builder.Services.AddGrpcClient<Literature.LiteratureClient>(o =>
            {
                o.Address = new Uri(builder.Configuration["gRPCSettings:CatalogUrl"]);
            });
           builder.Services.AddGrpcClient<Exemplar.ExemplarClient>(o =>
            {
                o.Address = new Uri(builder.Configuration["gRPCSettings:CatalogUrl"]);
            });
            builder.Services.AddGrpcClient<WrittenOff.WrittenOffClient>(o =>
            {
                o.Address = new Uri(builder.Configuration["gRPCSettings:WrittenOffUrl"]);
            });

            builder.Services.AddScoped<LiteratureAggregatorService>();
            builder.Services.AddScoped<ExemplarAggregatorService>();

            var app = builder.Build();
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