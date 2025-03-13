using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pilotic.Core.Interfaces;

namespace Pilotic.Domain.Models;

public class Label : IRepositoryEntity
{
    [Key]
    [JsonPropertyName("label")]
    public string Id { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

}
