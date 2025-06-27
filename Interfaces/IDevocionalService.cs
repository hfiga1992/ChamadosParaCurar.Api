using ChamadosParaCurar.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChamadosParaCurar.Api.Interfaces
{
    public interface IDevocionalService
    {
        Task<Devocional?> ObterPorData(DateTime data);
        Task<Devocional?> ObterDevocionalHoje();
        Task<Devocional> CriarDevocional(Devocional devocional);
        Task<List<Devocional>> ListarTodos();
    }
} 