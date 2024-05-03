using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Application.Services;
using DesafioPonta.Api.Application.Services.Interfaces;
using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DesafioPonta.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : Controller
    {
        private readonly ITarefaService _tarefaService;

        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        /// <summary>
        /// Retorna todas as tarefas ativas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResultService<TarefaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> GetAllTarefas()
        {
            var result = await _tarefaService.GetAllAsync();         
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retorna os valores numéricos referentes aos status.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResultService<TarefaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultService<TarefaDTO>), StatusCodes.Status400BadRequest)]
        [Route("ConsultaTiposStatus")]
        public async Task<ActionResult> GetStatusTarefaEnumValues()
        {
            var result = await _tarefaService.GetEnumValues();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retorna as tarefas através do status. O valor informado é o valor numérico correspondente ao status. Consulte os valores no endpoint ConsultaTiposStatus.
        /// </summary>
        /// <param name="status"></param>
        [HttpGet]
        [ProducesResponseType(typeof(ResultService<TarefaDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultService<TarefaDTO>), StatusCodes.Status200OK)]
        [Route("{status}")]
        public async Task<ActionResult> GetByStatus(StatusTarefa status)
        {
            var result = await _tarefaService.GetByStatusAsync(status);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Criar uma nova tarefa.
        /// </summary>
        /// <param name="tarefa"></param>
        [HttpPost]
        [ProducesResponseType(typeof(ResultService<CreateTarefaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultService<CreateTarefaDTO>), StatusCodes.Status400BadRequest)]
        [Route("Criar")]
        public async Task<ActionResult> Create(CreateTarefaDTO tarefa)
        {
            string token = AuthorizationHelper.GetTokenFromHeader(HttpContext.Request);
            var result = await _tarefaService.CreateAsync(tarefa, token);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Edita uma tarefa existente.
        /// </summary>
        /// <param name="tarefa"></param>
        [HttpPut]
        [ProducesResponseType(typeof(ResultService<TarefaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultService<TarefaDTO>), StatusCodes.Status404NotFound)]
        [Route("Editar")]
        public async Task<ActionResult> Edit(EditTarefaDTO tarefa)
        {
            string token = AuthorizationHelper.GetTokenFromHeader(HttpContext.Request);
            var result = await _tarefaService.EditAsync(tarefa, token);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Este método exclui uma tarefa existente passando o id correspondente. A exclusão é feita de maneira lógica.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("Deletar")]
        public async Task<ActionResult> Delete(Guid id)
        {
            string token = AuthorizationHelper.GetTokenFromHeader(HttpContext.Request);
            var result = await _tarefaService.DeleteAsync(id, token);
            return StatusCode(result.StatusCode, result);
        }
    }

}
