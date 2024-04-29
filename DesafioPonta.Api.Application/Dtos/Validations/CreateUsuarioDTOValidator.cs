using DesafioPonta.Api.Application.Dtos.Usuario;
using DesafioPonta.Api.Domain.Models.Enums;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Application.Dtos.Validations
{
    public class CreateUsuarioDTOValidator : AbstractValidator<CreateUsuarioDTO>
    {
        public CreateUsuarioDTOValidator()
        {
            RuleFor(usuario => usuario.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("email não deve ser vazio ou nulo");

            RuleFor(p => p.Senha).NotEmpty().WithMessage("A senha não pode ser vazia")
                   .MinimumLength(8).WithMessage("A senha deve conter pelo menos 8 caracteres")
                   .MaximumLength(16).WithMessage("A senha não deve conter mais que 16 caracteres")
                   .Matches(@"[A-Z]+").WithMessage("A senha deve conter pelo menos uma letra maiúscula")
                   .Matches(@"[a-z]+").WithMessage("A senha deve conter pelo menos uma letra minúscula")
                   .Matches(@"[0-9]+").WithMessage("A senha deve conter pelo menos um número.")
                   .Matches(@"(?=.*\W)").WithMessage("A senha deve conter pelo menos 1 caractere especial.");
        }
    }
}
