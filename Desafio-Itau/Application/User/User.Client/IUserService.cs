using DesafioInvestimentosItau.Application.User.User.Client.DTOs;

namespace DesafioInvestimentosItau.Application.User.User.Client;

public interface IUserService
{
    Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
}