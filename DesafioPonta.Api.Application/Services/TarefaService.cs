using AutoMapper;
using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Application.Dtos.Validations;
using DesafioPonta.Api.Application.Services.Interfaces;
using DesafioPonta.Api.Domain.Authentication;
using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace DesafioPonta.Api.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IMapper _mapper;
        private readonly ITokenGenerator _tokenHandler;
        private readonly ILogger _logger;

        public TarefaService(ITarefaRepository tarefaRepository, ILogger<TarefaService> logger, IMapper mapper, ITokenGenerator tokenHandler)
        {
            _tarefaRepository = tarefaRepository;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
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
                entityTarefa.UserId = Guid.Parse(_tokenHandler.GetUserIdFromToken(token));
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
                var existingTarefa = await _tarefaRepository.GetByIdAsync(tarefaDTO.Id);

                if (existingTarefa is null)
                    return ResultService.NotFound<TarefaDTO>("Tarefa não encontrada");

                if (!VerificaUsuarioCriador(existingTarefa.UserId, token))
                    return ResultService.Forbidden<TarefaDTO>("Usuário não tem permissão para realizar essa ação");

                var entityTarefa = _mapper.Map<Tarefa>(tarefaDTO);

                entityTarefa.UserId = existingTarefa.UserId;
                var data = await _tarefaRepository.EditAsync(entityTarefa);

                return ResultService.Ok(_mapper.Map<TarefaDTO>(data));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar tarefa");
                return ResultService.InternalServerError<TarefaDTO>("Erro interno na aplicação");
            }        
        }

        public async Task<ResultService> DeleteAsync(Guid id, string token)
        {
            Tarefa tarefa = await _tarefaRepository.GetByIdAsync(id);

            if (tarefa is null || tarefa?.Ativo == false)
                return ResultService.NotFound("Tarefa não encontrada");

            if(!VerificaUsuarioCriador(tarefa.Id, token))
                return ResultService.Forbidden<TarefaDTO>("Usuário não tem permissão para realizar essa ação");

            await _tarefaRepository.DeleteByIdAsync(id);

            return ResultService.Ok("Tarefa excluída com sucesso");
        }

        public async Task<ResultService<ICollection<TarefaDTO>>> GetAllAsync()
        {
            var tarefas = await _tarefaRepository.GetAllAsync();

            if (tarefas.Any())
            {
                var tarefasDto = _mapper.Map<ICollection<TarefaDTO>>(tarefas);

                return ResultService.Ok(tarefasDto);
            }

            return ResultService.Fail<ICollection<TarefaDTO>>("Sem tarefas cadastradas");
        }

        public async Task<ResultService<ICollection<TarefaDTO>>> GetByStatusAsync(StatusTarefa status)
        {
            var tarefas = await _tarefaRepository.GetByStatusAsync(status);

            if (tarefas.Any())
            {
                var tarefasDto = _mapper.Map<ICollection<TarefaDTO>>(tarefas);

                return ResultService.Ok(tarefasDto);
            }

            return ResultService.NotFound<ICollection<TarefaDTO>>("Sem tarefas cadastradas");

        }

        public async Task<ResultService> GetEnumValues()
        {
            var enumValues = Enum.GetValues(typeof(StatusTarefa)).Cast<StatusTarefa>();

            var enumData = enumValues.Select(e => new
            {
                Valor = (int)e,
                Descricao = e.ToString()
            });

            return ResultService.Ok(enumData);
        }

        /// <summary>
        /// Verifica se o id do criador da tarefa é o mesmo contido na claim 'Id' do Jwt
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool VerificaUsuarioCriador(Guid userId, string token)
        {
            string userIdFromToken = _tokenHandler.GetUserIdFromToken(token);
            return userIdFromToken == userId.ToString();
        }
    }
}

