using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Customer.Handlers;
using CustomerManagement.Application.Customer.Queries;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;
using Moq;

namespace CustomerManagement.Tests.Application
{
    public class GetCustomerByIdQueryHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly GetCustomerByIdQueryHandler _handler;

        public GetCustomerByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            _handler = new GetCustomerByIdQueryHandler(_repositoryMock.Object);
        }

        #region Success Cases

        [Fact]
        public async Task Handle_WithExistingCustomer_ShouldReturnCustomerDTO()
        {
            // Arrange
            var query = new GetCustomerByIdQuery { Id = 1 };
            var document = DocumentNumber.Create("529.982.247-25");
            var customer = new CustomerEntity("João Silva", document);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _handler.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("João Silva", result.Name);
            Assert.Equal("52998224725", result.Document);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task Handle_WithExistingCustomerCNPJ_ShouldReturnCustomerDTO()
        {
            // Arrange
            var query = new GetCustomerByIdQuery { Id = 2 };
            var document = DocumentNumber.Create("11.444.777/0001-61");
            var customer = new CustomerEntity("Empresa Teste LTDA", document);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _handler.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Empresa Teste LTDA", result.Name);
            Assert.Equal("11444777000161", result.Document);
        }

        [Fact]
        public async Task Handle_WithInactiveCustomer_ShouldReturnIsActiveFalse()
        {
            // Arrange
            var query = new GetCustomerByIdQuery { Id = 1 };
            var document = DocumentNumber.Create("529.982.247-25");
            var customer = new CustomerEntity("João Silva", document);
            customer.Deactivate();

            _repositoryMock
                .Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _handler.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsActive);
        }

        #endregion

        #region Not Found Cases

        [Fact]
        public async Task Handle_WithNonExistingCustomer_ShouldReturnNull()
        {
            // Arrange
            var query = new GetCustomerByIdQuery { Id = 999 };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CustomerEntity?)null);

            // Act
            var result = await _handler.Handle(query);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task Handle_WithInvalidId_ShouldReturnNull(int invalidId)
        {
            // Arrange
            var query = new GetCustomerByIdQuery { Id = invalidId };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(invalidId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CustomerEntity?)null);

            // Act
            var result = await _handler.Handle(query);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Repository Interaction Tests

        [Fact]
        public async Task Handle_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            var query = new GetCustomerByIdQuery { Id = 42 };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(42, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CustomerEntity?)null);

            // Act
            await _handler.Handle(query);

            // Assert
            _repositoryMock.Verify(
                r => r.GetByIdAsync(42, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryOnlyOnce()
        {
            // Arrange
            var query = new GetCustomerByIdQuery { Id = 1 };
            var document = DocumentNumber.Create("529.982.247-25");
            var customer = new CustomerEntity("João Silva", document);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            // Act
            await _handler.Handle(query);

            // Assert
            _repositoryMock.Verify(
                r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion

        #region DTO Mapping Tests

        [Fact]
        public async Task Handle_ShouldMapAllPropertiesCorrectly()
        {
            // Arrange
            var query = new GetCustomerByIdQuery { Id = 1 };
            var document = DocumentNumber.Create("529.982.247-25");
            var customer = new CustomerEntity("Maria Santos", document);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _handler.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetCustomerByIdResultDTO>(result);
            Assert.Equal(customer.Id, result.Id);
            Assert.Equal(customer.Name, result.Name);
            Assert.Equal(customer.DocumentNumber.ToString(), result.Document);
            Assert.Equal(customer.Active, result.IsActive);
        }

        #endregion
    }
}
