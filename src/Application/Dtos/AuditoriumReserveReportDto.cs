namespace Application.Dtos
{
    public class AuditoriumReserveReportDto
    {
        public List<AuditoriumReserveDto> Reserves { get; set; }

        public int TotalPrice { get; set; }
    }
}
