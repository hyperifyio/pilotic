using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pilotic.Domain.Models;

public class User
{
    [Key]
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("username")]
    [Required, MaxLength(255)]
    public string Username { get; set; } = string.Empty;
    
}
