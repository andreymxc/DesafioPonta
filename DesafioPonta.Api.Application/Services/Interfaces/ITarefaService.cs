using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Domain.Models.Enums;

namespace DesafioPonta.Api.Application.Services.Interfaces
{
    public interface ITarefaService
    {
        Task<ResultService<ICollection<TarefaDTO>>> GetAllAsync();
        Task<ResultService<TarefaDTO>> CreateAsync(CreateTarefaDTO tarefaDTO, string token);
        Task<ResultService<ICollection<TarefaDTO>>> GetByStatusAsync(StatusTarefa status);
        Task<ResultService<TarefaDTO>> EditAsync(TarefaDTO tarefaDTO, string token);
        Task<ResultService> DeleteAsync(Guid id, string token);
        Task<ResultService> GetEnumValues();
    }
}
