using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Domain.Extensions
{
    public static class TipoDeDocumentoExtensao
    {
        public static ValidationsGeneral.Factory.DocumentType ToValidationsDocumentType(this Enums.TipoDeDocumento documentType)
        {
            return documentType switch
            {
                Enums.TipoDeDocumento.Cpf => ValidationsGeneral.Factory.DocumentType.Cpf,
                Enums.TipoDeDocumento.Cnpj => ValidationsGeneral.Factory.DocumentType.Cnpj,
                _ => throw new DomainException("Tipo de documento não suportado para conversão.")
            };
        }

        public static Enums.TipoDeDocumento ToDomainDocumentType(this ValidationsGeneral.Factory.DocumentType documentType)
        {
            return documentType switch
            {
                ValidationsGeneral.Factory.DocumentType.Cpf => Enums.TipoDeDocumento.Cpf,
                ValidationsGeneral.Factory.DocumentType.Cnpj => Enums.TipoDeDocumento.Cnpj,
                _ => throw new DomainException("Tipo de documento não suportado para conversão.")
            };
        }
    }
}