using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;

namespace DesafioInvestimentosItau.Api.Controller;

using Microsoft.AspNetCore.Mvc;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;

[ApiController]
[Route("api/[controller]")]
public class QuotationController : ControllerBase
{
    private readonly IQuoteService _quoteService;

    public QuotationController(IQuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    [HttpGet("latest/{assetCode}")]
    public async Task<IActionResult> GetLatestQuotation(string assetCode)
    {
        var quote = await _quoteService.SearchQuote(assetCode);
       

        return Ok(quote);
    }
}