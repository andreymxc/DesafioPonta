using AutoMapper;
using DesafioPonta.Api.Application.Dtos.Usuario;
using DesafioPonta.Api.Application.Services;
using DesafioPonta.Api.Domain.Authentication;
using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace DesafioPonta.Api.Tests.Application
{
    public  class UsuarioServiceTests
    {

        private Mock<IUsuarioRepository> _usuarioRepository;
        private Mock<ILogger<UsuarioService>> _logger;
        private Mock<ITokenHandler> _tokenGenerator;
        private UsuarioService _usuarioService;
        private IMapper _mapper = new BaseTest().GetMapperProfile();
        private Mock<IMapper> _mapperMock;

        public UsuarioServiceTests()
        {
            _usuarioRepository = new Mock<IUsuarioRepository>();
            _logger = new Mock<ILogger<UsuarioService>>();
            _tokenGenerator = new Mock<ITokenHandler>();
            _mapperMock = new Mock<IMapper>();

            _usuarioService = new UsuarioService(
                    _usuarioRepository.Object,
                     _mapper,
                    _tokenGenerator.Object,
                    _logger.Object
            );
        }

        [Fact]
        public async Task CreateUsuario_NullUsuarioDTO_ReturnsFailResult()
        {

            // Act
            var result = await _usuarioService.CreateUsuario(null);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Objeto informado é nulo.", result.Message);
        }

        [Fact]
        public async Task CreateUsuario_InvalidUsuarioDTO_ReturnsRequestErrorResult()
        {
            // Arrange
            var invalidUsuarioDTO = new CreateUsuarioDTO()
            {
                Email = "",
                Senha = "Teste!1234abc"
            };

            var validationResult = new ValidationResult(new[] { new ValidationFailure("Email", "Erro de e-mail") });
            
            var validatorMock = new Mock<IValidator<CreateUsuarioDTO>>();
            
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateUsuarioDTO>>(), default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _usuarioService.CreateUsuario(invalidUsuarioDTO);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Payload inválido.", result.Message);
        }

        [Fact]
        public async Task CreateUsuario_ExistingEmail_ReturnsFailResult()
        {
            // Arrange
            string emailExistente = "usuarioTeste@teste.com";

            _usuarioRepository.Setup(repo => repo.GetUsuarioByEmail(emailExistente))
                .ReturnsAsync(new Usuario() { Email = emailExistente });

            var usuarioDTO = new CreateUsuarioDTO { Email = emailExistente, Senha = "Teste123@!" };

            var validatorMock = new Mock<IValidator<CreateUsuarioDTO>>();

            var validationResult = new ValidationResult();
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateUsuarioDTO>>(), default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _usuarioService.CreateUsuario(usuarioDTO);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("E-mail já cadastrado!", result.Message);

            validatorMock.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateUsuarioDTO>>(), default), Times.Never);
        }

        [Fact]
        public async Task CreateUsuario_ValidUsuarioDTO_ReturnsOkResult()
        {
            // Arrange
            var usuarioDTO = new CreateUsuarioDTO { Email = "teste@teste.com.br", Senha = "Teste1234!@" };
            _usuarioRepository.Setup(repo => repo.GetUsuarioByEmail(usuarioDTO.Email))
                .ReturnsAsync((Usuario)null);
            
            
            _usuarioRepository.Setup(repo => repo.CreateAsync(It.IsAny<Usuario>()))
                .ReturnsAsync(new Usuario()); 

            // Act
            var result = await _usuarioService.CreateUsuario(usuarioDTO);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
        }
    }
}
