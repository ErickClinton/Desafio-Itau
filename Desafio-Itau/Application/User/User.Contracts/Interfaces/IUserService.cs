using DesafioInvestimentosItau.Application.User.User.Client.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;

public interface IUserService
{
    Task<UserEntity> CreateAsync(CreateUserRequestDto dto);
    Task<UserEntity?> GetByIdAsync(long id);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<bool> ExistsAsync(string email);
}