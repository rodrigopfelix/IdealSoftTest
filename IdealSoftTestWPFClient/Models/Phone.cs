namespace IdealSoftTestWPFClient.Models
{
    public class Phone
    {
        public Guid? Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string RegionCode { get; set; } = "BR";
        public string Type { get; set; } = string.Empty;
    }
}
