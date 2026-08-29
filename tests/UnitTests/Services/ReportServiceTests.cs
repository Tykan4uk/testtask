using Application.Dtos;
using Application.Models;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Resources;
using Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace UnitTests.Services;

public class ReportServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IAuditoriumReserveRepository> _reserveRepositoryMock = new();

    private readonly ReportService _service;

    public ReportServiceTests()
    {
        _unitOfWorkMock
            .Setup(x => x.AuditoriumReserves)
            .Returns(_reserveRepositoryMock.Object);

        _service = new ReportService(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetReserveReportAsync_ShouldReturnBadRequest_WhenFromIsGreaterThanTo()
    {
        // Arrange
        var model = new AuditoriumReserveReportModel
        {
            From = new DateTime(2026, 8, 20),
            To = new DateTime(2026, 8, 10)
        };

        // Act
        var result = await _service.GetReserveReportAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Error.Code);
        Assert.Equal(
            ErrorReasons.StartTimeBiggerThanEnd,
            result.Error.Message);

        _reserveRepositoryMock.Verify(
            x => x.GetByPeriodAsync(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()),
            Times.Never);
    }

    [Fact]
    public async Task GetReserveReportAsync_ShouldReturnEmptyReport_WhenNoReservesFound()
    {
        // Arrange
        var model = new AuditoriumReserveReportModel
        {
            From = new DateTime(2026, 8, 1),
            To = new DateTime(2026, 8, 31)
        };

        var reserves = new List<AuditoriumReserve>();

        var dtos = new List<AuditoriumReserveDto>();

        _reserveRepositoryMock
            .Setup(x => x.GetByPeriodAsync(model.From, model.To))
            .ReturnsAsync(reserves);

        _mapperMock
            .Setup(x => x.Map<List<AuditoriumReserveDto>>(reserves))
            .Returns(dtos);

        // Act
        var result = await _service.GetReserveReportAsync(model);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        Assert.Empty(result.Value.Reserves);
        Assert.Equal(0, result.Value.TotalPrice);

        _reserveRepositoryMock.Verify(
            x => x.GetByPeriodAsync(model.From, model.To),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<AuditoriumReserveDto>>(reserves),
            Times.Once);
    }

    [Fact]
    public async Task GetReserveReportAsync_ShouldReturnReportWithTotalPrice()
    {
        // Arrange
        var model = new AuditoriumReserveReportModel
        {
            From = new DateTime(2026, 8, 1),
            To = new DateTime(2026, 8, 31)
        };

        var reserves = new List<AuditoriumReserve>
        {
            new()
            {
                Id = Guid.NewGuid(),
                TotalPrice = 1000
            },
            new()
            {
                Id = Guid.NewGuid(),
                TotalPrice = 2500
            },
            new()
            {
                Id = Guid.NewGuid(),
                TotalPrice = 750
            }
        };

        var dtos = new List<AuditoriumReserveDto>
        {
            new() { Id = reserves[0].Id, TotalPrice = 1000 },
            new() { Id = reserves[1].Id, TotalPrice = 2500 },
            new() { Id = reserves[2].Id, TotalPrice = 750 }
        };

        _reserveRepositoryMock
            .Setup(x => x.GetByPeriodAsync(model.From, model.To))
            .ReturnsAsync(reserves);

        _mapperMock
            .Setup(x => x.Map<List<AuditoriumReserveDto>>(reserves))
            .Returns(dtos);

        // Act
        var result = await _service.GetReserveReportAsync(model);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(3, result.Value.Reserves.Count);
        Assert.Equal(4250, result.Value.TotalPrice);

        Assert.Equal(dtos, result.Value.Reserves);

        _reserveRepositoryMock.Verify(
            x => x.GetByPeriodAsync(model.From, model.To),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<AuditoriumReserveDto>>(reserves),
            Times.Once);
    }

    [Fact]
    public async Task GetReserveReportAsync_ShouldAcceptEqualDates()
    {
        // Arrange
        var date = new DateTime(2026, 8, 10);

        var model = new AuditoriumReserveReportModel
        {
            From = date,
            To = date
        };

        // Act
        var result = await _service.GetReserveReportAsync(model);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Error.Code);

        _reserveRepositoryMock.Verify(
            x => x.GetByPeriodAsync(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()),
            Times.Never);
    }
}