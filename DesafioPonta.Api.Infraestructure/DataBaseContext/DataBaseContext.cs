using DesafioPonta.Api.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioPonta.Api.Infraestructure
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }


        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

    }
}
