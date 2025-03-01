using System.Text.Json.Serialization;

namespace Pilotic.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TicketStatus
{

    [JsonPropertyName("unknown")]
    Unknown,

    [JsonPropertyName("open")]
    Open,

    [JsonPropertyName("closed")]
    Closed,

}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TicketType
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

public class Ticket
{
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public TicketStatus Status { get; set; } = TicketStatus.Unknown;

    [JsonPropertyName("type")]
    public TicketType Type { get; set; } = TicketType.Undefined;

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("due_date")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("assignees")]
    public List<string> Assignees { get; set; } = Array.Empty<string>().ToList(); // GitHub usernames or internal IDs

    [JsonPropertyName("labels")]
    public List<string> Labels { get; set; } = Array.Empty<string>().ToList(); // Tags or categories

    [JsonPropertyName("parent_id")]
    public string? ParentId { get; set; }

    [JsonPropertyName("milestone_id")]
    public string? MilestoneId { get; set; }

    [JsonPropertyName("comments")]
    public List<string> Comments { get; set; } = Array.Empty<string>().ToList(); // Comment IDs or message texts

    [JsonPropertyName("pinned")] public bool Pinned { get; set; } = false;

    [JsonPropertyName("conversation_locked")]
    public bool ConversationLocked { get; set; } = false;

}
