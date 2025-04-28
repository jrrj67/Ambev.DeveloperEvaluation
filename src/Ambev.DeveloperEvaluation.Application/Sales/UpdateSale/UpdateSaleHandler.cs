using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale.Commands;
using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// 
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, Guid>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICartPriceService _cartPriceService;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<CreateSaleHandler> _logger;

    public UpdateSaleHandler(ISaleRepository saleRepository,
                             ICartPriceService cartPriceService,
                             IEventPublisher eventPublisher,
                             ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _cartPriceService = cartPriceService;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<Guid> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (sale is null)
        {
            _logger.LogWarning("Sale with ID {SaleId} not found or invalid type", request.Id);
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found or invalid type");
        }

        sale.UpdateCustomer(request.Customer);
        sale.UpdateBranch(request.Branch);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Disparo manual do evento após o ID ser confirmado no repositório
        var saleCreatedEvent = new SaleModifiedEvent(sale.Id, request.Customer, request.Branch, request.IsCancelled);

        try
        {
            await _eventPublisher.PublishEventAsync(saleCreatedEvent);
            _logger.LogInformation("Evento SaleModifiedEvent publicado com sucesso para a venda ID: {SaleId}", sale.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar o evento SaleModifiedEvent para a venda ID: {SaleId}", sale.Id);
        }

        return sale.Id;
    }
}
