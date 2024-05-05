using DesafioPonta.Api.Domain.Models.Enums;
using DesafioPonta.Api.Domain.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Domain.Models.Entities
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public Usuario()
        {

        }

        public Usuario(string email, string senha)
        {
            Validation(email, senha);
        }

        public Usuario(Guid id, string email, string senha)
        {
            DomainValidationException.When(id == Guid.Empty, "Id deve ser informado");

            Id = id;

            Validation(email, senha);
        }

        private void Validation(string email, string senha)
        {
            DomainValidationException.When(string.IsNullOrWhiteSpace(email), "Email deve ser informado");
            DomainValidationException.When(string.IsNullOrWhiteSpace(senha), "Senha deve ser informado");

            Email = email;
            Senha = senha;
          
        }
    }
}
