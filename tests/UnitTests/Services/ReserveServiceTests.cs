using Application.Dtos;
using Application.Models;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Resources;
using Infrastructure.Interfaces;
using Moq;
using Xunit;
using AuditoriumService = Domain.Entities.AuditoriumService;

namespace UnitTests.Services;

public class ReserveServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private readonly Mock<IAuditoriumRepository> _auditoriumRepositoryMock = new();
    private readonly Mock<IAuditoriumReserveRepository> _reserveRepositoryMock = new();
    private readonly Mock<IAuditoriumServiceRepository> _auditoriumServiceRepositoryMock = new();
    private readonly Mock<ITimeRateRepository> _timeRateRepositoryMock = new();

    private readonly ReserveService _service;

    public ReserveServiceTests()
    {
        _unitOfWorkMock
            .Setup(x => x.Auditoriums)
            .Returns(_auditoriumRepositoryMock.Object);

        _unitOfWorkMock
            .Setup(x => x.AuditoriumReserves)
            .Returns(_reserveRepositoryMock.Object);

        _unitOfWorkMock
            .Setup(x => x.AuditoriumServices)
            .Returns(_auditoriumServiceRepositoryMock.Object);

        _unitOfWorkMock
            .Setup(x => x.TimeRates)
            .Returns(_timeRateRepositoryMock.Object);

        _service = new ReserveService(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task CreateReserveAsync_ShouldReturnBadRequest_WhenDurationIsZero()
    {
        // Arrange
        var model = new AuditoriumReserveModel
        {
            AuditoriumId = Guid.NewGuid(),
            Date = new DateTime(2026, 8, 28, 10, 0, 0),
            Duration = TimeSpan.Zero
        };

        // Act
        var result = await _service.CreateReserveAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Error.Code);
        Assert.Equal(
            ErrorReasons.DurationMustBeNotZero,
            result.Error.Message);

        _auditoriumRepositoryMock.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateReserveAsync_ShouldReturnNotFound_WhenAuditoriumDoesNotExist()
    {
        // Arrange
        var auditoriumId = Guid.NewGuid();

        var model = CreateModel(auditoriumId);

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(auditoriumId))
            .ReturnsAsync((Auditorium?)null);

        // Act
        var result = await _service.CreateReserveAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error.Code);
        Assert.Equal(
            ErrorReasons.AuditoriumNotExist,
            result.Error.Message);

        _reserveRepositoryMock.Verify(
            x => x.IsBusyAsync(
                It.IsAny<Guid>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateReserveAsync_ShouldReturnConflict_WhenAuditoriumIsBusy()
    {
        // Arrange
        var auditoriumId = Guid.NewGuid();

        var model = CreateModel(auditoriumId);

        var auditorium = new Auditorium
        {
            Id = auditoriumId,
            Name = "Auditorium 1",
            BaseRentalPrice = 1000
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(auditoriumId))
            .ReturnsAsync(auditorium);

        _reserveRepositoryMock
            .Setup(x => x.IsBusyAsync(
                auditoriumId,
                model.Date,
                model.Date.Add(model.Duration)))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateReserveAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(409, result.Error.Code);
        Assert.Equal(
            ErrorReasons.AuditoriumAlreadyReserved,
            result.Error.Message);

        _auditoriumServiceRepositoryMock.Verify(
            x => x.GetByIdsAsync(
                It.IsAny<Guid>(),
                It.IsAny<IEnumerable<Guid>>()),
            Times.Never);

        _reserveRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<AuditoriumReserve>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateReserveAsync_ShouldReturnBadRequest_WhenServiceDoesNotBelongToAuditorium()
    {
        // Arrange
        var auditoriumId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var model = CreateModel(
            auditoriumId,
            serviceId);

        var auditorium = new Auditorium
        {
            Id = auditoriumId,
            Name = "Auditorium 1",
            BaseRentalPrice = 1000
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(auditoriumId))
            .ReturnsAsync(auditorium);

        _reserveRepositoryMock
            .Setup(x => x.IsBusyAsync(
                auditoriumId,
                model.Date,
                model.Date.Add(model.Duration)))
            .ReturnsAsync(false);

        _auditoriumServiceRepositoryMock
            .Setup(x => x.GetByIdsAsync(
                auditoriumId,
                It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<AuditoriumService>());

        // Act
        var result = await _service.CreateReserveAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Error.Code);
        Assert.Equal(
            ErrorReasons.ServicesNotAvailable,
            result.Error.Message);

        _timeRateRepositoryMock.Verify(
            x => x.GetListAsync(),
            Times.Never);

        _reserveRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<AuditoriumReserve>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateReserveAsync_ShouldCreateReserve_WhenModelIsValid()
    {
        // Arrange
        var auditoriumId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var model = CreateModel(
            auditoriumId,
            serviceId);

        var auditorium = new Auditorium
        {
            Id = auditoriumId,
            Name = "Auditorium 1",
            BaseRentalPrice = 1000
        };

        var auditoriumService = new AuditoriumService
        {
            Id = serviceId,
            AuditoriumId = auditoriumId,
            ServiceId = Guid.NewGuid(),
            Service = new Service
            {
                Id = Guid.NewGuid(),
                Name = "Projector",
                Price = 500
            }
        };

        var rates = new List<TimeRate>
        {
            new()
            {
                Id = Guid.NewGuid(),
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(18, 0),
                Rate = 1m
            }
        };

        var reserve = new AuditoriumReserve
        {
            Id = Guid.NewGuid(),
            AuditoriumId = auditoriumId,
            DateTime = model.Date,
            EndDateTime = model.Date.Add(model.Duration)
        };

        var dto = new AuditoriumReserveDto
        {
            Id = reserve.Id,
            AuditoriumId = auditoriumId,
            DateTime = model.Date,
            Duration = model.Duration,
            TotalPrice = 2500
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(auditoriumId))
            .ReturnsAsync(auditorium);

        _reserveRepositoryMock
            .Setup(x => x.IsBusyAsync(
                auditoriumId,
                model.Date,
                model.Date.Add(model.Duration)))
            .ReturnsAsync(false);

        _auditoriumServiceRepositoryMock
            .Setup(x => x.GetByIdsAsync(
                auditoriumId,
                It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<AuditoriumService>
            {
                auditoriumService
            });

        _timeRateRepositoryMock
            .Setup(x => x.GetListAsync())
            .ReturnsAsync(rates);

        _mapperMock
            .Setup(x => x.Map<AuditoriumReserveModel, AuditoriumReserve>(model))
            .Returns(reserve);

        _mapperMock
            .Setup(x => x.Map<AuditoriumReserve, AuditoriumReserveDto>(reserve))
            .Returns(dto);

        // Act
        var result = await _service.CreateReserveAsync(model);

        // Assert
        Assert.True(result.IsSuccess);

        // 2 * 1000 * rate 1 = 2000
        // + 500 = 2500
        Assert.Equal(2500, result.Value.TotalPrice);

        _reserveRepositoryMock.Verify(
            x => x.AddAsync(reserve),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task CreateReserveAsync_ShouldRemoveDuplicateServiceIds()
    {
        // Arrange
        var auditoriumId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var model = CreateModel(
            auditoriumId,
            serviceId,
            serviceId);

        var auditorium = new Auditorium
        {
            Id = auditoriumId,
            BaseRentalPrice = 1000
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(auditoriumId))
            .ReturnsAsync(auditorium);

        _reserveRepositoryMock
            .Setup(x => x.IsBusyAsync(
                It.IsAny<Guid>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .ReturnsAsync(false);

        _auditoriumServiceRepositoryMock
            .Setup(x => x.GetByIdsAsync(
                auditoriumId,
                It.Is<IEnumerable<Guid>>(ids =>
                    ids.Count() == 1 &&
                    ids.Single() == serviceId)))
            .ReturnsAsync(new List<AuditoriumService>
            {
                new()
                {
                    Id = serviceId,
                    Service = new Service
                    {
                        Price = 500
                    }
                }
            });

        _timeRateRepositoryMock
            .Setup(x => x.GetListAsync())
            .ReturnsAsync(new List<TimeRate>
            {
                new()
                {
                    StartTime = new TimeOnly(9, 0),
                    EndTime = new TimeOnly(18, 0),
                    Rate = 1
                }
            });

        var reserve = new AuditoriumReserve();

        _mapperMock
            .Setup(x => x.Map<AuditoriumReserveModel, AuditoriumReserve>(model))
            .Returns(reserve);

        _mapperMock
            .Setup(x => x.Map<AuditoriumReserve, AuditoriumReserveDto>(reserve))
            .Returns(new AuditoriumReserveDto());

        // Act
        var result = await _service.CreateReserveAsync(model);

        // Assert
        Assert.True(result.IsSuccess);

        _auditoriumServiceRepositoryMock.Verify(
            x => x.GetByIdsAsync(
                auditoriumId,
                It.Is<IEnumerable<Guid>>(ids =>
                    ids.Count() == 1 &&
                    ids.Single() == serviceId)),
            Times.Once);
    }

    [Fact]
    public async Task CreateReserveAsync_ShouldApplyDifferentRates_WhenReservationCrossesRateBoundary()
    {
        // Arrange
        var auditoriumId = Guid.NewGuid();

        var model = new AuditoriumReserveModel
        {
            AuditoriumId = auditoriumId,
            Date = new DateTime(2026, 8, 28, 17, 0, 0),
            Duration = TimeSpan.FromHours(3)
        };

        var auditorium = new Auditorium
        {
            Id = auditoriumId,
            BaseRentalPrice = 1000
        };

        _auditoriumRepositoryMock
            .Setup(x => x.GetByIdAsync(auditoriumId))
            .ReturnsAsync(auditorium);

        _reserveRepositoryMock
            .Setup(x => x.IsBusyAsync(
                It.IsAny<Guid>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .ReturnsAsync(false);

        _auditoriumServiceRepositoryMock
            .Setup(x => x.GetByIdsAsync(
                auditoriumId,
                It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<AuditoriumService>());

        _timeRateRepositoryMock
            .Setup(x => x.GetListAsync())
            .ReturnsAsync(new List<TimeRate>
            {
                new()
                {
                    StartTime = new TimeOnly(9, 0),
                    EndTime = new TimeOnly(17, 59),
                    Rate = 1m
                },
                new()
                {
                    StartTime = new TimeOnly(18, 0),
                    EndTime = new TimeOnly(23, 0),
                    Rate = 1.5m
                }
            });

        var reserve = new AuditoriumReserve();

        _mapperMock
            .Setup(x => x.Map<AuditoriumReserveModel, AuditoriumReserve>(model))
            .Returns(reserve);

        _mapperMock
            .Setup(x => x.Map<AuditoriumReserve, AuditoriumReserveDto>(reserve))
            .Returns(new AuditoriumReserveDto() { TotalPrice = 4000 });

        // Act
        var result = await _service.CreateReserveAsync(model);

        // Assert
        Assert.True(result.IsSuccess);

        // 17-18 = 1 * 1000 * 1.0 = 1000
        // 18-20 = 2 * 1000 * 1.5 = 3000
        // Total = 4000
        Assert.Equal(4000, result.Value.TotalPrice);
    }

    private static AuditoriumReserveModel CreateModel(
        Guid auditoriumId,
        params Guid[] serviceIds)
    {
        return new AuditoriumReserveModel
        {
            AuditoriumId = auditoriumId,
            Date = new DateTime(2026, 8, 28, 10, 0, 0),
            Duration = TimeSpan.FromHours(2),
            Services = serviceIds
                .Select(x => new AuditoriumServiceModel
                {
                    Id = x
                })
                .ToList()
        };
    }
}