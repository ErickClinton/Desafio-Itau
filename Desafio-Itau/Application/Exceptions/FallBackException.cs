using System.Net;

namespace DesafioInvestimentosItau.Application.Exceptions;

public class FallBackException : ApiException
{
    public FallBackException(string message)
        : base(message, (int)HttpStatusCode.BadRequest) { }
}