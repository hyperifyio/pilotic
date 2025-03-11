using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Pilotic.Domain.Models;

public class Comment
{
    [Key]
    [JsonPropertyName("commentId")]
    public string CommentId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    [JsonPropertyName("issueId")]
    public string IssueId { get; set; } = string.Empty;

    [JsonPropertyName("issue")]
    [ForeignKey("IssueId")]
    public Issue Issue { get; set; } = null!;
    
}
