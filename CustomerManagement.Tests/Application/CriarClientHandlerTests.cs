using CustomerManagement.Application.Customer.Commands;
using CustomerManagement.Application.Customer.Handlers;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;
using Moq;

namespace CustomerManagement.Tests.Application
{
    public class CriarClientHandlerTests
    {
        private readonly Mock<IClienteRepository> _repositorioMock;
        private readonly CriarClienteCommandHandler _handler;

        public CriarClientHandlerTests()
        {
            _repositorioMock = new Mock<IClienteRepository>();
            _handler = new CriarClienteCommandHandler(_repositorioMock.Object);
        }

        #region Casos de Sucesso

        [Fact]
        public async Task Handle_ComCPFValido_DeveRetornarSucesso()
        {
            // Arrange
            var comando = new CriarClienteCommand
            {
                Nome = "João Silva",
                NumeroDocumento = "529.982.247-25"
            };

            _repositorioMock
                .Setup(r => r.ExisteNumeroDocumentoAsync(It.IsAny<NumeroDocumento>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositorioMock
                .Setup(r => r.CriarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _handler.Handle(comando);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal("Cadastro realizado com sucesso!", resultado.Mensagem);
        }

        [Fact]
        public async Task Handle_ComCNPJValido_DeveRetornarSucesso()
        {
            // Arrange
            var comando = new CriarClienteCommand
            {
                Nome = "Empresa Teste LTDA",
                NumeroDocumento = "11.444.777/0001-61"
            };

            _repositorioMock
                .Setup(r => r.ExisteNumeroDocumentoAsync(It.IsAny<NumeroDocumento>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositorioMock
                .Setup(r => r.CriarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _handler.Handle(comando);

            // Assert
            Assert.True(resultado.Sucesso);
        }

        #endregion

        #region Casos de Documento Duplicado

        [Fact]
        public async Task Handle_ComDocumentoDuplicado_DeveRetornarFalha()
        {
            // Arrange
            var comando = new CriarClienteCommand
            {
                Nome = "João Silva",
                NumeroDocumento = "529.982.247-25"
            };

            _repositorioMock
                .Setup(r => r.ExisteNumeroDocumentoAsync(It.IsAny<NumeroDocumento>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _handler.Handle(comando);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Documento já cadastrado.", resultado.Mensagem);
        }

        [Fact]
        public async Task Handle_ComDocumentoDuplicado_NaoDeveChamarCreateAsync()
        {
            // Arrange
            var comando = new CriarClienteCommand
            {
                Nome = "João Silva",
                NumeroDocumento = "529.982.247-25"
            };

            _repositorioMock
                .Setup(r => r.ExisteNumeroDocumentoAsync(It.IsAny<NumeroDocumento>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(comando);

            // Assert
            _repositorioMock.Verify(
                r => r.CriarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        #endregion

        #region Casos de Documento Inválido

        [Theory]
        [InlineData("111.111.111-11")]
        [InlineData("123.456.789-00")]
        [InlineData("11.111.111/1111-11")]
        public async Task Handle_ComDocumentoInvalido_DeveRetornarFalha(string documentoInvalido)
        {
            // Arrange
            var comando = new CriarClienteCommand
            {
                Nome = "João Silva",
                NumeroDocumento = documentoInvalido
            };

            // Act
            var resultado = await _handler.Handle(comando);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains("inválido", resultado.Mensagem);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Handle_ComDocumentoVazio_DeveRetornarFalha(string documentoVazio)
        {
            // Arrange
            var comando = new CriarClienteCommand
            {
                Nome = "João Silva",
                NumeroDocumento = documentoVazio
            };

            // Act
            var resultado = await _handler.Handle(comando);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Documento não pode ser vazio.", resultado.Mensagem);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("12345678")]
        [InlineData("123456789012345")]
        public async Task Handle_ComQuantidadeInvalidaDeDigitos_DeveRetornarFalha(string documentoInvalido)
        {
            // Arrange
            var comando = new CriarClienteCommand
            {
                Nome = "João Silva",
                NumeroDocumento = documentoInvalido
            };

            // Act
            var resultado = await _handler.Handle(comando);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Documento com quantidade de dígitos inválida.", resultado.Mensagem);
        }

        #endregion

        #region Casos de Nome Inválido

        [Fact]
        public async Task Handle_ComNomeVazio_DeveRetornarFalha()
        {
            // Arrange
            var comando = new CriarClienteCommand
            {
                Nome = "",
                NumeroDocumento = "529.982.247-25"
            };

            _repositorioMock
                .Setup(r => r.ExisteNumeroDocumentoAsync(It.IsAny<NumeroDocumento>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await _handler.Handle(comando);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Nome não pode ser vazio.", resultado.Mensagem);
        }

        [Fact]
        public async Task Handle_ComNomeMuitoCurto_DeveRetornarFalha()
        {
            // Arrange
            var comando = new CriarClienteCommand
            {
                Nome = "A",
                NumeroDocumento = "529.982.247-25"
            };

            _repositorioMock
                .Setup(r => r.ExisteNumeroDocumentoAsync(It.IsAny<NumeroDocumento>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await _handler.Handle(comando);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Nome deve ter pelo menos 2 caracteres.", resultado.Mensagem);
        }

        #endregion
    }
}
