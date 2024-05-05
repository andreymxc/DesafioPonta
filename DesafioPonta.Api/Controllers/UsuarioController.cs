using DesafioPonta.Api.Application.Dtos.Usuario;
using DesafioPonta.Api.Application.Services;
using DesafioPonta.Api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DesafioPonta.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService; 

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Cadastro de usuário para obter acesso a aplicação.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultService<CreateUsuarioDTO>), StatusCodes.Status400BadRequest)]
        [Route("Cadastrar")]
        public async Task<ActionResult> Create(CreateUsuarioDTO usuario)
        {
            var result = await _usuarioService.CreateUsuario(usuario);
            
            if (result.IsSuccess)
                return Ok();

            return BadRequest(result);
        }

        /// <summary>
        /// Gera o token de acesso através das credenciais do usuário:  E-mail e Senha. Registre-se através do Endpoint 'Tarefa/Cadastrar'.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResultService<dynamic>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultService<dynamic>), StatusCodes.Status400BadRequest)]
        [Route("Token")]
        public async Task<ActionResult> Token([FromBody]CreateUsuarioDTO usuario) 
        {
            var result = await _usuarioService.GenerateToken(usuario);
            return StatusCode(result.StatusCode, result);
        }

    }
}
