using Application.Dtos;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Resources;
using Infrastructure.Interfaces;
using Moq;
using Xunit;
using AuditoriumService = Application.Services.AuditoriumService;

namespace UnitTests.Services;

public class AuditoriumServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAuditoriumRepository> _auditoriumRepositoryMock;

    private readonly AuditoriumService _service;

    public AuditoriumServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _auditoriumRepositoryMock = new Mock<IAuditoriumRepository>();

        _unitOfWorkMock
            .Setup(x => x.Auditoriums)
            .Returns(_auditoriumRepositoryMock.Object);

        _service = new AuditoriumService(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    #region Create

    [Fact]
    public async Task CreateAuditoriumAsync_ShouldReturnFailure_WhenAuditoriumAlreadyExists()
    {
        // Arrange
        var model = new AuditoriumModel
        {
            Name = "Auditorium 1"
        };

        var existing = new Auditorium
        {
            Id = Guid.NewGuid(),
            Name = model.Name
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByNameAsync(model.Name))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.CreateAuditoriumAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Error.Code);
        Assert.Equal(
            ErrorReasons.AuditoriumExist,
            result.Error.Message);

        _mapperMock.Verify(
            x => x.Map<AuditoriumModel, Auditorium>(
                It.IsAny<AuditoriumModel>()),
            Times.Never);

        _auditoriumRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Auditorium>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task CreateAuditoriumAsync_ShouldCreateAuditorium_WhenNameIsUnique()
    {
        // Arrange
        var model = new AuditoriumModel
        {
            Name = "Auditorium 1",
            Capacity = 100,
            BaseRentalPrice = 1000
        };

        var entity = new Auditorium
        {
            Id = Guid.NewGuid(),
            Name = model.Name
        };

        var dto = new AuditoriumDto
        {
            Id = entity.Id,
            Name = entity.Name
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByNameAsync(model.Name))
            .ReturnsAsync((Auditorium?)null);

        _mapperMock
            .Setup(x => x.Map<AuditoriumModel, Auditorium>(model))
            .Returns(entity);

        _mapperMock
            .Setup(x => x.Map<Auditorium, AuditoriumDto>(entity))
            .Returns(dto);

        // Act
        var result = await _service.CreateAuditoriumAsync(model);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(dto, result.Value);

        _auditoriumRepositoryMock.Verify(
            x => x.AddAsync(entity),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    #endregion

    #region Update

    [Fact]
    public async Task UpdateAuditoriumAsync_ShouldReturnNotFound_WhenIdIsNull()
    {
        // Arrange
        var model = new AuditoriumModel
        {
            Id = null,
            Name = "Auditorium"
        };

        // Act
        var result = await _service.UpdateAuditoriumAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error.Code);

        _auditoriumRepositoryMock.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAuditoriumAsync_ShouldReturnNotFound_WhenAuditoriumDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var model = new AuditoriumModel
        {
            Id = id,
            Name = "Auditorium"
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Auditorium?)null);

        // Act
        var result = await _service.UpdateAuditoriumAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error.Code);

        _auditoriumRepositoryMock.Verify(
            x => x.GetByNameAsync(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAuditoriumAsync_ShouldReturnFailure_WhenNameBelongsToAnotherAuditorium()
    {
        // Arrange
        var id = Guid.NewGuid();
        var anotherId = Guid.NewGuid();

        var model = new AuditoriumModel
        {
            Id = id,
            Name = "New name"
        };

        var existing = new Auditorium
        {
            Id = id,
            Name = "Old name"
        };

        var auditoriumWithSameName = new Auditorium
        {
            Id = anotherId,
            Name = model.Name
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(existing);

        _auditoriumRepositoryMock
            .Setup(x => x.GetByNameAsync(model.Name))
            .ReturnsAsync(auditoriumWithSameName);

        // Act
        var result = await _service.UpdateAuditoriumAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Error.Code);

        _mapperMock.Verify(
            x => x.Map(
                It.IsAny<AuditoriumModel>(),
                It.IsAny<Auditorium>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAuditoriumAsync_ShouldUpdate_WhenNameIsUnique()
    {
        // Arrange
        var id = Guid.NewGuid();

        var model = new AuditoriumModel
        {
            Id = id,
            Name = "Updated auditorium",
            Capacity = 200,
            BaseRentalPrice = 2000
        };

        var existing = new Auditorium
        {
            Id = id,
            Name = "Old auditorium"
        };

        var dto = new AuditoriumDto
        {
            Id = id,
            Name = model.Name
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(existing);

        _auditoriumRepositoryMock
            .Setup(x => x.GetByNameAsync(model.Name))
            .ReturnsAsync((Auditorium?)null);

        _mapperMock
            .Setup(x => x.Map(model, existing))
            .Returns(existing);

        _mapperMock
            .Setup(x => x.Map<Auditorium, AuditoriumDto>(existing))
            .Returns(dto);

        // Act
        var result = await _service.UpdateAuditoriumAsync(model);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(dto, result.Value);

        _mapperMock.Verify(
            x => x.Map(model, existing),
            Times.Once);

        _auditoriumRepositoryMock.Verify(
            x => x.UpdateAsync(existing),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    #endregion

    #region Remove

    [Fact]
    public async Task RemoveAuditoriumAsync_ShouldReturnNotFound_WhenAuditoriumDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Auditorium?)null);

        // Act
        var result = await _service.RemoveAuditoriumAsync(id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error.Code);

        _auditoriumRepositoryMock.Verify(
            x => x.DeleteAsync(It.IsAny<Guid>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task RemoveAuditoriumAsync_ShouldDelete_WhenAuditoriumExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        var existing = new Auditorium
        {
            Id = id,
            Name = "Auditorium"
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.RemoveAuditoriumAsync(id);

        // Assert
        Assert.True(result.IsSuccess);

        _auditoriumRepositoryMock.Verify(
            x => x.DeleteAsync(id),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    #endregion

    #region SearchFree

    [Fact]
    public async Task SearchFreeAsync_ShouldReturnBadRequest_WhenStartTimeIsGreaterThanEndTime()
    {
        // Arrange
        var model = new AuditoriumSearchFreeModel
        {
            Date = new DateOnly(2026, 8, 28),
            StartTime = new TimeOnly(15, 0),
            EndTime = new TimeOnly(14, 0),
            Capacity = 10
        };

        // Act
        var result = await _service.SearchFreeAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Error.Code);
        Assert.Equal(
            ErrorReasons.StartTimeBiggerThanEnd,
            result.Error.Message);

        _auditoriumRepositoryMock.Verify(
            x => x.GetFreeAuditoriumsAsync(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task SearchFreeAsync_ShouldReturnAuditoriums_WhenRequestIsValid()
    {
        // Arrange
        var model = new AuditoriumSearchFreeModel
        {
            Date = new DateOnly(2026, 8, 28),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 0),
            Capacity = 10
        };

        var auditoriums = new List<Auditorium>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Auditorium 1",
                Capacity = 20
            }
        };

        var dto = new List<AuditoriumDto>
        {
            new()
            {
                Id = auditoriums[0].Id,
                Name = "Auditorium 1"
            }
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetFreeAuditoriumsAsync(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                model.Capacity))
            .ReturnsAsync(auditoriums);

        _mapperMock
            .Setup(x => x.Map<List<AuditoriumDto>>(auditoriums))
            .Returns(dto);

        // Act
        var result = await _service.SearchFreeAsync(model);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(dto, result.Value);

        _auditoriumRepositoryMock.Verify(
            x => x.GetFreeAuditoriumsAsync(
                It.Is<DateTime>(x =>
                    x == new DateTime(
                        2026,
                        8,
                        28,
                        10,
                        0,
                        0,
                        DateTimeKind.Utc)),
                It.Is<DateTime>(x =>
                    x == new DateTime(
                        2026,
                        8,
                        28,
                        12,
                        0,
                        0,
                        DateTimeKind.Utc)),
                model.Capacity),
            Times.Once);
    }

    #endregion
}