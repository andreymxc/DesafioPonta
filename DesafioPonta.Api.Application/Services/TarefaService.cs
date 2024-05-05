using AutoMapper;
using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Application.Dtos.Validations;
using DesafioPonta.Api.Application.Services.Interfaces;
using DesafioPonta.Api.Domain.Authentication;
using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Domain.Repositories;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioPonta.Api.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenService;
        private readonly ILogger _logger;

        public TarefaService(ITarefaRepository tarefaRepository, ILogger<TarefaService> logger, IMapper mapper, ITokenHandler tokenService)
        {
            _tarefaRepository = tarefaRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<ResultService<TarefaDTO>> CreateAsync(CreateTarefaDTO tarefaDTO, string token)
        {
            try
            {
                if (tarefaDTO == null)
                    return ResultService.Fail<TarefaDTO>("Payload enviado é nulo.");

                var result = new CreateTarefaDTOValidator().Validate(tarefaDTO);

                if (!result.IsValid)
                {
                    return ResultService.RequestError<TarefaDTO>("Erros de validação do objeto.", result);
                }

                var entityTarefa = _mapper.Map<Tarefa>(tarefaDTO);

                entityTarefa.Id = new Guid();
                entityTarefa.UserId = Guid.Parse(_tokenService.GetUserIdFromToken(token));
                entityTarefa.Ativo = true;

                var data = await _tarefaRepository.CreateAsync(entityTarefa);

                return ResultService.Ok<TarefaDTO>(_mapper.Map<TarefaDTO>(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar tarefa");
                return ResultService.InternalServerError<TarefaDTO>("Erro interno na aplicação");
            }
        }

        public async Task<ResultService<TarefaDTO>> EditAsync(EditTarefaDTO tarefaDTO, string token)
        {
            try
            {
                var result = new EditTarefaDTOValidator().Validate(tarefaDTO);

                if (!result.IsValid)
                {
                    return ResultService.RequestError<TarefaDTO>("Erros de validação do objeto.", result);
                }

                var existingTarefa = await _tarefaRepository.GetByIdAsync(tarefaDTO.Id);

                if (existingTarefa is null)
                    return ResultService.NotFound<TarefaDTO>("Tarefa não encontrada");

                if (!_tokenService.CheckIfCreatedByUser(existingTarefa.UserId, token))
                    return ResultService.Unauthorized<TarefaDTO>("Usuário não tem permissão para realizar essa ação");

                var entityTarefa = _mapper.Map<Tarefa>(tarefaDTO);

                var data = await _tarefaRepository.EditAsync(entityTarefa);

                if (data is null)
                {
                    return ResultService.Fail<TarefaDTO>("Erro ao editar tarefa");
                }

                return ResultService.Ok(_mapper.Map<TarefaDTO>(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar tarefa");
                return ResultService.InternalServerError<TarefaDTO>("Erro interno na aplicação");
            }
        }

        public async Task<ResultService> DeleteAsync(Guid id, string token)
        {
            try
            {
                Tarefa tarefa = await _tarefaRepository.GetByIdAsync(id);

                if (tarefa is null || tarefa?.Ativo == false)
                    return ResultService.NotFound("Tarefa não encontrada");

                if (!_tokenService.CheckIfCreatedByUser(tarefa.UserId, token))
                    return ResultService.Unauthorized("Usuário não tem permissão para realizar essa ação");

                await _tarefaRepository.DeleteByIdAsync(id);

                return ResultService.Ok("Tarefa excluída com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar tarefa");
                return ResultService.InternalServerError("Erro interno na aplicação");
            }

        }

        public async Task<ResultService<ICollection<TarefaDTO>>> GetAllAsync()
        {
            try
            {
                var tarefas = await _tarefaRepository.GetAllAsync();
                var tarefasDto = _mapper.Map<ICollection<TarefaDTO>>(tarefas);

                if (!tarefasDto.Any())
                    return ResultService.NoContent(tarefasDto);

                return ResultService.Ok(tarefasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar tarefa");
                return ResultService.InternalServerError<ICollection<TarefaDTO>>("Erro interno na aplicação");
            }
        }

        public async Task<ResultService<ICollection<TarefaDTO>>> GetByStatusAsync(StatusTarefa status)
        {
            try
            {
                var tarefas = await _tarefaRepository.GetByStatusAsync(status);

                var tarefasDto = _mapper.Map<ICollection<TarefaDTO>>(tarefas);
          
                return ResultService.Ok(tarefasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar tarefa por status");
                return ResultService.InternalServerError<ICollection<TarefaDTO>>("Erro interno na aplicação");
            }
        }

        public async Task<ResultService> GetEnumValues()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(StatusTarefa)).Cast<StatusTarefa>();

                var enumData = enumValues.Select(e => new
                {
                    Valor = (int)e,
                    Descricao = e.ToString()
                });

                return ResultService.Ok(enumData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar valores numérios dos status");
                return ResultService.InternalServerError("Erro interno na aplicação");
            }
        }
    }
}

