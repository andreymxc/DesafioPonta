using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Domain.Validations;

namespace DesafioPonta.Api.Tests.Domain.Entities
{
    public class UsuarioTests
    {

        [Fact]
        public void CreateUsuario_ShouldThrowDomainException_WhenEmailIsNullOrEmpty()
        {
            // Arrange
            string? email = "";
            string? senha = "TesteSenha!123";
            StatusTarefa status = StatusTarefa.Pendente;

            var ex = Assert.Throws<DomainValidationException>(() => new Usuario(email, senha));

            Assert.Equal("Email deve ser informado", ex.Message);
        }


        [Fact]
        public void CreateUsuario_ShouldThrowDomainException_WhenSenhaIsNullOrEmpty()
        {
            // Arrange
            string? email = "teste@teste.com.br";
            string? senha = string.Empty;
            StatusTarefa status = StatusTarefa.Pendente;

            var ex = Assert.Throws<DomainValidationException>(() => new Usuario(email, senha));

            Assert.Equal("Senha deve ser informado", ex.Message);
        }


        [Fact]
        public void CreateUsuario_ShouldThrowDomainException_WhenIdIsEmpty()
        {
            // Arrange
            var senha = "teste@teste.com.br";
            var descricao = "TesteSenha123!";
            var id = Guid.Empty;

            var ex = Assert.Throws<DomainValidationException>(() => new Usuario(id, senha, descricao));

            Assert.Equal("Id deve ser informado", ex.Message);
        }

        [Fact]
        public void CreateUsuario_ShouldCreateSuccessfully_WhenAllParameterAreValid()
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
