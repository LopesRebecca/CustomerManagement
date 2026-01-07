namespace CustomerManagement.Application.Customer.DTO
{
    public class CreateCustomerResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? CustomerId { get; set; }

        public CreateCustomerResultDTO(bool sucess, string message, int? customerId)
        {
            Success = sucess;
            Message = message;
            CustomerId = customerId;
        }
        public static CreateCustomerResultDTO Ok(int id)
            => new(true, "Cadastro realizado com sucesso!", id);

        public static CreateCustomerResultDTO Failed(string error)
            => new(false, error, null);
    }
}
