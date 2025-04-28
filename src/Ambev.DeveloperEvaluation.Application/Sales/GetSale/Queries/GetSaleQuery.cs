using Ambev.DeveloperEvaluation.Application.Sales.GetSale.Responses;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale.Queries;

public class GetSaleQuery : IRequest<GetSaleResponse>
{
    public Guid Id { get; }

    public GetSaleQuery(Guid id)
    {
        Id = id;
    }
}
