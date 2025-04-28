using Ambev.DeveloperEvaluation.Common.Messaging;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Common;

public class RabbitMqProducerTests
{
    private readonly Mock<IEventPublisher> _eventPublisherMock;

    public RabbitMqProducerTests()
    {
        // Cria o mock genérico para IEventPublisher
        _eventPublisherMock = new Mock<IEventPublisher>();

        // Configura o mock para aceitar qualquer tipo de evento e retornar uma tarefa concluída
        _eventPublisherMock
            .Setup(p => p.PublishEventAsync(It.IsAny<object>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
    }

    [Fact]
    public async Task PublishEventAsync_ShouldCallPublishMethod_WhenEventIsProvided()
    {
        // Arrange
        var testEvent = new { Name = "TestEvent", Value = 42 };

        // Act
        await _eventPublisherMock.Object.PublishEventAsync(testEvent);

        // Assert
        _eventPublisherMock.Verify(p => p.PublishEventAsync(It.IsAny<object>()), Times.Once);
    }
}
