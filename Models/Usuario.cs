using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ChamadosParaCurar.Api.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        [BsonElement("nome")]
        public string Nome { get; set; } = null!;
        
        [BsonElement("email")]
        public string Email { get; set; } = null!;
        
        [BsonElement("senha")]
        public string Senha { get; set; } = null!;
        
        [BsonElement("tipo")]
        public string Tipo { get; set; } = null!; // "Gratuito" ou "Pago"

        [BsonElement("devocionaisLidos")]
        public List<DateOnly> DevocionaisLidos { get; set; } = new List<DateOnly>();
    }
} 