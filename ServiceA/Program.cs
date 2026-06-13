using ServiceA.Services;
using Shared.Configurations;

namespace ServiceA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();

            var rabbitMqConfig = builder.Configuration.GetSection(nameof(RabbitMQConfiguration)).Get<RabbitMQConfiguration>();
            builder.Services.AddSingleton(rabbitMqConfig);
            builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<SumCalcService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}
