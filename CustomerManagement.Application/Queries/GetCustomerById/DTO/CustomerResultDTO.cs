namespace CustomerManagement.Application.Queries.GetCustomerResultById.DTO
{
    public class CustomerResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}
