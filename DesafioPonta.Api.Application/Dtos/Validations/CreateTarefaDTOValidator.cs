using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Domain.Models.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Application.Dtos.Validations
{
    public class CreateTarefaDTOValidator : AbstractValidator<CreateTarefaDTO>
    {
        public CreateTarefaDTOValidator()
        {
            RuleFor(tarefa => tarefa.Titulo)
                .NotEmpty()
                .NotNull()
                .WithMessage("Titulo não deve ser vazio ou nulo");

            RuleFor(tarefa => tarefa.Descricao)
               .NotEmpty()
               .NotNull()
               .WithMessage("Descricao não deve ser vazio ou nulo");

            RuleFor(tarefa => tarefa.Status)
             .NotEmpty()
             .NotNull()
             .Must(status => Enum.IsDefined(typeof(StatusTarefa), status))
             .WithMessage("Status inválido");
        }
    }
}
