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
    }
}
