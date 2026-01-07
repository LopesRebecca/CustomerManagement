using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Extensions;
using ValidationsGeneral.Factory;

namespace CustomerManagement.Domain.ValueObjects
{
    public sealed class DocumentNumber : IEquatable<DocumentNumber>
    {
        public string Value { get; }
        public Enums.DocumentType Type { get; }

        private DocumentNumber() { }

        private DocumentNumber(string valor, Enums.DocumentType tipo)
        {
            Value = valor;
            Type = tipo;
        }

        public static DocumentNumber Create(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException("Documento não pode ser vazio.");

            var digit = new string(valor.Where(char.IsDigit).ToArray());

            var type = digit.Length switch
            {
                11 => Enums.DocumentType.Cpf,
                14 => Enums.DocumentType.Cnpj,
                _ => throw new DomainException("Documento com quantidade de dígitos inválida.")
            };

            var validator = ValidatorFactory.Create(type.ToValidationsDocumentType());

            switch (type)
            {
                case Enums.DocumentType.Cpf:
                    if (!validator.Validate(digit).IsValid)
                        throw new DomainException("CPF inválido.");
                    break;

                case Enums.DocumentType.Cnpj:
                    if (!validator.Validate(digit).IsValid)
                        throw new DomainException("CNPJ inválido.");
                    break;

                default:
                    throw new DomainException("Tipo de documento inválido.");
            }

            return new DocumentNumber(digit, type);
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) => Equals(obj as DocumentNumber);

        public override int GetHashCode() => HashCode.Combine(Value, Type);

        public bool Equals(DocumentNumber? other)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(DocumentNumber? left, DocumentNumber? right)
            => left?.Equals(right) ?? right is null;

        public static bool operator !=(DocumentNumber? left, DocumentNumber? right)
            => !(left == right);
    }
}
