using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Domain.ValueObjects
{
    public sealed class DocumentNumber
    {
        public string Valor { get; }
        public DocumentType Tipo { get; }

        private DocumentNumber(string valor, DocumentType tipo)
        {
            Valor = valor;
            Tipo = tipo;
        }

        public static DocumentNumber Create(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException("Documento não pode ser vazio.");

            var numerico = new string(valor.Where(char.IsDigit).ToArray());

            var tipo = numerico.Length switch
            {
                11 => DocumentType.Cpf,
                14 => DocumentType.Cnpj,
                _ => throw new DomainException("Documento com quantidade de dígitos inválida.")
            };

            switch (tipo)
            {
                case DocumentType.Cpf:
                    ValidarCpf(numerico);
                    break;

                case DocumentType.Cnpj:
                    ValidarCnpj(numerico);
                    break;

                default:
                    throw new DomainException("Tipo de documento inválido.");
            }

            return new DocumentNumber(numerico, tipo);
        }


        public override string ToString() => Valor;

        private static void ValidarCpf(string cpf)
        {
            if (cpf.Length != 11)
                throw new DomainException("CPF inválido.");
        }

        private static void ValidarCnpj(string cnpj)
        {
            if (cnpj.Length != 14)
                throw new DomainException("CNPJ inválido.");
        }

    }
}
