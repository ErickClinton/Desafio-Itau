using DesafioInvestimentosItau.Application.User.User.Client;
using DesafioInvestimentosItau.Application.User.User.Client.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.User.User.Contracts;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request)
    {
        var user = new UserEntity
        {
            Name = request.Name,
            Email = request.Email,
            BrokerageFee = request.BrokerageFee
        };

        var created = await _userRepository.CreateAsync(user);

        return new UserResponseDto()
        {
            Id = created.Id,
            Name = created.Name,
            Email = created.Email,
            BrokerageFee = created.BrokerageFee
        };
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(user => new UserResponseDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            BrokerageFee = user.BrokerageFee
        });
    }
}