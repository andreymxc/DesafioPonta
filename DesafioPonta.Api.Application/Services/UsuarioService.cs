using AutoMapper;
using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Application.Dtos.Usuario;
using DesafioPonta.Api.Application.Dtos.Validations;
using DesafioPonta.Api.Application.Services.Interfaces;
using DesafioPonta.Api.Domain.Authentication;
using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace DesafioPonta.Api.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenGenerator;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper, ITokenHandler tokenGenerator, ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        public async Task<ResultService<dynamic>> GenerateToken(CreateUsuarioDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO is null)
                {
                    return ResultService.Fail<dynamic>("Objeto informado é nulo.");
                }

                var result = new CreateUsuarioDTOValidator().Validate(usuarioDTO);

                if (!result.IsValid)
                {
                    return ResultService.RequestError<dynamic>("Payload inválido.", result);
                }

                var usuario = await _usuarioRepository.GetUsuarioByEmailAndSenha(usuarioDTO.Email, usuarioDTO.Senha);

                if (usuario is null)
                {
                    return ResultService.Fail<dynamic>("Usuário e/ou senha inválidos");
                }

                var tokenPayload = _tokenGenerator.GenerateToken(usuario);

                return ResultService.Ok<dynamic>(tokenPayload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar token");
                return ResultService.InternalServerError<dynamic>("Erro interno na aplicação");
            }
        }

        public async Task<ResultService<UsuarioDTO>> CreateUsuario(CreateUsuarioDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO is null)
                {
                    return ResultService.Fail<UsuarioDTO>("Objeto informado é nulo.");
                }

                var result = new CreateUsuarioDTOValidator().Validate(usuarioDTO);

                if (!result.IsValid)
                {
                    return ResultService.RequestError<UsuarioDTO>("Payload inválido.", result);
                }

                var usuarioExistente = await _usuarioRepository.GetUsuarioByEmail(usuarioDTO.Email);

                if (usuarioExistente is not null)
                {
                    return ResultService.Fail<UsuarioDTO>("E-mail já cadastrado!");
                }

                var entityUsuario = _mapper.Map<Usuario>(usuarioDTO);

                var data = await _usuarioRepository.CreateAsync(entityUsuario);

                return ResultService.Ok<UsuarioDTO>(_mapper.Map<UsuarioDTO>(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário");
                return ResultService.InternalServerError<UsuarioDTO>("Erro interno na aplicação");
            }
        }
    }
}
