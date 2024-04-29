using DesafioPonta.Api.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Domain.Authentication
{
    public interface ITokenGenerator
    {
        dynamic GenerateToken(Usuario? usuario);
        string GetUserIdFromToken(string token);

    }
}
