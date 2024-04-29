using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DesafioPonta.Api.Infraestructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataBaseContext _dataBaseContext;

        public UsuarioRepository(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            usuario.Id = new Guid();
            _dataBaseContext.Add(usuario);
            await _dataBaseContext.SaveChangesAsync();
            return usuario;
        }

        public async Task<ICollection<Usuario>> GetAllAsync()
        {
            return await _dataBaseContext.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(Guid id)
        {
            return await _dataBaseContext.Usuarios.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Usuario?> GetUsuarioByEmail(string email)
        {
            return await _dataBaseContext.Usuarios.FirstOrDefaultAsync(i => i.Email == email);
        }

        public async Task<Usuario?> GetUsuarioByEmailAndSenha(string email, string senha)
        {
            return await _dataBaseContext.Usuarios.FirstOrDefaultAsync(i => i.Email == email && i.Senha == senha);
        }
    }
}
