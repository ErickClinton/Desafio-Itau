using System.Net;

namespace DesafioInvestimentosItau.Application.Exceptions;

public class InvalidTradeDataException : ApiException
{
    public InvalidTradeDataException(string message)
        : base(message, (int)HttpStatusCode.BadRequest) { }
}