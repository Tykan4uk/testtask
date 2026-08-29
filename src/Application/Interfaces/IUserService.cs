using Application.Common;
using Application.Dtos;
using Application.Models;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDto>> CreateAsync(UserModel user);

        Task<Result<LoginDto>> LoginAsync(LoginModel user);
    }
}
