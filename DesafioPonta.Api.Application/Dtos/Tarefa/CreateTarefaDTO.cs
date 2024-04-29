using DesafioPonta.Api.Domain.Models.Enums;

namespace DesafioPonta.Api.Application.Dtos.Tarefa
{
    public class CreateTarefaDTO
    {

        /// <summary>
        /// Título da tarefa
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descrição da Tarefa
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Status da Tarefa. 0 = Pendente, 1 = EmAndamento, 2 = Finalizada.
        /// </summary>
        public StatusTarefa Status { get; set; }
    }
}
