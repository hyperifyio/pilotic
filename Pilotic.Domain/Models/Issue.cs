using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pilotic.Core.Interfaces;

namespace Pilotic.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IssueStatus
{

    [JsonPropertyName("unknown")]
    Unknown,

    [JsonPropertyName("open")]
    Open,

    [JsonPropertyName("closed")]
    Closed,

}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IssueType
{
    [JsonPropertyName("undefined")]
    Undefined,
    [JsonPropertyName("bug")]
    Bug,
    [JsonPropertyName("feature")]
    Feature,
    [JsonPropertyName("task")]
    Task,
}

public class Issue : IRepositoryEntity
{
    [Key]
    [JsonPropertyName("issueId")] 
    public string Id { get; set; } = string.Empty;
    
    [Required, MaxLength(255)]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public IssueStatus Status { get; set; } = IssueStatus.Unknown;

    [JsonPropertyName("type")]
    public IssueType Type { get; set; } = IssueType.Undefined;

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("assignees")]
    public List<User> Assignees { get; set; } = Array.Empty<User>().ToList();

    [JsonPropertyName("labels")]
    public List<Label> Labels { get; set; } = Array.Empty<Label>().ToList();

    [JsonPropertyName("parentId")]
    public string? ParentId { get; set; }

    [JsonPropertyName("milestoneId")]
    public string? MilestoneId { get; set; }

    [JsonPropertyName("comments")]
    public List<Comment> Comments { get; set; } = Array.Empty<Comment>().ToList();

    [JsonPropertyName("pinned")] public bool Pinned { get; set; } = false;

    [JsonPropertyName("conversation_locked")]
    public bool ConversationLocked { get; set; } = false;
    
    [JsonPropertyName("subIssues")]
    public List<Issue> SubIssues { get; set; } = Array.Empty<Issue>().ToList();

}
