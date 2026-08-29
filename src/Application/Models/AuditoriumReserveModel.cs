namespace Application.Models
{
    public class AuditoriumReserveModel
    {
        public Guid AuditoriumId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Duration { get; set; }

        public IEnumerable<AuditoriumServiceModel> Services { get; set; } = new List<AuditoriumServiceModel>();
    }
}
