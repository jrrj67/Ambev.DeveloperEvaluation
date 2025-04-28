using Ambev.DeveloperEvaluation.Application.Sales.GetSale.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale.Profiles;

/// <summary>
/// Profile for mapping between Sale entity and GetSaleResponse
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale operation
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Sale, GetSaleResponse>();
    }
}

