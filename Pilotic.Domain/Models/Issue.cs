using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pilotic.Core.Interfaces;

namespace Pilotic.Domain.Models;

/// <summary>
/// Represents the status of an issue.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IssueStatus
{

    /// <summary>
    /// The status of the issue is unknown.
    /// </summary>
    [JsonPropertyName("unknown")]
    Unknown,

    /// <summary>
    /// The issue is open.
    /// </summary>
    [JsonPropertyName("open")]
    Open,

    /// <summary>
    /// The issue is closed.
    /// </summary>
    [JsonPropertyName("closed")]
    Closed,

}

/// <summary>
/// Represents the type of an issue.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IssueType
{
    /// <summary>
    /// Undefined – Default type for issues whose classification hasn't yet been decided.
    /// </summary>
    [JsonPropertyName("n/a")]
    Undefined,
    
    /// <summary>
    /// Other – Clearly defined issue types not currently supported by our application.
    /// </summary>
    [JsonPropertyName("Other")]
    Other,
    
    /// <summary>
    /// Bug – Identifies an issue or defect causing unexpected behavior or errors in existing functionality.
    /// </summary>
    [JsonPropertyName("Bug")]
    Bug,
    
    /// <summary>
    /// Feature – Represents new functionality or enhancements that add value for users.
    /// </summary>
    [JsonPropertyName("Feature")]
    Feature,

    /// <summary>
    /// Task – Defines actionable work that needs to be completed within the project, usually specific and well-scoped.
    /// </summary>
    [JsonPropertyName("Task")]
    Task,

    /// <summary>
    /// Root – The primary issue representing the project itself; all other issues are organized as sub-issues under this.
    /// </summary>
    [JsonPropertyName("Root")]
    Root,
    
    /// <summary>
    /// Story – Describes a specific user requirement or interaction, providing user-centered context.
    /// </summary>
    [JsonPropertyName("Story")]
    Story,
    
    /// <summary>
    /// Epic – Represents a large-scale objective or initiative consisting of multiple related user stories.
    /// </summary>
    [JsonPropertyName("Epic")]
    Epic,
    
    /// <summary>
    /// EpicList – Serves as a container for organizing multiple epics, commonly used for backlog management.
    /// </summary>
    [JsonPropertyName("EpicList")]
    EpicList,
    
    /// <summary>
    /// Requirement – Defines a specific, often technical or functional, criterion that the software must fulfill.
    /// </summary>
    [JsonPropertyName("Requirement")]
    Requirement,
    
    /// <summary>
    /// RequirementList – Categorizes and groups related requirements logically (e.g., Business Requirements, Technical Requirements).
    /// </summary>
    [JsonPropertyName("RequirementList")]
    RequirementList,
    
    /// <summary>
    /// Actor – Defines roles or personas interacting with or impacted by the system.
    /// </summary>
    [JsonPropertyName("Actor")]
    Actor,
    
    /// <summary>
    /// ActorList – Organizes and describes all actors into a logical structure or hierarchy.
    /// </summary>
    [JsonPropertyName("ActorList")]
    ActorList,
    
    /// <summary>
    /// Sprint – Represents a fixed-duration iteration during which selected stories and tasks are implemented.
    /// </summary>
    [JsonPropertyName("Sprint")]
    Sprint,
    
    /// <summary>
    /// SprintList – Maintains a structured record of multiple sprints, typically ordered chronologically.
    /// </summary>
    [JsonPropertyName("SprintList")]
    SprintList,

}

/// <summary>
/// Represents an issue in a repository.
/// </summary>
public class Issue : IRepositoryEntity
{
    
    /// <summary>
    /// The unique identifier of the issue.
    /// </summary>
    /// <example>ISSUE-123</example>
    [Key]
    [JsonPropertyName("issueId")] 
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// The title of the issue.
    /// </summary>
    /// <example>Implement user authentication</example>
    [Required, MaxLength(255)]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The detailed description of the issue in markdown format.
    /// </summary>
    /// <example>Add JWT-based authentication with refresh tokens</example>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The status of the issue.
    /// </summary>
    [JsonPropertyName("status")]
    public IssueStatus Status { get; set; } = IssueStatus.Unknown;

    /// <summary>
    /// The type of the issue.
    /// </summary>
    /// <example>Feature</example>
    [JsonPropertyName("type")]
    public IssueType Type { get; set; } = IssueType.Undefined;

    /// <summary>
    /// The date and time when the issue was created.
    /// </summary>
    /// <example>2024-03-20T10:00:00Z</example>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// The date and time when the issue was last updated.
    /// </summary>
    /// <example>2024-03-20T10:00:00Z</example>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// The date when the issue is due to be completed.
    /// </summary>
    /// <example>2024-04-01</example>
    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// List of users assigned to this issue.
    /// </summary>
    /// <example>[{"id": "user1", "name": "John Doe"}]</example>
    [JsonPropertyName("assignees")]
    public List<User> Assignees { get; set; } = Array.Empty<User>().ToList();

    /// <summary>
    /// List of labels associated with this issue.
    /// </summary>
    /// <example>[{"id": "high-priority", "name": "High Priority"}]</example>
    [JsonPropertyName("labels")]
    public List<Label> Labels { get; set; } = Array.Empty<Label>().ToList();

    /// <summary>
    /// The ID of the parent issue, if this is a sub-issue.
    /// </summary>
    /// <example>ISSUE-100</example>
    [JsonPropertyName("parentId")]
    public string? ParentId { get; set; }

    /// <summary>
    /// The ID of the milestone this issue belongs to.
    /// </summary>
    /// <example>MILESTONE-1</example>
    [JsonPropertyName("milestoneId")]
    public string? MilestoneId { get; set; }

    [JsonPropertyName("comments")]
    public List<Comment> Comments { get; set; } = Array.Empty<Comment>().ToList();

    [JsonPropertyName("pinned")] public bool Pinned { get; set; } = false;

    [JsonPropertyName("conversation_locked")]
    public bool ConversationLocked { get; set; } = false;
    
    /// <summary>
    /// List of sub-issues that belong to this issue.
    /// </summary>
    [JsonPropertyName("subIssues")]
    public List<Issue> SubIssues { get; set; } = Array.Empty<Issue>().ToList();

}


