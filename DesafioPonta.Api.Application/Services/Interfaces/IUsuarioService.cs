using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Application.Dtos.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResultService<UsuarioDTO>> CreateUsuario(CreateUsuarioDTO tarefaDTO);
        Task<ResultService<ICollection<UsuarioDTO>>> GetAllAsync();
        Task<ResultService<dynamic>> GenerateToken(CreateUsuarioDTO tarefaDTO);
    }
}
