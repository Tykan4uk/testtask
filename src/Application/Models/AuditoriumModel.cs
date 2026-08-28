namespace Application.Models
{
    public class AuditoriumModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public int BaseRentalPrice { get; set; }

        public IEnumerable<ServiceModel> Services { get; set; } = new List<ServiceModel>();
    }
}
