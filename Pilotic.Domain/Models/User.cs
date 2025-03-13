using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pilotic.Core.Interfaces;

namespace Pilotic.Domain.Models;

public class User : IRepositoryEntity
{
    [Key]
    [JsonPropertyName("userId")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("username")]
    [Required, MaxLength(255)]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    [Required, MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
}
