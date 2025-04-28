namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale.Responses;

public class GetSaleResponse
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }
    public decimal PriceTotal { get; set; }

    // External ID do Cart
    public Guid CartId { get; set; }

    // Campos desnormalizados do Cart
    public Guid UserId { get; set; }

    public GetSaleResponse(string saleNumber,
                           DateTime saleDate,
                           string customer,
                           string branch,
                           bool isCancelled,
                           decimal priceTotal,
                           Guid cartId,
                           Guid userId)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        Customer = customer;
        Branch = branch;
        IsCancelled = isCancelled;
        PriceTotal = priceTotal;
        CartId = cartId;
        UserId = userId;
    }
}
