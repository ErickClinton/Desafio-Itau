using System.Net;

namespace DesafioInvestimentosItau.Application.Exceptions;

public class BusinessRuleException : ApiException
{
    public BusinessRuleException(string message)
        : base(message, (int)HttpStatusCode.BadRequest) { }
}