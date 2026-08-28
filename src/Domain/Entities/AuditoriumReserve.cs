namespace Domain.Entities
{
    public class AuditoriumReserve
    {
        public Guid Id { get; set; }

        public Guid AuditoriumId { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public Auditorium Auditorium { get; set; }

        public virtual ICollection<AuditoriumReserveService> Services { get; set; }
            = new List<AuditoriumReserveService>();

    }
}
