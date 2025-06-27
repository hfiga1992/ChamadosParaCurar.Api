using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChamadosParaCurar.Api.Models
{
    public class Devocional
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [BsonElement("titulo")]
        [Required(ErrorMessage = "O título é obrigatório")]
        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = null!;

        [BsonElement("conteudo")]
        [Required(ErrorMessage = "O conteúdo é obrigatório")]
        [JsonPropertyName("conteudo")]
        public string Conteudo { get; set; } = null!;

        [BsonElement("data")]
        [Required(ErrorMessage = "A data é obrigatória")]
        [JsonPropertyName("data")]
        public DateTime Data { get; set; }
    }
} 