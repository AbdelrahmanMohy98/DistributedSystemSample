using RabbitMQ.Client;
using Shared.Configurations;
using System.Text;
using System.Text.Json;

namespace ServiceA.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ILogger<RabbitMQService> _logger;
        private readonly RabbitMQConfiguration _configuration;
        private readonly ConnectionFactory _factory;

        public RabbitMQService(ILogger<RabbitMQService> logger, RabbitMQConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _factory = new ConnectionFactory
            {
                HostName = _configuration.Server,
                UserName = _configuration.UserName,
                Password = _configuration.Password
            };
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _configuration.QueueName, 
                durable: true, 
                exclusive: false, 
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await channel.BasicPublishAsync<BasicProperties>(exchange: "",
                routingKey: _configuration.QueueName,
                mandatory: false,
                basicProperties: new BasicProperties(),
                body: body,
                cancellationToken: cancellationToken);

        }
    }
}
