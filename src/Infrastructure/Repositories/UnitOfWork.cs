using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TestTaskDbContext _context;

        private IAuditoriumRepository? _auditoriums;
        private IAuditoriumReserveRepository? _auditoriumReserves;
        private IAuditoriumServiceRepository? _auditoriumServices;
        private ITimeRateRepository? _timeRates;
        private IUserInfoRepository? _userInfos;

        public UnitOfWork(TestTaskDbContext context)
        {
            _context = context;
        }


        public IAuditoriumRepository Auditoriums => _auditoriums ??= new AuditoriumRepository(_context);
        public IAuditoriumReserveRepository AuditoriumReserves => _auditoriumReserves ??= new AuditoriumReserveRepository(_context);
        public IAuditoriumServiceRepository AuditoriumServices => _auditoriumServices ??= new AuditoriumServiceRepository(_context);
        public ITimeRateRepository TimeRates => _timeRates ??= new TimeRateRepository(_context);
        public IUserInfoRepository UserInfos => _userInfos ??= new UserInfoRepository(_context);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
