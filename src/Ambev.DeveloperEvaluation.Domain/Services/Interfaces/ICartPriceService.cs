namespace Ambev.DeveloperEvaluation.Domain.Services.Interfaces;

public interface ICartPriceService
{
    Task<decimal> GetPriceTotalAsync(Guid cartId);
}
