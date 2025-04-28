using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale.Commands;

public class CancelSaleCommand : IRequest<Guid>
{
    public Guid Id { get; set; }

    public CancelSaleCommand(Guid id)
    {
        Id = id;
    }
}
