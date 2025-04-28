using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public class CartPriceService : ICartPriceService
{
    private readonly ICartRepository _cartRepository;

    public CartPriceService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<decimal> GetPriceTotalAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetByIdAsync(cartId);
        return cart == null ? throw new KeyNotFoundException($"Cart with ID {cartId} not found.") : cart.PriceTotal;
    }
}
