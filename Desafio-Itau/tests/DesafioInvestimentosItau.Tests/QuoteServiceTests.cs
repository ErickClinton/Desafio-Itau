using Xunit;
using Moq;
using FluentAssertions;

using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;
using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Client;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;


public class QuoteServiceTests
{
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock;
    private readonly Mock<IQuoteInternalService> _quoteInternalServiceMock;
    private readonly Mock<IKafkaProducer> _kafkaProducerMock;
    private readonly Mock<ILogger<QuoteService>> _loggerMock;
    private readonly Mock<IAssetService> _assetServiceMock;
    private readonly QuoteService _quoteService;

    public QuoteServiceTests()
    {
        _quoteRepositoryMock = new Mock<IQuoteRepository>();
        _quoteInternalServiceMock = new Mock<IQuoteInternalService>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _loggerMock = new Mock<ILogger<QuoteService>>();
        _assetServiceMock = new Mock<IAssetService>();

        _quoteService = new QuoteService(
            _quoteRepositoryMock.Object,
            _quoteInternalServiceMock.Object,
            _kafkaProducerMock.Object,
            _loggerMock.Object,
            _assetServiceMock.Object
        );
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
    {
        var assetCode = "PETR4";
        var timestamp = DateTime.UtcNow;

        _quoteRepositoryMock.Setup(q => q.ExistsAsync(assetCode, timestamp)).ReturnsAsync(true);

        var result = await _quoteService.ExistsAsync(assetCode, timestamp);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedQuote()
    {
        var dto = new CreateQuoteDto
        {
            AssetCode = "ITUB4",
            UnitPrice = 34.56m,
            Timestamp = DateTime.UtcNow
        };

        _quoteRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<QuoteEntity>()))
            .ReturnsAsync((QuoteEntity q) =>
            {
                q.Id = 123;
                return q;
            });

        var result = await _quoteService.CreateAsync(dto);

        result.Should().NotBeNull();
        result.AssetCode.Should().Be(dto.AssetCode);
        result.UnitPrice.Should().Be(dto.UnitPrice);
        result.Timestamp.Should().Be(dto.Timestamp);
        result.Id.Should().Be(123);
    }

    [Fact]
    public async Task SearchQuote_ShouldReturnQuote_WhenApiSucceeds()
    {
        var assetCode = "BBDC4";
        var mockApiResponse = new B3QuotationResponseDto
        {
            ticker = assetCode,
            price = 22.5m,
            tradeTime = DateTime.UtcNow
        };

        _quoteInternalServiceMock.Setup(q => q.GetQuotationByAssetCodeAsync(assetCode))
            .ReturnsAsync(mockApiResponse);

        var result = await _quoteService.SearchQuote(assetCode);

        result.Should().NotBeNull();
        result.AssetCode.Should().Be(assetCode);
        result.UnitPrice.Should().Be(22.5m);
    }

    [Fact]
    public async Task SearchQuote_ShouldReturnFallback_WhenApiFailsAndDbHasData()
    {
        var assetCode = "B3SA3";
        var fallbackQuote = new QuoteEntity
        {
            AssetCode = assetCode,
            UnitPrice = 99.99m,
            Timestamp = DateTime.UtcNow
        };

        _quoteInternalServiceMock.Setup(q => q.GetQuotationByAssetCodeAsync(assetCode))
            .ThrowsAsync(new Exception("API down"));

        _quoteRepositoryMock.Setup(q => q.GetLatestByAssetCodeAsync(assetCode))
            .ReturnsAsync(fallbackQuote);

        var result = await _quoteService.SearchQuote(assetCode);

        result.Should().NotBeNull();
        result.AssetCode.Should().Be(assetCode);
        result.UnitPrice.Should().Be(99.99m);
    }

    [Fact]
    public async Task SearchQuote_ShouldThrowFallbackException_WhenApiAndDbFail()
    {
        var assetCode = "FAIL3";

        _quoteInternalServiceMock.Setup(q => q.GetQuotationByAssetCodeAsync(assetCode))
            .ThrowsAsync(new Exception("API error"));

        _quoteRepositoryMock.Setup(q => q.GetLatestByAssetCodeAsync(assetCode))
            .ReturnsAsync((QuoteEntity?)null);

        Func<Task> act = async () => await _quoteService.SearchQuote(assetCode);

        await act.Should().ThrowAsync<Exception>().WithMessage("*Unable to fetch quote*");
    }
}
