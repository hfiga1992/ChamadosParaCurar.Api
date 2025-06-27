using System.ComponentModel.DataAnnotations;

namespace ChamadosParaCurar.Api.Models.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        [Required]
        public string Senha { get; set; } = null!;
    }
} 