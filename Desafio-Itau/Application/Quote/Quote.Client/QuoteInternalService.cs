using System.Net;
using System.Net.Http.Json;
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
    public async Task<B3QuotationResponseDto> GetQuotationByAssetCodeAsync(string assetName)
    {
        var response = await _httpClient.GetAsync($"api/Assets/{assetName}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new Exception($"O ativo '{assetName}' não foi encontrado na B3.");

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<B3QuotationResponseDto>();

        if (data == null)
            throw new Exception("Resposta da API inválida.");
        return data;
    }
}