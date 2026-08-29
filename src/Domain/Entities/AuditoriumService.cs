namespace Domain.Entities
{
    public class AuditoriumService
    {
        public Guid Id { get; set; }

        public Guid AuditoriumId { get; set; }

        public Guid ServiceId { get; set; }

        public Auditorium Auditorium { get; set; }

        public Service Service { get; set; }
    }
}
