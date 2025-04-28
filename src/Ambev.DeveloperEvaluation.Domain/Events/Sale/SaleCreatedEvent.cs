using Ambev.DeveloperEvaluation.Common.Messaging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale;

public class SaleCreatedEvent : IEvent
{
    public Guid SaleId { get; }
    public string SaleNumber { get; private set; } = string.Empty;
    public DateTime SaleDate { get; private set; }
    public string Customer { get; private set; } = string.Empty;
    public string Branch { get; private set; } = string.Empty;
    public bool IsCancelled { get; private set; }

    public DateTime OccurredOn { get; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

    public SaleCreatedEvent(Guid saleId, string saleNumber, DateTime saleDate, string customer, string branch, bool isCancelled)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        Customer = customer;
        Branch = branch;
        IsCancelled = isCancelled;
    }
}