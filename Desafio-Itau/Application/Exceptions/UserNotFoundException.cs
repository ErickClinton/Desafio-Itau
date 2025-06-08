using System.Net;

namespace DesafioInvestimentosItau.Application.Exceptions;


public class UserNotFoundException : ApiException
{
    public UserNotFoundException(long userId)
        : base($"User with ID {userId} was not found.", (int)HttpStatusCode.NotFound) { }
}