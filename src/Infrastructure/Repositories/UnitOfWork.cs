using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TestTaskDbContext _context;

        private IAuditoriumRepository? _auditoriums;
        private IAuditoriumReserveRepository? _auditoriumReservs;

        public UnitOfWork(TestTaskDbContext context)
        {
            _context = context;
        }


        public IAuditoriumRepository Auditoriums => _auditoriums ??= new AuditoriumRepository(_context);
        public IAuditoriumReserveRepository AuditoriumReservs => _auditoriumReservs ??= new AuditoriumReserveRepository(_context);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
