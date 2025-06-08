using System.Net;

namespace DesafioInvestimentosItau.Application.Exceptions;

public class AveragePriceException : ApiException
{
    public AveragePriceException(string assetCode, long userId)
        : base($"User {userId} does not have the average price for the asset {assetCode}", (int)HttpStatusCode.NotFound) { }
}