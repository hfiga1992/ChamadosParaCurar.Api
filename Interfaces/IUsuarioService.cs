using ChamadosParaCurar.Api.Models;
using System;
using System.Threading.Tasks;

namespace ChamadosParaCurar.Api.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> ValidarUsuario(string email, string senha);
        Task<Usuario> CriarUsuario(Usuario usuario);
        Task<Usuario?> ObterPorEmail(string email);
        Task<Usuario?> ObterPorId(string id);
        Task<bool> MarcarDevocionalComoLido(string userId, DateOnly data);
        Task<bool> VerificarDevocionalLido(string userId, DateOnly data);
    }
} 