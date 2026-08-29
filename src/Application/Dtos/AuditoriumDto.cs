namespace Application.Dtos
{
    public class AuditoriumDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public int BaseRentalPrice { get; set; }

        public virtual ICollection<AuditoriumServiceDto> AuditoriumServices { get; set; }
            = new List<AuditoriumServiceDto>();
    }
}
