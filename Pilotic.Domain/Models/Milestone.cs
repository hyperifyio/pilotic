using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pilotic.Domain.Models;

public class Milestone
{
    [Key]
    [JsonPropertyName("milestoneId")]
    public string MilestoneId { get; set; } = Guid.NewGuid().ToString();
    
    [Required, MaxLength(255)]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    
    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("issues")]
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    
}
