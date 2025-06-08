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

    public async Task<UserEntity> CreateAsync(CreateUserRequestDto dto)
    {
        _logger.LogInformation($"Start Service CreateAsync - Request - {dto}");
        var alreadyExists = await _userRepository.ExistsAsync(dto.Email);
        if(alreadyExists == true)
            throw new Exception($"Email {dto.Email} already exists");
        
        var user = new UserEntity
        {
            Name = dto.Name,
            Email = dto.Email,
            BrokerageFee = 5
        };

        user = await _userRepository.CreateAsync(user);

        _logger.LogInformation($"End Service CreateAsync - Request - {user}");

        return user;
    }

    public async Task<UserEntity?> GetByIdAsync(long id)
    {
        _logger.LogInformation($"Start Service GetByIdAsync - Request - {id}");
        var response = await _userRepository.GetByIdAsync(id);
        _logger.LogInformation($"End Service GetByIdAsync - Response - {response}");
        return response;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        _logger.LogInformation($"Start Service GetAllAsync");

        var users = await _userRepository.GetAllAsync();
        var response = users.Select(user => new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            BrokerageFee = user.BrokerageFee
        }).ToList();
        _logger.LogInformation($"End Service GetAllAsync - Response - {response}");
        return response;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        _logger.LogInformation($"Start Service ExistsAsync - Request - {email}");
        var response = await _userRepository.ExistsAsync(email);
        _logger.LogInformation($"End Service ExistsAsync - Response - {response}");
        return response;
    }
}
