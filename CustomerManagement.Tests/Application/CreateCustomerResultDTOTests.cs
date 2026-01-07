using CustomerManagement.Application.Customer.DTO;

namespace CustomerManagement.Tests.Application
{
    public class CreateCustomerResultDTOTests
    {
        #region Ok Factory Method Tests

        [Fact]
        public void Ok_ShouldReturnSuccessResult()
        {
            // Arrange
            var customerId = 1;

            // Act
            var result = CreateCustomerResultDTO.Ok(customerId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Cadastro realizado com sucesso!", result.Message);
            Assert.Equal(customerId, result.CustomerId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999999)]
        public void Ok_WithDifferentIds_ShouldReturnCorrectId(int customerId)
        {
            // Act
            var result = CreateCustomerResultDTO.Ok(customerId);

            // Assert
            Assert.Equal(customerId, result.CustomerId);
        }

        #endregion

        #region Failed Factory Method Tests

        [Fact]
        public void Failed_ShouldReturnFailedResult()
        {
            // Arrange
            var errorMessage = "Documento já cadastrado.";

            // Act
            var result = CreateCustomerResultDTO.Failed(errorMessage);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(errorMessage, result.Message);
            Assert.Null(result.CustomerId);
        }

        [Theory]
        [InlineData("Documento já cadastrado.")]
        [InlineData("CPF inválido.")]
        [InlineData("Nome não pode ser vazio.")]
        [InlineData("Erro interno.")]
        public void Failed_WithDifferentMessages_ShouldReturnCorrectMessage(string errorMessage)
        {
            // Act
            var result = CreateCustomerResultDTO.Failed(errorMessage);

            // Assert
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public void Failed_ShouldSetCustomerIdToNull()
        {
            // Act
            var result = CreateCustomerResultDTO.Failed("Qualquer erro");

            // Assert
            Assert.Null(result.CustomerId);
        }

        #endregion
    }
}
