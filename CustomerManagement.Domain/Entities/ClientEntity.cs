using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Entities
{
    public class ClientEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DocumentNumber DocumentNumber { get; private set; }
        public bool Active { get; private set; }

        protected ClientEntity() { }

        public ClientEntity(string fantasyName, DocumentNumber documentNumber)
        {
            Name = fantasyName;
            DocumentNumber = documentNumber;
            Active = true;
        }
        public void Deactivate()
        {
            if (!Active)
                throw new DomainException("Cliente já está inativo.");
            Active = false;
        }

        public void Activate()
        {
            if (Active)
                throw new DomainException("Cliente já está ativo.");
            Active = true;
        }

        public void Validate(string fantasyName, string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(fantasyName))
                throw new DomainException("Nome fantasia não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new DomainException("Número do documento não pode ser vazio.");
        }
    }
}
