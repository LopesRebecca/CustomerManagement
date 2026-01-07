using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Domain.ValueObjects
{
    public sealed class DocumentNumber : IEquatable<DocumentNumber>
    {
        public string Value { get; }
        public DocumentType Type { get; }

        // Construtor para NHibernate
        private DocumentNumber() { }

        private DocumentNumber(string valor, DocumentType tipo)
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
                11 => DocumentType.Cpf,
                14 => DocumentType.Cnpj,
                _ => throw new DomainException("Documento com quantidade de dígitos inválida.")
            };

            switch (type)
            {
                case DocumentType.Cpf:
                    ValidarCpf(digit);
                    break;

                case DocumentType.Cnpj:
                    ValidarCnpj(digit);
                    break;

                default:
                    throw new DomainException("Tipo de documento inválido.");
            }

            return new DocumentNumber(digit, type);
        }

        public override string ToString() => Tipo == DocumentType.Cpf
            ? FormatarCpf(Valor)
            : FormatarCnpj(Valor);

        public override string ToString() => Value;

        private static string FormatarCnpj(string cnpj)
            => $"{cnpj[..2]}.{cnpj[2..5]}.{cnpj[5..8]}/{cnpj[8..12]}-{cnpj[12..]}";

        private static void ValidarCpf(string cpf)
        {
            if (cpf.Length != 11)
                throw new DomainException("CPF inválido.");

            if (cpf.All(d => d == cpf[0]))
                throw new DomainException("CPF inválido.");

            // Validação dos dígitos verificadores
            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            if (cpf[9].ToString() != digito1.ToString() || cpf[10].ToString() != digito2.ToString())
                throw new DomainException("CPF inválido.");
        }

        private static void ValidarCnpj(string cnpj)
        {
            if (cnpj.Length != 14)
                throw new DomainException("CNPJ inválido.");

            if (cnpj.All(d => d == cnpj[0]))
                throw new DomainException("CNPJ inválido.");

            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCnpj += digito1;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            if (cnpj[12].ToString() != digito1.ToString() || cnpj[13].ToString() != digito2.ToString())
                throw new DomainException("CNPJ inválido.");
        }

        public override bool Equals(object? obj) => Equals(obj as DocumentNumber);

        public override int GetHashCode() => HashCode.Combine(Valor, Tipo);

        public static bool operator ==(DocumentNumber? left, DocumentNumber? right)
            => left?.Equals(right) ?? right is null;

        public static bool operator !=(DocumentNumber? left, DocumentNumber? right)
            => !(left == right);
    }
}
