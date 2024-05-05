using DesafioPonta.Api.Domain.Models.Enums;

namespace DesafioPonta.Api.Application.Dtos.Tarefa
{
    public class EditTarefaDTO
    {
        /// <summary>
        /// Identificador único da tarefa gerada
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Título da tarefa
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// Descrição da tarefa
        /// </summary>
        public string Descricao { get; set; }
        /// <summary>
        /// Status da Tarefa. 0 = Pendente, 1 = EmAndamento, 2 = Finalizada.
        /// </summary>
        public StatusTarefa Status { get; set; }
    }
}
