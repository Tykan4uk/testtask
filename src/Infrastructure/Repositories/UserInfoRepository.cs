using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly TestTaskDbContext _context;

        public UserInfoRepository(TestTaskDbContext context)
        {
            _context = context;
        }

        public async Task<UserInfo> CreateAsync(UserInfo user)
        {
            await _context.UserInfos.AddAsync(user);

            return user;
        }

        public async Task<UserInfo> GetAsync(UserInfo user)
        {
            var result = await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

            return result;
        }

        public async Task<UserInfo> GetByEmailAsync(string email)
        {
            var result = await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == email);

            return result;
        }
    }
}
