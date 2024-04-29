using AutoMapper;
using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Application.Dtos.Usuario;
using DesafioPonta.Api.Domain.Models.Entities;

namespace DesafioPonta.Api.Application.Mappings
{
    public class DomainToDtoMapping : Profile
    {
        public DomainToDtoMapping()
        {

            CreateMap<Tarefa, CreateTarefaDTO>();
            CreateMap<Tarefa, TarefaDTO>();
            CreateMap<Usuario, CreateUsuarioDTO>();
            CreateMap<Usuario, UsuarioDTO>();

        }
    }
}
