namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IAuditoriumRepository Auditoriums { get; }
        IAuditoriumReserveRepository AuditoriumReservs { get; }

        Task<int> SaveChangesAsync();
    }
}
