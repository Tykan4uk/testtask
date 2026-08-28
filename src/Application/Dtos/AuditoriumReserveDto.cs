namespace Application.Dtos
{
    public class AuditoriumReserveDto
    {
        public Guid Id { get; set; }

        public Guid AuditoriumId { get; set; }

        public DateTime DateTime { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
