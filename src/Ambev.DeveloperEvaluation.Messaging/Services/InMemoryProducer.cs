using Ambev.DeveloperEvaluation.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Messaging.Services;

public class InMemoryProducer : IEventPublisher
{
    private readonly ILogger<InMemoryProducer> _logger;

    public InMemoryProducer(ILogger<InMemoryProducer> logger)
    {
        _logger = logger;
    }

    public Task PublishEventAsync<T>(T domainEvent) where T : class
    {
        _logger.LogInformation("Simulando publicação de evento: {EventName}", typeof(T).Name);
        return Task.CompletedTask;
    }
}
