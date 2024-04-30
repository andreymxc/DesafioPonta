using AutoMapper;
using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Application.Dtos.Usuario;
using DesafioPonta.Api.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Application.Mappings
{
    public class DtoToDomainMapping : Profile
    {
        public DtoToDomainMapping()
        {
            CreateMap<TarefaDTO, Tarefa>();
            CreateMap<EditTarefaDTO, Tarefa>();
            CreateMap<CreateTarefaDTO, Tarefa>();
            CreateMap<CreateUsuarioDTO, Usuario>();
            CreateMap<UsuarioDTO, Usuario>();

        }
    }
}
