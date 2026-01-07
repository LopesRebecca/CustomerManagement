namespace CustomerManagement.Application.Customer.DTO
{
    public class GetCustomerByIdResultDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Document { get; set; }
        public bool IsActive { get; set; }

        public GetCustomerByIdResultDTO(int id, string name, string document, bool isActive)
        {
            Id = id;
            Name = name;
            Document = document;
            IsActive = isActive;
        }

        public GetCustomerByIdResultDTO()
        {
        }
    }
}
