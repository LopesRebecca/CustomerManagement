using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Tests.Domain
{
    public class CustomerEntityTests
    {
        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidData_ShouldCreateCustomer()
        {
            // Arrange
            var name = "João Silva";
            var document = DocumentNumber.Create("529.982.247-25");

            // Act
            var customer = new CustomerEntity(name, document);

            // Assert
            Assert.Equal(name, customer.Name);
            Assert.Equal(document, customer.DocumentNumber);
            Assert.True(customer.Active);
        }

        [Fact]
        public void Constructor_WithValidData_ShouldSetActiveToTrue()
        {
            // Arrange
            var name = "Empresa Teste LTDA";
            var document = DocumentNumber.Create("11.444.777/0001-61");

            // Act
            var customer = new CustomerEntity(name, document);

            // Assert
            Assert.True(customer.Active);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithEmptyName_ShouldThrowDomainException(string? name)
        {
            // Arrange
            var document = DocumentNumber.Create("529.982.247-25");

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new CustomerEntity(name!, document));
            Assert.Equal("Nome não pode ser vazio.", exception.Message);
        }

        [Fact]
        public void Constructor_WithNameTooShort_ShouldThrowDomainException()
        {
            // Arrange
            var document = DocumentNumber.Create("529.982.247-25");

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new CustomerEntity("A", document));
            Assert.Equal("Nome deve ter pelo menos 2 caracteres.", exception.Message);
        }

        [Fact]
        public void Constructor_WithNameTooLong_ShouldThrowDomainException()
        {
            // Arrange
            var document = DocumentNumber.Create("529.982.247-25");
            var longName = new string('A', 201);

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new CustomerEntity(longName, document));
            Assert.Equal("Nome deve ter no máximo 200 caracteres.", exception.Message);
        }

        [Fact]
        public void Constructor_WithNullDocument_ShouldThrowDomainException()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new CustomerEntity("João Silva", null!));
            Assert.Equal("Documento é obrigatório.", exception.Message);
        }

        [Fact]
        public void Constructor_WithNameAtMinLength_ShouldCreateCustomer()
        {
            // Arrange
            var document = DocumentNumber.Create("529.982.247-25");

            // Act
            var customer = new CustomerEntity("AB", document);

            // Assert
            Assert.Equal("AB", customer.Name);
        }

        [Fact]
        public void Constructor_WithNameAtMaxLength_ShouldCreateCustomer()
        {
            // Arrange
            var document = DocumentNumber.Create("529.982.247-25");
            var maxName = new string('A', 200);

            // Act
            var customer = new CustomerEntity(maxName, document);

            // Assert
            Assert.Equal(200, customer.Name.Length);
        }

        #endregion

        #region Deactivate Tests

        [Fact]
        public void Deactivate_WhenActive_ShouldSetActiveToFalse()
        {
            // Arrange
            var customer = new CustomerEntity("João Silva", DocumentNumber.Create("529.982.247-25"));
            Assert.True(customer.Active);

            // Act
            customer.Deactivate();

            // Assert
            Assert.False(customer.Active);
        }

        [Fact]
        public void Deactivate_WhenAlreadyInactive_ShouldThrowDomainException()
        {
            // Arrange
            var customer = new CustomerEntity("João Silva", DocumentNumber.Create("529.982.247-25"));
            customer.Deactivate();

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => customer.Deactivate());
            Assert.Equal("Cliente já está inativo.", exception.Message);
        }

        #endregion

        #region Activate Tests

        [Fact]
        public void Activate_WhenInactive_ShouldSetActiveToTrue()
        {
            // Arrange
            var customer = new CustomerEntity("João Silva", DocumentNumber.Create("529.982.247-25"));
            customer.Deactivate();
            Assert.False(customer.Active);

            // Act
            customer.Activate();

            // Assert
            Assert.True(customer.Active);
        }

        [Fact]
        public void Activate_WhenAlreadyActive_ShouldThrowDomainException()
        {
            // Arrange
            var customer = new CustomerEntity("João Silva", DocumentNumber.Create("529.982.247-25"));

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => customer.Activate());
            Assert.Equal("Cliente já está ativo.", exception.Message);
        }

        #endregion

        #region UpdateName Tests

        [Fact]
        public void UpdateName_WithValidName_ShouldUpdateName()
        {
            // Arrange
            var customer = new CustomerEntity("João Silva", DocumentNumber.Create("529.982.247-25"));
            var newName = "João Santos";

            // Act
            customer.UpdateName(newName);

            // Assert
            Assert.Equal(newName, customer.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UpdateName_WithEmptyName_ShouldThrowDomainException(string? name)
        {
            // Arrange
            var customer = new CustomerEntity("João Silva", DocumentNumber.Create("529.982.247-25"));

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => customer.UpdateName(name!));
            Assert.Equal("Nome não pode ser vazio.", exception.Message);
        }

        [Fact]
        public void UpdateName_WithNameTooShort_ShouldThrowDomainException()
        {
            // Arrange
            var customer = new CustomerEntity("João Silva", DocumentNumber.Create("529.982.247-25"));

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => customer.UpdateName("A"));
            Assert.Equal("Nome deve ter pelo menos 2 caracteres.", exception.Message);
        }

        [Fact]
        public void UpdateName_WithNameTooLong_ShouldThrowDomainException()
        {
            // Arrange
            var customer = new CustomerEntity("João Silva", DocumentNumber.Create("529.982.247-25"));
            var longName = new string('A', 201);

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => customer.UpdateName(longName));
            Assert.Equal("Nome deve ter no máximo 200 caracteres.", exception.Message);
        }

        #endregion
    }
}
