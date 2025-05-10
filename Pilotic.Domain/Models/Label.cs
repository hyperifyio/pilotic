using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pilotic.Core.Interfaces;

namespace Pilotic.Domain.Models;

/// <summary>
/// Represents a label that can be attached to issues.
/// </summary>
public class Label : IRepositoryEntity
{

    /// <summary>
    /// The unique identifier for the label.
    /// </summary>
    /// <example>high-priority</example>
    [Key]
    [JsonPropertyName("label")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The description of the label.
    /// </summary>
    /// <example>High Priority</example>
    [Required, MaxLength(255)]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The color of the label.
    /// </summary>
    /// <example>High Priority</example>
    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

}
