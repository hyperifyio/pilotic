using System.Text.Json.Serialization;

namespace Pilotic.Domain.Models;

public class CreateIssue
{

    [JsonPropertyName("issueId")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; } = string.Empty;
    
    [JsonPropertyName("type")]
    public IssueType? Type { get; set; } = IssueType.Undefined;

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("assignees")]
    public List<string>? Assignees { get; set; } = Array.Empty<string>().ToList(); // GitHub usernames or internal IDs

    [JsonPropertyName("labels")]
    public List<string>? Labels { get; set; } = Array.Empty<string>().ToList(); // Tags or categories

    [JsonPropertyName("parentId")]
    public string? ParentId { get; set; }

    [JsonPropertyName("milestoneId")]
    public string? MilestoneId { get; set; }

}
