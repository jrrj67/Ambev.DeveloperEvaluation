using Ambev.DeveloperEvaluation.Application.Sales.CancelSale.Commands;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale.Commands;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale.Queries;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale.Commands;
using Ambev.DeveloperEvaluation.Common.Cache;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[Route("api/sales")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;

    private const string SALE_CACHE_KEY = "Product:{id}";

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        var id = await _mediator.Send(command);
        return Created(nameof(GetSaleAsync), new { id }, id);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleCommand command)
    {
        command.Id = id;
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [InvalidateCache(SALE_CACHE_KEY)]
    public async Task<IActionResult> CancelSale([FromRoute] Guid id)
    {
        var command = new CancelSaleCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = nameof(GetSaleAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Cache(SALE_CACHE_KEY, DurationInMinutes = 15)]
    public async Task<IActionResult> GetSaleAsync([FromRoute] Guid id)
    {
        var query = new GetSaleQuery(id);

        var sale = await _mediator.Send(query);

        return Ok(sale);
    }
}
