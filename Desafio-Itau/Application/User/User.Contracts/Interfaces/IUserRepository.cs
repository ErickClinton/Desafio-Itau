using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;

public interface IUserRepository
{
    Task<UserEntity> CreateAsync(UserEntity user);
    Task<UserEntity?> GetByIdAsync(long id);
    Task<IEnumerable<UserEntity>> GetAllAsync();
    Task<bool> ExistsAsync(string email);
}