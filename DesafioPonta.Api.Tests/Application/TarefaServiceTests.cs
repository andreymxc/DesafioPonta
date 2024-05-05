using AutoMapper;
using Castle.Core.Logging;
using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Application.Services;
using DesafioPonta.Api.Domain.Authentication;
using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.ObjectModel;

namespace DesafioPonta.Api.Tests.Application
{
    public class TarefaServiceTests
    {
        private Mock<ITarefaRepository> _tarefaRepository;
        private Mock<ILogger<TarefaService>> _logger;
        private Mock<ITokenHandler> _tokenGenerator;
        private TarefaService _tarefaService;
        private IMapper _mapper = new BaseTest().GetMapperProfile();
        private Mock<IMapper> _mapperMock; 
        private string _jwtToken;

        public TarefaServiceTests()
        {
            _tarefaRepository = new Mock<ITarefaRepository>();
            _logger = new Mock<ILogger<TarefaService>>();
            _tokenGenerator = new Mock<ITokenHandler>();
            _mapperMock = new Mock<IMapper>();

            _tarefaService = new TarefaService(
                    _tarefaRepository.Object,
                    _logger.Object,
                    _mapper,
                    _tokenGenerator.Object
            );

            _jwtToken = "5Bc1pKOJY5MlqC6VD6vhzDmMtspe5O1xMSmmqy3mKluHOZqKqXCY5y7DVaDBK1ds";
        }

        [Fact]
        public async Task CreateAsync_WhenAllParametersAreValid_ReturnsSuccessResult()
        {
            var dto = new CreateTarefaDTO()
            {
                Titulo = "Titulo de teste",
                Descricao = "Descrição de teste",
                Status = StatusTarefa.EmAndamento
            };

            _tokenGenerator.Setup(x =>
                                  x.GetUserIdFromToken(It.IsAny<string>()))
                                  .Returns(Guid.NewGuid().ToString());

            _tarefaRepository.Setup(x =>
                                    x.CreateAsync(It.IsAny<Tarefa>()))
                                    .ReturnsAsync(It.IsAny<Tarefa>());
           
            var result = await _tarefaService.CreateAsync(dto, _jwtToken);
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WhenDtoIsNull_ReturnsFailResult()
        {
            // Arrange
            CreateTarefaDTO dto = null;

            var token = Guid.NewGuid().ToString();

            // Act
            var result = await _tarefaService.CreateAsync(dto, token);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Payload enviado é nulo.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task CreateAsync_WhenDtoTituloIsNullOrEmpty_ReturnsRequestErrorResult()
        {
            // Arrange
            var invalidDto = new CreateTarefaDTO()
            {
                Titulo = null,
                Descricao = "Descrição de teste",
                Status = StatusTarefa.EmAndamento
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(CreateTarefaDTO.Titulo), "Titulo não deve ser vazio ou nulo"),
            });

            var validatorMock = new Mock<IValidator<CreateTarefaDTO>>();
            validatorMock.Setup(x => x.Validate(invalidDto))
                         .Returns(validationResult);

            // Act
            var result = await _tarefaService.CreateAsync(invalidDto, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Erros de validação do objeto.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task CreateAsync_WhenDtoDescricaoIsNullOrEmpty_ReturnsRequestErrorResult()
        {
            // Arrange
            var invalidDto = new CreateTarefaDTO()
            {
                Titulo = "Descrição de teste",
                Descricao = string.Empty,
                Status = StatusTarefa.EmAndamento
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(CreateTarefaDTO.Descricao), "Descrição não deve ser vazio ou nulo"),
            });

            var validatorMock = new Mock<IValidator<CreateTarefaDTO>>();

            validatorMock.Setup(x => x.Validate(invalidDto))
                         .Returns(validationResult);

            // Act
            var result = await _tarefaService.CreateAsync(invalidDto, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Erros de validação do objeto.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task CreateAsync_WhenDtoSituacaoIsInvalid_ReturnsRequestErrorResult()
        {
            // Arrange
            var invalidDto = new CreateTarefaDTO()
            {
                Titulo = "Descrição de teste",
                Descricao = "Descricao de teste",
                Status = (StatusTarefa)4 // Status inválido
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(CreateTarefaDTO.Descricao), "Descrição não deve ser vazio ou nulo"),
            });

            var validatorMock = new Mock<IValidator<CreateTarefaDTO>>();
            validatorMock.Setup(x => x.Validate(invalidDto))
                         .Returns(validationResult);

            // Act
            var result = await _tarefaService.CreateAsync(invalidDto, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Erros de validação do objeto.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task CreateAsync_WhenTokenHandlerReturnsNull_ReturnsInternalServerErrorResult()
        {
            // Arrange
            var dto = new CreateTarefaDTO()
            {
                Titulo = "Título de teste",
                Descricao = "Descricao de teste",
                Status = StatusTarefa.Pendente
            };

            _tokenGenerator.Setup(x => x.GetUserIdFromToken(_jwtToken))
                           .Returns((string)null); // Mockando a extração do Id da claim do JWT para falhar
            // Act
            var result = await _tarefaService.CreateAsync(dto, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Erro interno na aplicação", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetAllAsync_WhenTasksExist_ReturnsOkResultWithTasks()
        {
            // Arrange
            var tarefas = new Collection<Tarefa>
                {
            new Tarefa { Id = Guid.NewGuid(), Titulo = "Tarefa 1", Descricao = "Descricao 1", Status = StatusTarefa.Pendente},
            new Tarefa { Id = Guid.NewGuid(), Titulo = "Tarefa 2",Descricao = "Descricao 2", Status = StatusTarefa.Pendente}
            };

            _tarefaRepository.Setup(repo => repo.GetAllAsync())
                             .ReturnsAsync(tarefas);

            var tarefasDto = new Collection<TarefaDTO>
            {
                new TarefaDTO { Id = tarefas[0].Id, Titulo = tarefas[0].Titulo, Descricao = tarefas[0].Descricao, Status = tarefas[0].Status},
                new TarefaDTO { Id = tarefas[1].Id, Titulo = tarefas[1].Titulo, Descricao = tarefas[1].Descricao, Status = tarefas[1].Status }
            };

            _mapperMock.Setup(mapper => mapper.Map<ICollection<TarefaDTO>>(tarefas))
           .Returns(tarefasDto);

            // Act
            var result = await _tarefaService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Data);
        }


        [Fact]
        public async Task EditAsync_WhenValidInput_ReturnsOkResult()
        {
            // Arrange
            var validDto = new EditTarefaDTO
            {
                Id = Guid.NewGuid(),
                Titulo = "Título de teste",
                Descricao = "Descricao de teste",
                Status = StatusTarefa.Pendente,
            };

            var validatorMock = new Mock<IValidator<EditTarefaDTO>>();
            validatorMock.Setup(x => x.Validate(validDto)).Returns(new ValidationResult());

            _tarefaRepository.Setup(repo => repo.GetByIdAsync(validDto.Id))
                            .ReturnsAsync(new Tarefa());

            // Usuário é o criador da tarefa
            _tokenGenerator.Setup(x => x.CheckIfCreatedByUser(new Guid(), _jwtToken)).Returns(true);

            _tarefaRepository.Setup(repo => repo.EditAsync(It.IsAny<Tarefa>()))
                            .ReturnsAsync(new Tarefa());

            // Act
            var result = await _tarefaService.EditAsync(validDto, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task EditAsync_WhenLoggedUserIsNotTheCreator_ReturnsFail()
        {
            // Arrange
            var validDto = new EditTarefaDTO
            {
                Id = Guid.NewGuid(),
                Titulo = "Título de teste",
                Descricao = "Descricao de teste",
                Status = StatusTarefa.Pendente,
            };

            var validatorMock = new Mock<IValidator<EditTarefaDTO>>();
            validatorMock.Setup(x => x.Validate(validDto)).Returns(new ValidationResult());

            _tarefaRepository.Setup(repo => repo.GetByIdAsync(validDto.Id))
                            .ReturnsAsync(new Tarefa());

            // Usuário não é o criador da tarefa
            _tokenGenerator.Setup(x => x.CheckIfCreatedByUser(new Guid(), _jwtToken)).Returns(false);

            _tarefaRepository.Setup(repo => repo.EditAsync(It.IsAny<Tarefa>()))
                            .ReturnsAsync(new Tarefa());

            // Act
            var result = await _tarefaService.EditAsync(validDto, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Usuário não tem permissão para realizar essa ação", result.Message);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task EditAsync_WhenIdDoesNotExistOnDatabase_ReturnsFail()
        {
            // Arrange
            var validDto = new EditTarefaDTO
            {
                Id = Guid.NewGuid(),
                Titulo = "Título de teste",
                Descricao = "Descricao de teste",
                Status = StatusTarefa.Pendente,
            };

            var validatorMock = new Mock<IValidator<EditTarefaDTO>>();
            validatorMock.Setup(x => x.Validate(validDto)).Returns(new ValidationResult());

            _tarefaRepository.Setup(repo => repo.GetByIdAsync(validDto.Id))
                            .ReturnsAsync((Tarefa)null);

            // Act
            var result = await _tarefaService.EditAsync(validDto, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tarefa não encontrada", result.Message);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteAsync_WhenExistingAndActiveTask_DeletesSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var tarefa = new Tarefa { Id = id, Ativo = true };

            _tarefaRepository.Setup(repo => repo.GetByIdAsync(id))
                             .ReturnsAsync(tarefa);

            _tokenGenerator.Setup(service => service.CheckIfCreatedByUser(tarefa.UserId, _jwtToken))
                         .Returns(true);

            // Act
            var result = await _tarefaService.DeleteAsync(id, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Tarefa excluída com sucesso", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenTarefaIsInactive_ReturnsFail()
        {
            // Arrange
            var id = Guid.NewGuid();
            var tarefa = new Tarefa { Id = id, Ativo = false };

            _tarefaRepository.Setup(repo => repo.GetByIdAsync(id))
                             .ReturnsAsync(tarefa);
            // Act
            var result = await _tarefaService.DeleteAsync(id, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Tarefa não encontrada", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenLoggedUserIsNotTheCreator_ReturnsFail()
        {
            // Arrange
            var id = Guid.NewGuid();
            var tarefa = new Tarefa { Id = id, Ativo = true };

            _tarefaRepository.Setup(repo => repo.GetByIdAsync(id))
                             .ReturnsAsync(tarefa);

            _tokenGenerator.Setup(service => service.CheckIfCreatedByUser(tarefa.UserId, _jwtToken))
                         .Returns(false);

            // Act
            var result = await _tarefaService.DeleteAsync(id, _jwtToken);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Usuário não tem permissão para realizar essa ação", result.Message);
        }
    }

}
