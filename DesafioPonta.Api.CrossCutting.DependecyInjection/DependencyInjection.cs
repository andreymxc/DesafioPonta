using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DesafioPonta.Api.Infraestructure;
using DesafioPonta.Api.Domain.Repositories;
using DesafioPonta.Api.Application.Services.Interfaces;
using DesafioPonta.Api.Application.Services;
using DesafioPonta.Api.Infraestructure.Repositories;
using DesafioPonta.Api.Application.Mappings;
using Microsoft.EntityFrameworkCore;
using DesafioPonta.Api.Domain.Authentication;
using DesafioPonta.Api.Infraestructure.Authentication;
using Microsoft.AspNetCore.Http;

namespace DesafioPonta.Api.CrossCutting.DependecyInjection
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataBaseContext>(options => options.UseInMemoryDatabase("DesafioPonta"));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITarefaRepository, TarefaRepository>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(DomainToDtoMapping));
            services.AddAutoMapper(typeof(DtoToDomainMapping)); 
            services.AddScoped<ITarefaService, TarefaService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            return services;
        }
    }
}
