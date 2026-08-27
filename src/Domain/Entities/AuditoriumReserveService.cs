namespace Domain.Entities
{
    public class AuditoriumReserveService
    {
        public Guid AuditoriumReserveId { get; set; }

        public Guid AuditoriumServiceId { get; set; }

        public AuditoriumReserve AuditoriumReserve { get; set; }

        public AuditoriumService AuditoriumService { get; set; }
    }
}
