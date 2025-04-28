using Ambev.DeveloperEvaluation.Application.Sales.GetSale.Queries;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale.Responses;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetUserCommand requests
/// </summary>
public class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;
    /// <summary>
    /// Initializes a new instance of GetUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for GetUserCommand</param>
    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<GetSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetUserCommand request
    /// </summary>
    /// <param name="request">The GetUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<GetSaleResponse> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (sale == null)
        {
            _logger.LogWarning("Sale with ID {SaleId} not found", request.Id);
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");
        }

        return _mapper.Map<GetSaleResponse>(sale);
    }
}
