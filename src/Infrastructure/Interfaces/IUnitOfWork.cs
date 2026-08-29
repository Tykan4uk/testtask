namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IAuditoriumRepository Auditoriums { get; }
        IAuditoriumReserveRepository AuditoriumReserves { get; }
        IAuditoriumServiceRepository AuditoriumServices { get; }
        ITimeRateRepository TimeRates { get; }
        IUserInfoRepository UserInfos { get; }

        Task<int> SaveChangesAsync();
    }
}
