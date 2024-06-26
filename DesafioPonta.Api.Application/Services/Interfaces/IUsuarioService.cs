﻿using DesafioPonta.Api.Application.Dtos.Usuario;

namespace DesafioPonta.Api.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResultService<UsuarioDTO>> CreateUsuario(CreateUsuarioDTO tarefaDTO);
        Task<ResultService<dynamic>> GenerateToken(CreateUsuarioDTO tarefaDTO);
    }
}
