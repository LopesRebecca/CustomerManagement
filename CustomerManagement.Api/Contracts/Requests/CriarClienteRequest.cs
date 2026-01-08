using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Api.Contracts.Requests
{
    public class CriarClienteRequest
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 200 caracteres.")]
        public string Nome { get; init; } = default!;

        [Required(ErrorMessage = "Documento é obrigatório.")]
        public string NumeroDocumento { get; init; } = default!;
    }
}
