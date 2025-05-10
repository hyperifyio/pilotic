using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pilotic.Core.Interfaces;

namespace Pilotic.Domain.Models;

public class VerificationCode : IRepositoryEntity
{
    [Key]
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [JsonPropertyName("email")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("codeHash")]
    [MaxLength(512)]
    public string CodeHash { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("expiresAt")]
    public DateTime ExpiresAt { get; set; }

    [Required]
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 