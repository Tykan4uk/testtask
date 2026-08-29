using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common;
using Application.Config;
using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Resources;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly IOptions<AuthConfig> _options;

        public UserService(IUnitOfWork db, IMapper mapper, IOptions<AuthConfig> options)
        {
            _db = db;
            _mapper = mapper;
            _options = options;
        }

        public async Task<Result<UserDto>> CreateAsync(UserModel user)
        {
            var existingUser = await _db.UserInfos.GetByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return Result<UserDto>.Failure(new Error(409, ErrorReasons.EmailAlreadyExist, ErrorType.Conflict));
            }

            user.Password = HashPassword(user.Password);

            var entity = _mapper.Map<UserModel, UserInfo>(user);

            await _db.UserInfos.CreateAsync(entity);
            await _db.SaveChangesAsync();

            var dto = _mapper.Map<UserInfo, UserDto>(entity);

            return Result<UserDto>.Success(dto);
        }

        public async Task<Result<LoginDto>> LoginAsync(LoginModel user)
        {
            user.Password = HashPassword(user.Password);

            var entity = _mapper.Map<LoginModel, UserInfo>(user);

            var existingUser = await _db.UserInfos.GetAsync(entity);

            if (existingUser == null)
            {
                return Result<LoginDto>.Failure(new Error(404, ErrorReasons.UserNotExistOrIncorrectPassword, ErrorType.NotFound));
            }

            var loginDto = new LoginDto { Token = GenerateJWT(existingUser) };

            return Result<LoginDto>.Success(loginDto);
        }

        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }

        private string GenerateJWT(UserInfo user)
        {
            var authParams = _options.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            claims.Add(new Claim("role", "client"));

            var token = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
