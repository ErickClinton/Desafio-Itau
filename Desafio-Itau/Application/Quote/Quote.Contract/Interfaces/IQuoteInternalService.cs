using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;

public interface IQuoteInternalService
{
    Task<B3QuotationResponseDto> GetQuotationByAssetCodeAsync(string assetCode);

}