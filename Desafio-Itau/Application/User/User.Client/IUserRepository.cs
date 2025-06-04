using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.User.User.Client;

public interface IUserRepository
{
    Task<UserEntity> CreateAsync(UserEntity user);
    Task<IEnumerable<UserEntity>> GetAllAsync();
}