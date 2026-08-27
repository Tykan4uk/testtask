namespace Domain.Entities
{
    public class TimeRate
    {
        public Guid Id { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public decimal Rate {  get; set; }
    }
}
