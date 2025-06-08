using System.Net;

namespace DesafioInvestimentosItau.Application.Exceptions;

public class AssetNotFoundB3Exception : ApiException
{
    public AssetNotFoundB3Exception(string assetCode)
        : base($"Asset '{assetCode}' not found on B3.", (int)HttpStatusCode.NotFound) { }
}