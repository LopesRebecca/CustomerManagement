using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Customer.Handlers;
using CustomerManagement.Application.Customer.Queries;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;
using Moq;

namespace CustomerManagement.Tests.Application
{
    public class BuscarClientePorIdQueryHandlerTests
    {
        private readonly Mock<IClienteRepository> _repositorioMock;
        private readonly BuscarClientePorIdQueryHandler _handler;

        public BuscarClientePorIdQueryHandlerTests()
        {
            _repositorioMock = new Mock<IClienteRepository>();
            _handler = new BuscarClientePorIdQueryHandler(_repositorioMock.Object);
        }

        #region Success Cases

        [Fact]
        public async Task Handle_WithExistingCustomer_ShouldReturnCustomerDTO()
        {
            // Arrange
            var consulta = new BuscarClientePorIdQuery { Id = 1 };
            var documento = NumeroDocumento.Create("529.982.247-25");
            var cliente = new Cliente("João Silva", documento);

            _repositorioMock
                .Setup(r => r.ObterPorIdAsync(consulta.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cliente);

            // Act
            var resultado = await _handler.Handle(consulta);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("João Silva", resultado.Name);
            Assert.Equal("52998224725", resultado.Document);
            Assert.True(resultado.Ativo);
        }

        [Fact]
        public async Task Handle_WithExistingCustomerCNPJ_ShouldReturnCustomerDTO()
        {
            // Arrange
            var consulta = new BuscarClientePorIdQuery { Id = 2 };
            var documento = NumeroDocumento.Create("11.444.777/0001-61");
            var cliente = new Cliente("Empresa Teste LTDA", documento);

            _repositorioMock
                .Setup(r => r.ObterPorIdAsync(consulta.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cliente);

            // Act
            var resultado = await _handler.Handle(consulta);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Empresa Teste LTDA", resultado.Name);
            Assert.Equal("11444777000161", resultado.Document);
        }

        #endregion

        #region Not Found Cases

        [Fact]
        public async Task Handle_WithNonExistingCustomer_ShouldReturnNull()
        {
            // Arrange
            var consulta = new BuscarClientePorIdQuery { Id = 999 };

            _repositorioMock
                .Setup(r => r.ObterPorIdAsync(consulta.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cliente?)null);

            // Act
            var resultado = await _handler.Handle(consulta);

            // Assert
            Assert.Null(resultado);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task Handle_WithInvalidId_ShouldReturnNull(int invalidId)
        {
            // Arrange
            var consulta = new BuscarClientePorIdQuery { Id = invalidId };

            _repositorioMock
                .Setup(r => r.ObterPorIdAsync(invalidId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cliente?)null);

            // Act
            var resultado = await _handler.Handle(consulta);

            // Assert
            Assert.Null(resultado);
        }

        #endregion

        #region Repository Interaction Tests

        [Fact]
        public async Task Handle_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            var consulta = new BuscarClientePorIdQuery { Id = 42 };

            _repositorioMock
                .Setup(r => r.ObterPorIdAsync(42, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cliente?)null);

            // Act
            await _handler.Handle(consulta);

            // Assert
            _repositorioMock.Verify(
                r => r.ObterPorIdAsync(42, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryOnlyOnce()
        {
            // Arrange
            var consulta = new BuscarClientePorIdQuery { Id = 1 };
            var documento = NumeroDocumento.Create("529.982.247-25");
            var cliente = new Cliente("João Silva", documento);

            _repositorioMock
                .Setup(r => r.ObterPorIdAsync(consulta.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cliente);

            // Act
            await _handler.Handle(consulta);

            // Assert
            _repositorioMock.Verify(
                r => r.ObterPorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion

        #region DTO Mapping Tests

        [Fact]
        public async Task Handle_ShouldMapAllPropertiesCorrectly()
        {
            // Arrange
            var consulta = new BuscarClientePorIdQuery { Id = 1 };
            var documento = NumeroDocumento.Create("529.982.247-25");
            var cliente = new Cliente("Maria Santos", documento);

            _repositorioMock
                .Setup(r => r.ObterPorIdAsync(consulta.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cliente);

            // Act
            var resultado = await _handler.Handle(consulta);

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<BuscarClientePorIdResultadoDTO>(resultado);
            Assert.Equal(cliente.Id, resultado.Id);
            Assert.Equal(cliente.Nome, resultado.Name);
            Assert.Equal(cliente.NumeroDocumento.ToString(), resultado.Document);
            Assert.Equal(cliente.Ativo, resultado.Ativo);
        }

        #endregion
    }
}
