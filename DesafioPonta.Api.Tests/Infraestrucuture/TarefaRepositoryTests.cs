using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Infraestructure.Repositories;
using DesafioPonta.Api.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace DesafioPonta.Api.Tests.Infraestrucuture
{
    public class TarefaRepositoryTests
    {
        private readonly DbContextOptions<DataBaseContext> _options;

        public TarefaRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(databaseName: "DesafioPontaTesteTarefa")
                .Options;
        }

      
        [Fact]
        public async Task CreateAsync_Should_AddTarefaToDatabase()
        {
            // Arrange
            using (var context = new DataBaseContext(_options))
            {
                var repository = new TarefaRepository(context);
                var tarefa = new Tarefa(Guid.NewGuid(), "Tarefa 1", "Descrição 1", StatusTarefa.EmAndamento);

                // Act
                var result = await repository.CreateAsync(tarefa);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(tarefa, result);
            }
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowArgumentNullException_WhenCreatingNullTarefa()
        {
            // Arrange
            using (var context = new DataBaseContext(_options))
            {
                var repository = new TarefaRepository(context);

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.CreateAsync(null));
            }
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnAllActiveTarefas()
        {
            // Arrange
            using (var context = new DataBaseContext(_options))
            {
                var repository = new TarefaRepository(context);

                var tarefaAtiva = new Tarefa(Guid.NewGuid(), "Tarefa 1", "Descrição 1", StatusTarefa.EmAndamento);
                
                var tarefaInativa = new Tarefa(Guid.NewGuid(), "Tarefa 2", "Descrição 2", StatusTarefa.Pendente);
                tarefaInativa.Ativo = false;

                await repository.CreateAsync(tarefaAtiva);
                await repository.CreateAsync(tarefaInativa);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(0, result.Where(i => i.Ativo == false).Count());
            }
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnTarefa_IfExists()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            
            using (var context = new DataBaseContext(_options))
            {
                var repository = new TarefaRepository(context);
                var tarefa = new Tarefa(tarefaId, "Tarefa teste", "Descrição teste", StatusTarefa.Pendente);
                await repository.CreateAsync(tarefa);

                // Act
                var result = await repository.GetByIdAsync(tarefaId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(tarefa, result);
            }
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnNull_WhenTarefaNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            using (var context = new DataBaseContext(_options))
            {
                var repository = new TarefaRepository(context);

                // Act
                var result = await repository.GetByIdAsync(nonExistentId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetByStatusAsync_ShouldReturnTarefas_WithGivenStatus()
        {
            // Arrange
            using (var context = new DataBaseContext(_options))
            {
                var repository = new TarefaRepository(context);
                
                var tarefa1 = new Tarefa (Guid.NewGuid(),"Tarefa 1", "Descrição 1", StatusTarefa.Concluida);
                
                var tarefa2 = new Tarefa (Guid.NewGuid(), "Tarefa 2", "Descrição 2", StatusTarefa.Pendente);
                
                await repository.CreateAsync(tarefa1);
                
                await repository.CreateAsync(tarefa2);

                // Act
                var result = await repository.GetByStatusAsync(StatusTarefa.Concluida);

                // Assert
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Contains(tarefa1, result);
            }
        }

        [Fact]
        public async Task EditAsync_Should_UpdateTarefa_IfExists()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            using (var context = new DataBaseContext(_options))
            {
                var repository = new TarefaRepository(context);
                
                var originalTarefa = new Tarefa { Id = tarefaId, Titulo = "Tarefa Original", Descricao = "Descrição Original", Status = StatusTarefa.EmAndamento };
                
                await repository.CreateAsync(originalTarefa);
                
                var updatedTarefa = new Tarefa { Id = tarefaId, Titulo = "Tarefa Atualizada", Descricao = "Descrição Atualizada", Status = StatusTarefa.Concluida };

                // Act
                await repository.EditAsync(updatedTarefa);
                
                var tarefaInDatabase = await context.Tarefas.FindAsync(tarefaId);
                
                //Assert
                Assert.NotNull(tarefaInDatabase);
                Assert.Equal(updatedTarefa.Titulo, tarefaInDatabase.Titulo);
                Assert.Equal(updatedTarefa.Descricao, tarefaInDatabase.Descricao);
                Assert.Equal(updatedTarefa.Status, tarefaInDatabase.Status);
            }
        }

        [Fact]
        public async Task EditAsync_Should_ReturnNull_WhenTarefaNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            
            using (var context = new DataBaseContext(_options))
            {
                var repository = new TarefaRepository(context);
                var tarefa = new Tarefa { Id = nonExistentId, Titulo = "Tarefa Não Existente", Descricao = "Descrição Não Existente", Status = StatusTarefa.Pendente };

                // Act
                var result = await repository.EditAsync(tarefa);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteByIdAsync_Should_SetAtivoFalse_IfExists()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            
            using (var context = new DataBaseContext(_options))
            {
                var repository = new TarefaRepository(context);
                var tarefa = new Tarefa { Id = tarefaId, Titulo = "Tarefa 1", Descricao = "Descrição 1", Status = StatusTarefa.EmAndamento};
                await repository.CreateAsync(tarefa);

                // Act
                await repository.DeleteByIdAsync(tarefaId);

                // Assert
                var tarefaInDatabase = await context.Tarefas.FindAsync(tarefaId);
                Assert.False(tarefaInDatabase.Ativo);
            }
        }
    }
}
