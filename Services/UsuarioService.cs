using ChamadosParaCurar.Api.Interfaces;
using ChamadosParaCurar.Api.Models;
using ChamadosParaCurar.Api.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace ChamadosParaCurar.Api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuarioCollection;

        public UsuarioService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _usuarioCollection = mongoDatabase.GetCollection<Usuario>(
                mongoDbSettings.Value.UsuarioCollectionName);
        }

        public async Task<Usuario?> ValidarUsuario(string email, string senha)
        {
            return await _usuarioCollection
                .Find(u => u.Email == email && u.Senha == senha)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario> CriarUsuario(Usuario usuario)
        {
            // Verificar se o email já existe
            var usuarioExistente = await ObterPorEmail(usuario.Email);
            if (usuarioExistente != null)
            {
                throw new System.Exception("Email já cadastrado");
            }

            // Garantir que o Id está null para o MongoDB gerar automaticamente
            usuario.Id = null;

            await _usuarioCollection.InsertOneAsync(usuario);
            return usuario;
        }

        public async Task<Usuario?> ObterPorEmail(string email)
        {
            return await _usuarioCollection
                .Find(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario?> ObterPorId(string id)
        {
            return await _usuarioCollection
                .Find(u => u.Id == id)
                .FirstOrDefaultAsync();
        }
        
        public async Task<bool> MarcarDevocionalComoLido(string userId, DateOnly data)
        {
            var usuario = await ObterPorId(userId);
            if (usuario == null)
            {
                return false;
            }

            // Verificar se já está marcado como lido
            if (usuario.DevocionaisLidos.Contains(data))
            {
                return true; // Já está marcado como lido
            }

            // Adicionar a data à lista de devocionais lidos
            var updateDefinition = Builders<Usuario>.Update.Push(u => u.DevocionaisLidos, data);
            var result = await _usuarioCollection.UpdateOneAsync(u => u.Id == userId, updateDefinition);
            return result.ModifiedCount > 0;
        }
        
        public async Task<bool> VerificarDevocionalLido(string userId, DateOnly data)
        {
            var usuario = await ObterPorId(userId);
            if (usuario == null)
            {
                return false;
            }

            return usuario.DevocionaisLidos.Contains(data);
        }
    }
}