using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;

namespace DesafioInvestimentosItau.Api.Controller;

using Microsoft.AspNetCore.Mvc;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;

[ApiController]
[Route("api/quote")]
public class QuotationController : ControllerBase
{
    private readonly IQuoteService _quoteService;
    private readonly ILogger<QuotationController> _logger;

    public QuotationController(IQuoteService quoteService, ILogger<QuotationController> logger)
    {
        _quoteService = quoteService;
        _logger = logger;
    }

    [HttpGet("latest/{assetCode}")]
    public async Task<IActionResult> GetLatestQuotation(string assetCode)
    {
        _logger.LogInformation($"Start method GetLatestQuotation - Request - {assetCode}");
        var quote = await _quoteService.SearchQuote(assetCode);
        return Ok(quote);
    }
}