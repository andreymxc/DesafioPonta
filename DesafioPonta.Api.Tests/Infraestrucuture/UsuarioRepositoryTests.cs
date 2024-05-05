using AutoMapper.Configuration.Annotations;
using DesafioPonta.Api.Domain.Models.Entities;
using DesafioPonta.Api.Domain.Repositories;
using DesafioPonta.Api.Infraestructure;
using DesafioPonta.Api.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Tests.Infraestrucuture
{
    public class UsuarioRepositoryTests
    {
        private readonly DbContextOptions<DataBaseContext> _options;
        private Usuario _usuarioExistente = new Usuario() { Id = Guid.NewGuid(), Email = "testeRepository@teste.com.br", Senha = "TesteSenha!123" };

        public UsuarioRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(databaseName: "DesafioPontaTesteUsuario")
                .Options;

            using (var context = new DataBaseContext(_options))
            {
                context.Usuarios.Add(_usuarioExistente);
                context.SaveChanges();
            }
        }


        [Fact]
        public async Task CreateUsuario_ShouldCreateUser_WhenUserIsValid()
        {
            using (var context = new DataBaseContext(_options))
            {
                var usuarioRepository = new UsuarioRepository(context);

                var novoUsuario = new Usuario() { Email = "teste2@teste.com.br", Senha = "TesteSenha!123" };

                var result = await usuarioRepository.CreateAsync(novoUsuario);

                Assert.NotNull(result);
                Assert.NotEqual(Guid.Empty, result.Id);
                Assert.Equal(novoUsuario.Email, result.Email);
                Assert.Equal(novoUsuario.Senha, result.Senha);
            }
        }

        [Fact]
        public async Task CreateUsuario_ShowThrowNullReferenceException_WhenUsuarioIsNull()
        {
            using (var context = new DataBaseContext(_options))
            {
                var usuarioRepository = new UsuarioRepository(context);

                Usuario usuario = null;

                await Assert.ThrowsAsync<NullReferenceException>(async () => await usuarioRepository.CreateAsync(usuario));
            }
        }

        [Fact]
        public async Task GetUsuarioByEmailESenha_ReturnsUsuario_WhenEmailAndSenhaAreCorrect()
        {
            using (var context = new DataBaseContext(_options))
            {
                var usuarioRepository = new UsuarioRepository(context);

                var result = await usuarioRepository.GetUsuarioByEmailAndSenha(_usuarioExistente.Email, _usuarioExistente.Senha);

                Assert.NotNull(result);
                Assert.Equal(_usuarioExistente.Id, result.Id);
                Assert.Equal(_usuarioExistente.Email, result.Email);
                Assert.Equal(_usuarioExistente.Senha, result.Senha);
            }
        }

        [Fact]
        public async Task GetUsuarioByEmailESenha_ReturnsNull_WhenEmailIsIncorrect()
        {
            using (var context = new DataBaseContext(_options))
            {
                var usuarioRepository = new UsuarioRepository(context);

                string emailInvalido = "testeInvalido@teste.com.br";

                var result = await usuarioRepository.GetUsuarioByEmailAndSenha(emailInvalido, _usuarioExistente.Senha);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetUsuarioByEmailESenha_ReturnsNull_WhenSenhaIsIncorrect()
        {
            using (var context = new DataBaseContext(_options))
            {
                var usuarioRepository = new UsuarioRepository(context);
                string senhaIncorreta = "SenhaIncorreta!123";

                var result = await usuarioRepository.GetUsuarioByEmailAndSenha(_usuarioExistente.Email, senhaIncorreta);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetUsuarioByEmail_ShouldReturnUsuario_WhenEmailExistsInDatabase()
        {
            using (var context = new DataBaseContext(_options))
            {
                var usuarioRepository = new UsuarioRepository(context);

                var result = await usuarioRepository.GetUsuarioByEmail(_usuarioExistente.Email);

                Assert.NotNull(result);
                Assert.Equal(_usuarioExistente.Email, result.Email);
                Assert.Equal(_usuarioExistente.Senha, result.Senha);
            }
        }

        [Fact]
        public async Task GetUsuarioByEmail_ShouldReturnNull_WhenEmailIsNotInDatabase()
        {
            using (var context = new DataBaseContext(_options))
            {
                var usuarioRepository = new UsuarioRepository(context);

                string emailInvalido = "emailInvalido@teste.com.br";

                var result = await usuarioRepository.GetUsuarioByEmail(emailInvalido);

                Assert.Null(result);
            }
        }
    }
}
