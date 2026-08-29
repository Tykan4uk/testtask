using Application.Config;
using Application.Dtos;
using Application.Models;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Resources;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IUserInfoRepository> _userRepositoryMock = new();

    private readonly UserService _service;

    public UserServiceTests()
    {
        _unitOfWorkMock
            .Setup(x => x.UserInfos)
            .Returns(_userRepositoryMock.Object);

        var authConfig = Options.Create(new AuthConfig
        {
            Secret = "my-super-secret-key-at-least-32-chars",
            Issuer = "TestTask",
            Audience = "TestTaskClient",
            TokenLifeTime = 3600
        });

        _service = new UserService(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            authConfig);
    }

    #region Create

    [Fact]
    public async Task CreateAsync_ShouldReturnConflict_WhenEmailAlreadyExists()
    {
        // Arrange
        var model = new UserModel
        {
            Email = "test@test.com",
            Password = "password"
        };

        var existingUser = new UserInfo
        {
            Id = Guid.NewGuid(),
            Email = model.Email
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(model.Email))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _service.CreateAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(409, result.Error.Code);
        Assert.Equal(
            ErrorReasons.EmailAlreadyExist,
            result.Error.Message);

        _userRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<UserInfo>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenEmailIsUnique()
    {
        // Arrange
        var model = new UserModel
        {
            Email = "test@test.com",
            Password = "password"
        };

        var entity = new UserInfo
        {
            Id = Guid.NewGuid(),
            Email = model.Email
        };

        var dto = new UserDto
        {
            Id = entity.Id,
            Email = entity.Email
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(model.Email))
            .ReturnsAsync((UserInfo?)null);

        _mapperMock
            .Setup(x => x.Map<UserModel, UserInfo>(model))
            .Returns(entity);

        _mapperMock
            .Setup(x => x.Map<UserInfo, UserDto>(entity))
            .Returns(dto);

        // Act
        var result = await _service.CreateAsync(model);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(dto, result.Value);

        _userRepositoryMock.Verify(
            x => x.CreateAsync(entity),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        Assert.NotEqual("password", model.Password);
        Assert.Equal(64, model.Password.Length);
    }

    #endregion

    #region Login

    [Fact]
    public async Task LoginAsync_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var model = new LoginModel
        {
            Email = "test@test.com",
            Password = "password"
        };

        var entity = new UserInfo
        {
            Email = model.Email,
            Password = "some-hash"
        };

        _mapperMock
            .Setup(x => x.Map<LoginModel, UserInfo>(It.IsAny<LoginModel>()))
            .Returns(entity);

        _userRepositoryMock
            .Setup(x => x.GetAsync(entity))
            .ReturnsAsync((UserInfo?)null);

        // Act
        var result = await _service.LoginAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error.Code);
        Assert.Equal(
            ErrorReasons.UserNotExistOrIncorrectPassword,
            result.Error.Message);

        _userRepositoryMock.Verify(
            x => x.GetAsync(entity),
            Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnJwtToken_WhenCredentialsAreCorrect()
    {
        // Arrange
        var model = new LoginModel
        {
            Email = "test@test.com",
            Password = "password"
        };

        var entity = new UserInfo
        {
            Id = Guid.NewGuid(),
            Email = model.Email,
            Password = "hash"
        };

        _mapperMock
            .Setup(x => x.Map<LoginModel, UserInfo>(It.IsAny<LoginModel>()))
            .Returns(entity);

        _userRepositoryMock
            .Setup(x => x.GetAsync(entity))
            .ReturnsAsync(entity);

        // Act
        var result = await _service.LoginAsync(model);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.False(string.IsNullOrWhiteSpace(result.Value.Token));

        var tokenParts = result.Value.Token.Split('.');

        Assert.Equal(3, tokenParts.Length);
    }

    #endregion
}