using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pilotic.Core.Interfaces;

namespace Pilotic.Domain.Models;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User : IRepositoryEntity
{

    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    /// <example>user1</example>
    [Key]
    [JsonPropertyName("userId")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// The username of the user.
    /// </summary>
    /// <example>John Doe</example>
    [JsonPropertyName("username")]
    [Required, MaxLength(255)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The display name of the user.
    /// </summary>
    /// <example>John Doe</example>
    [JsonPropertyName("name")]
    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the user.
    /// </summary>
    /// <example>John Doe</example>
    [JsonPropertyName("email")]
    [Required, MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
}
