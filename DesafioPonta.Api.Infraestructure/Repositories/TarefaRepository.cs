using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DesafioPonta.Api.Infraestructure.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly DataBaseContext _dbContext;

        public TarefaRepository(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }       
        public async Task<Tarefa> CreateAsync(Tarefa tarefa)
        {            
            _dbContext.Add(tarefa);
            await _dbContext.SaveChangesAsync();
            return tarefa;
        }

        public async Task<ICollection<Tarefa>> GetAllAsync()
        {
            return await _dbContext.Tarefas.Where(i=>i.Ativo).ToListAsync();
        }

        public async Task<Tarefa?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Tarefas.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ICollection<Tarefa?>> GetByStatusAsync(StatusTarefa status)
        {
            return await _dbContext.Tarefas.Where(i => i.Status == status).ToListAsync();
        }

        public async Task<Tarefa?> EditAsync(Tarefa editTarefa)
        {
            var existingTarefa = await _dbContext.Tarefas.FindAsync(editTarefa.Id);

            if (existingTarefa != null)
            {
                _dbContext.Entry(existingTarefa).State = EntityState.Detached;
            }

            editTarefa.CriadoEm = existingTarefa.CriadoEm;            
            
            _dbContext.Update(editTarefa);
            
            await _dbContext.SaveChangesAsync();
            
            return editTarefa;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var tarefa = await _dbContext.Tarefas.FirstOrDefaultAsync(i => i.Id == id);
            
            tarefa.Ativo = false;
            
            await _dbContext.SaveChangesAsync();
        }
    }
}
