using ChamadosParaCurar.Api.Interfaces;
using ChamadosParaCurar.Api.Models;
using ChamadosParaCurar.Api.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace ChamadosParaCurar.Api.Services
{
    public class DevocionalService : IDevocionalService
    {
        private readonly IMongoCollection<Devocional> _devocionalCollection;

        public DevocionalService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var settings = MongoClientSettings.FromConnectionString(mongoDbSettings.Value.ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            
            var mongoClient = new MongoClient(settings);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _devocionalCollection = mongoDatabase.GetCollection<Devocional>(
                mongoDbSettings.Value.DevocionalCollectionName);
        }

        public async Task<Devocional?> ObterPorData(DateTime data)
        {
            // Criar um intervalo para a data (início e fim do dia)
            var inicioData = data.Date;
            var fimData = inicioData.AddDays(1);

            // Buscar devocional para a data específica
            var devocional = await _devocionalCollection
                .Find(d => d.Data >= inicioData && d.Data < fimData)
                .FirstOrDefaultAsync();

            return devocional;
        }

        public async Task<Devocional?> ObterDevocionalHoje()
        {
            return await ObterPorData(DateTime.Today);
        }

        public async Task<Devocional> CriarDevocional(Devocional devocional)
        {
            // Garantir que o Id está null para o MongoDB gerar automaticamente
            devocional.Id = null;
            
            // Se a data não for especificada, usar a data atual
            if (devocional.Data == default)
            {
                devocional.Data = DateTime.Today;
            }
            else
            {
                // Garantir que apenas a parte da data seja usada (sem hora)
                devocional.Data = devocional.Data.Date;
            }
            
            // Verificar se já existe um devocional para esta data
            var devocionalExistente = await ObterPorData(devocional.Data);
            if (devocionalExistente != null)
            {
                throw new InvalidOperationException($"Já existe um devocional para a data {devocional.Data.ToShortDateString()}");
            }
            
            await _devocionalCollection.InsertOneAsync(devocional);
            return devocional;
        }
        
        public async Task<List<Devocional>> ListarTodos()
        {
            // Retornar todos os devocionais ordenados por data (mais recente primeiro)
            return await _devocionalCollection
                .Find(_ => true)
                .SortByDescending(d => d.Data)
                .ToListAsync();
        }
    }
} 