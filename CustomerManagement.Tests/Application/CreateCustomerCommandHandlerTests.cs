using CustomerManagement.Application.Customer.Commands;
using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Customer.Handlers;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;
using Moq;

namespace CustomerManagement.Tests.Application
{
    public class CreateCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly CreateCustomerCommandHandler _handler;

        public CreateCustomerCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            _handler = new CreateCustomerCommandHandler(_repositoryMock.Object);
        }

        #region Success Cases

        [Fact]
        public async Task Handle_WithValidCPF_ShouldReturnSuccess()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "João Silva",
                DocumentNumber = "529.982.247-25"
            };

            _repositoryMock
                .Setup(r => r.ExistDocumentNumberAsync(It.IsAny<DocumentNumber>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Cadastro realizado com sucesso!", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidCNPJ_ShouldReturnSuccess()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "Empresa Teste LTDA",
                DocumentNumber = "11.444.777/0001-61"
            };

            _repositoryMock
                .Setup(r => r.ExistDocumentNumberAsync(It.IsAny<DocumentNumber>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }

        #endregion

        #region Duplicate Document Cases

        [Fact]
        public async Task Handle_WithDuplicateDocument_ShouldReturnFailed()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "João Silva",
                DocumentNumber = "529.982.247-25"
            };

            _repositoryMock
                .Setup(r => r.ExistDocumentNumberAsync(It.IsAny<DocumentNumber>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Documento já cadastrado.", result.Message);
        }

        [Fact]
        public async Task Handle_WithDuplicateDocument_ShouldNotCallCreateAsync()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "João Silva",
                DocumentNumber = "529.982.247-25"
            };

            _repositoryMock
                .Setup(r => r.ExistDocumentNumberAsync(It.IsAny<DocumentNumber>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command);

            // Assert
            _repositoryMock.Verify(
                r => r.CreateAsync(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        #endregion

        #region Invalid Document Cases

        [Theory]
        [InlineData("111.111.111-11")]
        [InlineData("123.456.789-00")]
        [InlineData("11.111.111/1111-11")]
        public async Task Handle_WithInvalidDocument_ShouldReturnFailed(string invalidDocument)
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "João Silva",
                DocumentNumber = invalidDocument
            };

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("inválido", result.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Handle_WithEmptyDocument_ShouldReturnFailed(string emptyDocument)
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "João Silva",
                DocumentNumber = emptyDocument
            };

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Documento não pode ser vazio.", result.Message);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("12345678")]
        [InlineData("123456789012345")]
        public async Task Handle_WithInvalidDigitCount_ShouldReturnFailed(string invalidDocument)
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "João Silva",
                DocumentNumber = invalidDocument
            };

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Documento com quantidade de dígitos inválida.", result.Message);
        }

        #endregion

        #region Invalid Name Cases

        [Fact]
        public async Task Handle_WithEmptyName_ShouldReturnFailed()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "",
                DocumentNumber = "529.982.247-25"
            };

            _repositoryMock
                .Setup(r => r.ExistDocumentNumberAsync(It.IsAny<DocumentNumber>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nome não pode ser vazio.", result.Message);
        }

        [Fact]
        public async Task Handle_WithNameTooShort_ShouldReturnFailed()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "A",
                DocumentNumber = "529.982.247-25"
            };

            _repositoryMock
                .Setup(r => r.ExistDocumentNumberAsync(It.IsAny<DocumentNumber>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nome deve ter pelo menos 2 caracteres.", result.Message);
        }

        #endregion
    }
}
