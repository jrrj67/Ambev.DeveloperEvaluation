using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale.Commands;

public class UpdateSaleCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }

    // External ID do Cart
    public Guid CartId { get; set; }

    // Campos desnormalizados do Cart
    public Guid UserId { get; set; }
}
