using System.Net;

namespace DesafioInvestimentosItau.Application.Exceptions;

public class NoTradesFoundException : ApiException
{
    public NoTradesFoundException(string assetCode)
        : base($"No buy trades found for asset '{assetCode}'.", (int)HttpStatusCode.NotFound) { }
}