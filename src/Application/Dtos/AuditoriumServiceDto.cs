namespace Application.Dtos
{
    public class AuditoriumServiceDto
    {
        public Guid Id { get; set; }

        public Guid AuditoriumId { get; set; }

        public Guid ServiceId { get; set; }

        public string ServiceName { get; set; }

        public int ServicePrice { get; set; }
    }
}
