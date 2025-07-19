namespace OutlistService.Domain.Entities
{
    public class OutlistProduct
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductCode { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public bool IsActive(DateTime now) => now >= ValidFrom && now <= ValidTo;
    }
}
