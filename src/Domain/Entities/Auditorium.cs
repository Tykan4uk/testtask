namespace Domain.Entities
{
    public class Auditorium
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public int BaseRentalPrice { get; set; }

        public virtual ICollection<AuditoriumService> AuditoriumServices { get; set; }
            = new List<AuditoriumService>();
    }
}
