using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Domain.Extensions
{
    public static class DocumentTypeExtensions
    {
        public static ValidationsGeneral.Factory.DocumentType ToValidationsDocumentType(this Enums.DocumentType documentType)
        {
            return documentType switch
            {
                Enums.DocumentType.Cpf => ValidationsGeneral.Factory.DocumentType.Cpf,
                Enums.DocumentType.Cnpj => ValidationsGeneral.Factory.DocumentType.Cnpj,
                _ => throw new DomainException("Tipo de documento não suportado para conversão.")
            };
        }

        public static Enums.DocumentType ToDomainDocumentType(this ValidationsGeneral.Factory.DocumentType documentType)
        {
            return documentType switch
            {
                ValidationsGeneral.Factory.DocumentType.Cpf => Enums.DocumentType.Cpf,
                ValidationsGeneral.Factory.DocumentType.Cnpj => Enums.DocumentType.Cnpj,
                _ => throw new DomainException("Tipo de documento não suportado para conversão.")
            };
        }
    }
}