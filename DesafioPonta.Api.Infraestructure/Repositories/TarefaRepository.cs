﻿using DesafioPonta.Api.Domain.Models.Entities;
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

        public async Task<ICollection<Tarefa>> GetByStatusAsync(StatusTarefa status)
        {
            return await _dbContext.Tarefas.Where(i => i.Status == status && i.Ativo).ToListAsync();
        }

        public async Task<Tarefa?> EditAsync(Tarefa tarefa)
        {
            var existingTarefa = await _dbContext.Tarefas.FirstOrDefaultAsync(i=>i.Id == tarefa.Id);

            if (existingTarefa != null)
            {
                existingTarefa.Titulo = tarefa.Titulo;
                existingTarefa.Descricao = tarefa.Descricao;
                existingTarefa.Status = tarefa.Status;

                _dbContext.Update(existingTarefa);

                await _dbContext.SaveChangesAsync();
            }

            return existingTarefa;
        }


        public async Task DeleteByIdAsync(Guid id)
        {
            var tarefa = await _dbContext.Tarefas.FirstOrDefaultAsync(i => i.Id == id);
            
            tarefa.Ativo = false;
            
            await _dbContext.SaveChangesAsync();
        }
    }
}
