using DesafioInvestimentosItau.Application.User.User.Client;
using DesafioInvestimentosItau.Application.User.User.Client.DTOs;
using DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.User.User.Client;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserRequestDto dto)
    {
        _logger.LogInformation("Creating user with email: {Email}", dto.Email);
        var alreadyExists = await _userRepository.ExistsAsync(dto.Email);
        if(alreadyExists == true)
            throw new Exception($"Email {dto.Email} already exists");
        
        var user = new UserEntity
        {
            Name = dto.Name,
            Email = dto.Email,
            BrokerageFee = dto.BrokerageFee
        };

        user = await _userRepository.CreateAsync(user);

        _logger.LogInformation("User created with ID: {UserId}", user.Id);

        return new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            BrokerageFee = user.BrokerageFee
        };
    }

    public async Task<UserEntity?> GetByIdAsync(long id)
    {
        _logger.LogInformation("Getting user by ID: {UserId}", id);
        
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all users");

        var users = await _userRepository.GetAllAsync();

        return users.Select(user => new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            BrokerageFee = user.BrokerageFee
        }).ToList();
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _userRepository.ExistsAsync(email);
    }
}
