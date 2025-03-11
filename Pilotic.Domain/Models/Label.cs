using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pilotic.Domain.Models;

public class Label
{
    [Key]
    [JsonPropertyName("label")]
    public string Tag { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

}
