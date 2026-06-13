using Microsoft.AspNetCore.Http.HttpResults;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Configurations;

namespace ServiceB.Services
{
    public class RabbitMqConsumer : BackgroundService
    {

        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly RabbitMQConfiguration _configuration;
        private readonly ConnectionFactory _factory;
        private readonly ITextFileService _textFileService;

        public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger, RabbitMQConfiguration configuration, ITextFileService textFileService)
        {
            _logger = logger;
            _configuration = configuration;
            _textFileService = textFileService;
            _factory = new ConnectionFactory
            {
                HostName = _configuration.Server,
                UserName = _configuration.UserName,
                Password = _configuration.Password
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _configuration.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, arg) =>
            {
                try
                {
                    var body = arg.Body.ToArray();
                    var message = System.Text.Encoding.UTF8.GetString(body);
                    _logger.LogInformation("Received message: {Message}", message);

                    var content = await _textFileService.ReadAsync();
                    if (string.IsNullOrEmpty(content))
                    {
                        content = "0";
                    }
                    string newContent = (double.Parse(content) + (double.Parse(message))).ToString();
                    await _textFileService.WriteAsync(newContent);

                    await channel.BasicAckAsync(deliveryTag: arg.DeliveryTag, multiple: false);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing message");
                    await channel.BasicNackAsync(deliveryTag: arg.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await channel.BasicConsumeAsync(queue: _configuration.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
