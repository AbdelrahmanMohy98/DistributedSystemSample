namespace ServiceA.Services
{
    public interface IRabbitMQService
    {
        public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
    }
}
