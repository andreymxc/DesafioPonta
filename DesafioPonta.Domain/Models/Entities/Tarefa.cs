using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace DesafioPonta.Api.Domain.Models.Entities
{
    public class Tarefa
    {
        [Key]
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime CriadoEm { get;  set; }
        public StatusTarefa Status { get; set; }
        public Guid UserId { get; set; }
        public bool Ativo { get; set; }

        public Tarefa()
        {

        }

        public Tarefa(string titulo, string descricao, StatusTarefa status)
        {
            Validation(titulo, descricao, status);  
        }

        public Tarefa(Guid id, string titulo, string descricao, StatusTarefa status)
        {
            DomainValidationException.When(id == Guid.Empty, "Id deve ser informado");

            Id = id;

            Validation(titulo, descricao, status);
        }

        private void Validation(string titulo, string descricao, StatusTarefa status)
        {
            DomainValidationException.When(string.IsNullOrWhiteSpace(titulo), "Título deve ser informado");
            DomainValidationException.When(string.IsNullOrWhiteSpace(descricao), "Descrição deve ser informada");
            DomainValidationException.When(!Enum.IsDefined(typeof(StatusTarefa), status), "Status da tarefa é inválido");

            Titulo = titulo;
            Descricao = descricao;
            Status = status;
            CriadoEm = DateTime.Now;
            Ativo = true;
        }
    }
}
