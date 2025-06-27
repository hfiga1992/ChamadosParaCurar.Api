namespace ChamadosParaCurar.Api.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string DevocionalCollectionName { get; set; } = null!;
        public string UsuarioCollectionName { get; set; } = null!;
    }
} 