using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Tests.Domain
{
    public class DocumentNumberTests
    {
        #region CPF Tests

        [Theory]
        [InlineData("529.982.247-25")]
        [InlineData("52998224725")]
        [InlineData("123.456.789-09")]
        [InlineData("12345678909")]
        public void Create_WithValidCPF_ShouldReturnDocumentNumberWithCpfType(string cpf)
        {
            // Act
            var document = DocumentNumber.Create(cpf);

            // Assert
            Assert.NotNull(document);
            Assert.Equal(DocumentType.Cpf, document.Type);
        }

        [Fact]
        public void Create_WithValidCPF_ShouldStoreOnlyDigits()
        {
            // Arrange
            var cpfWithMask = "529.982.247-25";
            var expectedDigits = "52998224725";

            // Act
            var document = DocumentNumber.Create(cpfWithMask);

            // Assert
            Assert.Equal(expectedDigits, document.Value);
        }

        [Theory]
        [InlineData("111.111.111-11")]
        [InlineData("222.222.222-22")]
        [InlineData("333.333.333-33")]
        [InlineData("000.000.000-00")]
        public void Create_WithRepeatedDigitsCPF_ShouldThrowDomainException(string cpf)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => DocumentNumber.Create(cpf));
            Assert.Equal("CPF inválido.", exception.Message);
        }

        [Theory]
        [InlineData("123.456.789-00")] // Dígitos verificadores incorretos
        [InlineData("529.982.247-26")] // Último dígito errado
        [InlineData("529.982.247-15")] // Ambos dígitos errados
        public void Create_WithInvalidCPFCheckDigits_ShouldThrowDomainException(string cpf)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => DocumentNumber.Create(cpf));
            Assert.Equal("CPF inválido.", exception.Message);
        }

        [Theory]
        [InlineData("123.456.789")] // Menos de 11 dígitos
        [InlineData("12345678")]
        [InlineData("1234567890")] // 10 dígitos
        public void Create_WithCPFLessThan11Digits_ShouldThrowDomainException(string cpf)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => DocumentNumber.Create(cpf));
            Assert.Equal("Documento com quantidade de dígitos inválida.", exception.Message);
        }

        #endregion

        #region CNPJ Tests

        [Theory]
        [InlineData("11.444.777/0001-61")]
        [InlineData("11444777000161")]
        [InlineData("27.865.757/0001-02")]
        [InlineData("27865757000102")]
        public void Create_WithValidCNPJ_ShouldReturnDocumentNumberWithCnpjType(string cnpj)
        {
            // Act
            var document = DocumentNumber.Create(cnpj);

            // Assert
            Assert.NotNull(document);
            Assert.Equal(DocumentType.Cnpj, document.Type);
        }

        [Fact]
        public void Create_WithValidCNPJ_ShouldStoreOnlyDigits()
        {
            // Arrange
            var cnpjWithMask = "11.444.777/0001-61";
            var expectedDigits = "11444777000161";

            // Act
            var document = DocumentNumber.Create(cnpjWithMask);

            // Assert
            Assert.Equal(expectedDigits, document.Value);
        }

        [Theory]
        [InlineData("11.111.111/1111-11")]
        [InlineData("22.222.222/2222-22")]
        [InlineData("00.000.000/0000-00")]
        public void Create_WithRepeatedDigitsCNPJ_ShouldThrowDomainException(string cnpj)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => DocumentNumber.Create(cnpj));
            Assert.Equal("CNPJ inválido.", exception.Message);
        }

        [Theory]
        [InlineData("11.444.777/0001-62")] // Último dígito errado
        [InlineData("11.444.777/0001-71")] // Primeiro dígito verificador errado
        [InlineData("12.345.678/0001-00")] // Dígitos verificadores incorretos
        public void Create_WithInvalidCNPJCheckDigits_ShouldThrowDomainException(string cnpj)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => DocumentNumber.Create(cnpj));
            Assert.Equal("CNPJ inválido.", exception.Message);
        }

        [Theory]
        [InlineData("11.444.777/0001")] // Menos de 14 dígitos
        [InlineData("1144477700016")]   // 13 dígitos
        [InlineData("114447770001")]    // 12 dígitos
        public void Create_WithCNPJLessThan14Digits_ShouldThrowDomainException(string cnpj)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => DocumentNumber.Create(cnpj));
            Assert.Equal("Documento com quantidade de dígitos inválida.", exception.Message);
        }

        #endregion

        #region General Validation Tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithEmptyOrNullValue_ShouldThrowDomainException(string? value)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => DocumentNumber.Create(value!));
            Assert.Equal("Documento não pode ser vazio.", exception.Message);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("12345")]
        [InlineData("123456789012345")] // 15 dígitos
        [InlineData("1234567890123456")] // 16 dígitos
        public void Create_WithInvalidDigitCount_ShouldThrowDomainException(string value)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => DocumentNumber.Create(value));
            Assert.Equal("Documento com quantidade de dígitos inválida.", exception.Message);
        }

        [Fact]
        public void ToString_ShouldReturnValue()
        {
            // Arrange
            var document = DocumentNumber.Create("52998224725");

            // Act
            var result = document.ToString();

            // Assert
            Assert.Equal("52998224725", result);
        }

        #endregion

        #region Equality Tests

        [Fact]
        public void GetHashCode_WithSameValue_ShouldReturnSameHashCode()
        {
            // Arrange
            var document1 = DocumentNumber.Create("52998224725");
            var document2 = DocumentNumber.Create("529.982.247-25");

            // Act & Assert
            Assert.Equal(document1.GetHashCode(), document2.GetHashCode());
        }

        #endregion
    }
}
