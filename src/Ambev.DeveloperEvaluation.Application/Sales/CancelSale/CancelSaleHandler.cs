using Ambev.DeveloperEvaluation.Application.Sales.CancelSale.Commands;
using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// 
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, Guid>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICartPriceService _cartPriceService;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<CancelSaleHandler> _logger;

    public CancelSaleHandler(ISaleRepository saleRepository,
                             ICartPriceService cartPriceService,
                             IEventPublisher eventPublisher,
                             ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _cartPriceService = cartPriceService;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<Guid> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

        sale.Cancel();

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Disparo manual do evento após o ID ser confirmado no repositório
        var saleCancelledEvent = new SaleCancelledEvent(sale.Id, true);

        try
        {
            await _eventPublisher.PublishEventAsync(saleCancelledEvent);
            _logger.LogInformation("Evento SaleCreatedEvent publicado com sucesso para a venda ID: {SaleId}", sale.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar o evento SaleCreatedEvent para a venda ID: {SaleId}", sale.Id);
        }

        return sale.Id;
    }
}
