using DesafioInvestimentosItau.Application.Asset.Asset.Contract.DTOs;
using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;

public class AssetService : IAssetService
{
    private readonly IAssetRepository _assetRepository;
    private readonly ILogger _logger;
    public AssetService(IAssetRepository assetRepository, ILogger<AssetService> logger)
    {
        _assetRepository = assetRepository;
        _logger = logger;
    }

    public async Task<AssetEntity> CreateAsync(CreateAssetRequest request)
    {
        _logger.LogInformation($"Start service CreateAsync - Request - {request}");
        var asset = new AssetEntity
        {
            Code = request.Code,
            Name = request.Name
        };
        
        var response = await _assetRepository.CreateAsync(asset);
        _logger.LogInformation($"End service CreateAsync - Response - {response}");
        return response;
    }
    
    public async Task<bool> ExistsByCodeAsync(string code)
    {
        _logger.LogInformation($"Start service ExistsByCodeAsync - Request - {code}");
        var response = await _assetRepository.ExistsByCodeAsync(code);
        _logger.LogInformation($"End service ExistsByCodeAsync - Response - {code}");
        return response;
    }
}