using System.Net;
using System.Net.Http.Json;
using DesafioInvestimentosItau.Application.Exceptions;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.Quote.Quote.Client;

public class QuoteInternalService:IQuoteInternalService
{
    
    private readonly HttpClient _httpClient;
    private readonly ILogger<QuoteInternalService> _logger;

    public QuoteInternalService(HttpClient httpClient, ILogger<QuoteInternalService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    public async Task<B3QuotationResponseDto> GetQuotationByAssetCodeAsync(string assetCode)
    {
        _logger.LogInformation($"Start Internal GetQuotationByAssetCodeAsync - Request - {assetCode}");
        var response = await _httpClient.GetAsync($"api/Assets/{assetCode}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new AssetNotFoundB3Exception(assetCode);

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<B3QuotationResponseDto>();

        if (data == null)
            throw new Exception("Response Api is null");
        _logger.LogInformation($"End Internal GetQuotationByAssetCodeAsync - Response - {data}");
        return data;
    }
}