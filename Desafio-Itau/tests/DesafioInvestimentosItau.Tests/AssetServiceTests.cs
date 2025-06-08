using DesafioInvestimentosItau.Application.Asset.Asset.Contract.DTOs;
using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Tests;

public class AssetServiceTests
{
    private readonly Mock<IAssetRepository> _assetRepositoryMock;
    private readonly Mock<ILogger<AssetService>> _loggerMock;
    private readonly AssetService _assetService;

    public AssetServiceTests()
    {
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _loggerMock = new Mock<ILogger<AssetService>>();
        _assetService = new AssetService(_assetRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAsset()
    {
        var request = new CreateAssetRequest()
        {
            Code = "PETR4",
            Name = "Petrobras"
        };

        _assetRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<AssetEntity>()))
            .ReturnsAsync((AssetEntity a) => {
                a.Id = 1;
                return a;
            });

        var result = await _assetService.CreateAsync(request);

        result.Should().NotBeNull();
        result.Code.Should().Be("PETR4");
        result.Name.Should().Be("Petrobras");
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task ExistsByCodeAsync_ShouldReturnTrue_WhenAssetExists()
    {
        var code = "ITUB4";
        _assetRepositoryMock.Setup(r => r.ExistsByCodeAsync(code)).ReturnsAsync(true);

        var result = await _assetService.ExistsByCodeAsync(code);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByCodeAsync_ShouldReturnFalse_WhenAssetDoesNotExist()
    {
        var code = "VALE3";
        _assetRepositoryMock.Setup(r => r.ExistsByCodeAsync(code)).ReturnsAsync(false);

        var result = await _assetService.ExistsByCodeAsync(code);

        result.Should().BeFalse();
    }
}