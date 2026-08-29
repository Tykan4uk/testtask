using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserInfoRepository
    {
        Task<UserInfo> CreateAsync(UserInfo user);
        Task<UserInfo> GetAsync(UserInfo user);
        Task<UserInfo> GetByEmailAsync(string email);
    }
}
