using DesafioPonta.Api.Application.Dtos.Tarefa;
using DesafioPonta.Api.Domain.Models.Enums;
using FluentValidation;

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
             .Must(status => Enum.IsDefined(typeof(StatusTarefa), status))
             .WithMessage("Status inválido");
        }
    }
}
