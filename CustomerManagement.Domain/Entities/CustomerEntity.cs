using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Entities
{
    public class CustomerEntity
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; } = default!;
        public virtual DocumentNumber DocumentNumber { get; protected set; } = default!;
        public virtual bool Active { get; protected set; }

        protected CustomerEntity() { }

        public CustomerEntity(string name, DocumentNumber documentNumber)
        {
            ValidateName(name);
            
            Name = name;
            DocumentNumber = documentNumber ?? throw new DomainException("Documento é obrigatório.");
            Active = true;
        }

        public virtual void Deactivate()
        {
            if (!Active)
                throw new DomainException("Cliente já está inativo.");
            Active = false;
        }

        public virtual void Activate()
        {
            if (Active)
                throw new DomainException("Cliente já está ativo.");
            Active = true;
        }

        public virtual void UpdateName(string name)
        {
            ValidateName(name);
            Name = name;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Nome não pode ser vazio.");

            if (name.Length < 2)
                throw new DomainException("Nome deve ter pelo menos 2 caracteres.");

            if (name.Length > 200)
                throw new DomainException("Nome deve ter no máximo 200 caracteres.");
        }
    }
}
