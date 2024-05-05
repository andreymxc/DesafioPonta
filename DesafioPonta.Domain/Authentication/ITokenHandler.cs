using DesafioPonta.Api.Domain.Models.Entities;

namespace DesafioPonta.Api.Domain.Authentication
{
    public interface ITokenHandler
    {
        dynamic GenerateToken(Usuario? usuario);
        string GetUserIdFromToken(string token);
        bool CheckIfCreatedByUser(Guid userId, string jwtToken);
    }
}
