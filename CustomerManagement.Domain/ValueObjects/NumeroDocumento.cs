using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Extensions;
using ValidationsGeneral.Factory; //Biblioteca criada por mim para validações gerais

namespace CustomerManagement.Domain.ValueObjects
{
    public sealed class NumeroDocumento : IEquatable<NumeroDocumento>
    {
        public string Valor { get; }
        public Enums.TipoDeDocumento Tipo { get; }

        private NumeroDocumento() { }

        private NumeroDocumento(string valor, Enums.TipoDeDocumento tipo)
        {
            Valor = valor;
            Tipo = tipo;
        }

        public static NumeroDocumento Create(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException("Documento não pode ser vazio.");

            var numerico = new string(valor.Where(char.IsDigit).ToArray());

            var tipo = numerico.Length switch
            {
                11 => Enums.TipoDeDocumento.Cpf,
                14 => Enums.TipoDeDocumento.Cnpj,
                _ => throw new DomainException("Documento com quantidade de dígitos inválida.")
            };

            var validacao = ValidatorFactory.Create(tipo.ToValidationsDocumentType());

            switch (tipo)
            {
                case Enums.TipoDeDocumento.Cpf:
                    if (!validacao.Validate(numerico).IsValid)
                        throw new DomainException("CPF inválido.");
                    break;

                case Enums.TipoDeDocumento.Cnpj:
                    if (!validacao.Validate(numerico).IsValid)
                        throw new DomainException("CNPJ inválido.");
                    break;

                default:
                    throw new DomainException("Tipo de documento inválido.");
            }

            return new NumeroDocumento(numerico, tipo);
        }

        public override string ToString() => Valor;

        public override bool Equals(object? obj) => Equals(obj as NumeroDocumento);

        public override int GetHashCode() => HashCode.Combine(Valor, Tipo);

        public bool Equals(NumeroDocumento? other)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(NumeroDocumento? left, NumeroDocumento? right)
            => left?.Equals(right) ?? right is null;

        public static bool operator !=(NumeroDocumento? left, NumeroDocumento? right)
            => !(left == right);
    }
}
