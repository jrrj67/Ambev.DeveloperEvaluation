namespace Ambev.DeveloperEvaluation.Domain.Services.Interfaces;

public interface IProductPriceService
{
    Task<decimal> GetPriceAsync(Guid productId);
}
