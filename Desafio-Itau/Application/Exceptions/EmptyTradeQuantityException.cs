using System.Net;

namespace DesafioInvestimentosItau.Application.Exceptions;

public class EmptyTradeQuantityException : ApiException
{
    public EmptyTradeQuantityException(string assetCode)
        : base($"Total quantity for asset '{assetCode}' is zero and cannot be used for average price calculation.", (int)HttpStatusCode.BadRequest) { }
}