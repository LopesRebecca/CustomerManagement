using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Entities
{
    public class Cliente
    {
        public virtual int Id { get; protected set; }
        public virtual string Nome { get; protected set; } = default!;
        public virtual NumeroDocumento NumeroDocumento { get; protected set; } = default!;
        public virtual bool Ativo { get; protected set; }

        protected Cliente() { }

        public Cliente(string nome, NumeroDocumento documentNumber)
        {
            ValidarNome(nome);
            
            Nome = nome;
            NumeroDocumento = documentNumber ?? throw new DomainException("Documento é obrigatório.");
            Ativo = true;
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome não pode ser vazio.");

            if (nome.Length < 2)
                throw new DomainException("Nome deve ter pelo menos 2 caracteres.");

            if (nome.Length > 200)
                throw new DomainException("Nome deve ter no máximo 200 caracteres.");
        }
    }
}
