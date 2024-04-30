using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Tests.Domain.Entities
{
    public class TarefaTest
    {
        [Fact]
        public void CreateTarefa_ShouldThrowDomainException_When_TituloIsNullOrEmpty()
        {
            // Arrange
            string? titulo = "";
            string? descricao = "Descrição válida";
            StatusTarefa status = StatusTarefa.Pendente;

            var ex = Assert.Throws<DomainValidationException>(() => new Tarefa(titulo, descricao, StatusTarefa.Pendente));

            Assert.Equal("Título deve ser informado", ex.Message);
        }


        [Fact]
        public void CreateTarefa_ShouldThrowDomainException_WhenDescricaoIsNullOrEmpty()
        {
            // Arrange
            string? titulo = "Título válido";
            string? invalidDescricao = null;
            StatusTarefa status = StatusTarefa.Pendente;

            var ex = Assert.Throws<DomainValidationException>(() => new Tarefa(titulo, invalidDescricao, StatusTarefa.Concluida));

            Assert.Equal("Descrição deve ser informada", ex.Message);
        }


        [Fact]
        public void CreateTarefa_ShouldThrowDomainException__WhenStatusIsInvalid()
        {
            // Arrange
            var titulo = "Titulo de teste";
            var descricao = "Descrição de teste";
            var invalidStatus = (StatusTarefa)5; // Status inexistente

            var ex = Assert.Throws<DomainValidationException>(() => new Tarefa(titulo,descricao, invalidStatus));

            Assert.Equal("Status da tarefa é inválido", ex.Message);
        }

        [Fact]
        public void CreateTarefa_ShouldThrowDomainException__WhenIdIsEmpty()
        {
            // Arrange
            var emptyId = Guid.Empty;
            var titulo = "Titulo de teste";
            var descricao = "Descrição de teste";
            var status = StatusTarefa.EmAndamento; 

            // Act & Assert
            var ex = Assert.Throws<DomainValidationException>(() => new Tarefa(emptyId, titulo, descricao, status));
            Assert.Equal("Id deve ser informado", ex.Message);
        }

        [Fact]
        public void CreateTarefa_ShouldCreateSuccessfully_WhenAllParameterAreValid()
        {
            // Arrange
            var titulo = "Titulo de teste";
            var descricao = "Descrição de teste";
            var status = StatusTarefa.EmAndamento;

            // Act
            var tarefa = new Tarefa(titulo, descricao, status);

            // Assert
            Assert.Equal(titulo, tarefa.Titulo);
            Assert.Equal(descricao, tarefa.Descricao);
            Assert.Equal(status, tarefa.Status);
        }

        [Fact]
        public void ShouldCreateTarefaSuccessfully_WhenIdIsValid()
        {
            // Arrange
            var id = Guid.NewGuid(); 
            var titulo = "Titulo de teste";
            var descricao = "Descrição de teste";
            var status = StatusTarefa.EmAndamento; 

            // Act
            var tarefa = new Tarefa(id, titulo, descricao, status);

            // Assert
            Assert.Equal(id, tarefa.Id);
            Assert.Equal(titulo, tarefa.Titulo);
            Assert.Equal(descricao, tarefa.Descricao);
            Assert.Equal(status, tarefa.Status);
        }
    }
}
