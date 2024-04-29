using DesafioPonta.Api.Domain.Models.Entities;

namespace DesafioPonta.Api.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<ICollection<Usuario>> GetAllAsync();
        Task<Usuario> GetByIdAsync(Guid id);
        Task<Usuario?> CreateAsync(Usuario usuario);
        Task<Usuario?> GetUsuarioByEmail(string email);
        Task<Usuario?> GetUsuarioByEmailAndSenha(string email,string senha);
    }
}
