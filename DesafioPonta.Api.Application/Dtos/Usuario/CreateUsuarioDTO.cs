using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Application.Dtos.Usuario
{
    public class CreateUsuarioDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
