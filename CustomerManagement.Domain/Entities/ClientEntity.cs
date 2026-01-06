using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Entities
{
    public class ClientEntity
    {
        public int Id { get; private set; }
        public string FantasyName { get; private set; }
        public DocumentNumber DocumentNumber { get; private set; }
        public bool Ativo { get; private set; }

        protected ClientEntity() { }

        public ClientEntity(string fantasyName, DocumentNumber documentNumber)
        {
            FantasyName = fantasyName;
            DocumentNumber = documentNumber;
            Ativo = true;
        }
    }
}
