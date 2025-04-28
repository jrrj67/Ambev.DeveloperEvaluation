using Ambev.DeveloperEvaluation.Common.Messaging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale;

public class SaleCancelledEvent : IEvent
{
    public Guid SaleId { get; }
    public bool IsCancelled { get; private set; }

    public DateTime OccurredOn { get; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

    public SaleCancelledEvent(Guid saleId, bool isCancelled)
    {
        SaleId = saleId;
        IsCancelled = isCancelled;
    }
}

