using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Domain.Repositories
{
    public interface ITarefaRepository
    {
        Task<Tarefa> GetByIdAsync(Guid id);
        Task<ICollection<Tarefa>> GetAllAsync();
        Task<Tarefa> CreateAsync(Tarefa tarefa);
        Task<Tarefa> EditAsync(Tarefa tarefa);
        Task DeleteByIdAsync(Guid id);
        Task<ICollection<Tarefa?>> GetByStatusAsync(StatusTarefa status);
    }
}
