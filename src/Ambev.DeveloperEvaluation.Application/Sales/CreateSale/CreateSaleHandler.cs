using Ambev.DeveloperEvaluation.Application.Sales.CreateSale.Commands;
using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// 
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, Guid>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICartPriceService _cartPriceService;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(ISaleRepository saleRepository,
                             ICartPriceService cartPriceService,
                             IEventPublisher eventPublisher,
                             ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _cartPriceService = cartPriceService;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var priceTotal = await _cartPriceService.GetPriceTotalAsync(request.CartId);

        var sale = new Sale(Guid.NewGuid(),
                            request.SaleNumber,
                            request.SaleDate,
                            request.Customer,
                            request.Branch,
                            request.IsCancelled,
                            request.CartId,
                            request.UserId,
                            priceTotal);

        await _saleRepository.CreateAsync(sale, cancellationToken);

        // Disparo manual do evento após o ID ser confirmado no repositório
        var saleCreatedEvent = new SaleCreatedEvent(sale.Id,
                                                    request.SaleNumber,
                                                    request.SaleDate,
                                                    request.Customer,
                                                    request.Branch,
                                                    request.IsCancelled);

        try
        {
            await _eventPublisher.PublishEventAsync(saleCreatedEvent);
            _logger.LogInformation("Evento SaleCreatedEvent publicado com sucesso para a venda ID: {SaleId}", sale.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar o evento SaleCreatedEvent para a venda ID: {SaleId}", sale.Id);
        }

        return sale.Id;
    }
}
