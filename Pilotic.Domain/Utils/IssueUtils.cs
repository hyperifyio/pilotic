using System.Text;
using System.Text.RegularExpressions;
using Pilotic.Domain.Models;

namespace Pilotic.Domain.Utils;

public static class IssueUtils
{

    /// <summary>
    /// Copy the base properties of one issue to another.
    /// </summary>
    /// <param name="toIssue"></param>
    /// <param name="fromIssue"></param>
    public static void CopyIssueProperties(Issue toIssue, Issue fromIssue)
    {
        toIssue.Id = fromIssue.Id;
        toIssue.ParentId = fromIssue.ParentId;
        toIssue.Title = fromIssue.Title;
        toIssue.Description = fromIssue.Description;
        toIssue.Status = fromIssue.Status;
        toIssue.Type = fromIssue.Type;
        toIssue.CreatedAt = fromIssue.CreatedAt;
        toIssue.UpdatedAt = fromIssue.UpdatedAt;
        toIssue.DueDate = fromIssue.DueDate;
        toIssue.MilestoneId = fromIssue.MilestoneId;
    }
    
    /// <summary>
    /// Find a sub-issue by its issue ID.
    /// </summary>
    /// <param name="rootIssue"></param>
    /// <param name="issueId"></param>
    /// <returns></returns>
    public static Issue? FindChildIssue(Issue rootIssue, string issueId)
    {
        if (rootIssue.Id == issueId)
        {
            return rootIssue;
        }
        foreach (var subIssue in rootIssue.SubIssues)
        {
            var foundIssue = FindChildIssue(subIssue, issueId);
            if (foundIssue != null)
            {
                return foundIssue;
            }
        }
        return null;
    }

    /// <summary>
    /// Find all parent issues of an issue.
    /// </summary>
    /// <param name="issue"></param>
    /// <param name="rootIssue"></param>
    /// <returns></returns>
    private static List<Issue> FindParentIssues(List<Issue> parents, Issue rootIssue, Issue issue)
    {
        if (issue.ParentId == null)
        {
            return parents;
        }
        var parentIssue = FindChildIssue(rootIssue, issue.ParentId);
        if (parentIssue != null)
        {
            parents.Add(parentIssue);
            return FindParentIssues(parents, rootIssue, parentIssue);
        }
        return parents;
    }

    /// <summary>
    /// Find all parent issues of an issue. The root issue will be first, and the direct parent issue will be last.
    /// </summary>
    /// <param name="issue"></param>
    /// <param name="rootIssue"></param>
    /// <returns></returns>
    public static List<Issue> FindParentIssues(Issue rootIssue, Issue issue)
    {
        var parentIssues = new List<Issue>();
        FindParentIssues(parentIssues, rootIssue, issue);
        parentIssues.Reverse();
        return parentIssues;
    }

    /// <summary>
    /// Transforms issue references in markdown text, such as `#123`, into formatted internal links like `[Requirement 123 - Do Something](#123)`.
    /// Uses the provided root issue to search and validate issue references, generating titles through ToIssueTitlePrefix().
    /// </summary>
    /// <param name="text">Markdown text to transform.</param>
    /// <param name="rootIssue">Root issue used for searching referenced issues.</param>
    /// <returns>Transformed markdown with internal links.</returns>
    public static string TransformIssueLinks(string text, Issue rootIssue)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var markdown = new StringBuilder();
        var issueLinkPattern = new Regex(@"#(\d+)");
    
        int lastIndex = 0;

        foreach (Match match in issueLinkPattern.Matches(text))
        {
            // Append the text preceding the match
            markdown.Append(text, lastIndex, match.Index - lastIndex);

            var issueId = match.Groups[1].Value;
            var issue = FindChildIssue(rootIssue, issueId);

            if (issue != null)
            {
                var issueTitle = ToIssueTitlePrefix(issue.Type, issue.Id, issue.Title);
                markdown.AppendFormat("[{0}](#{1})", issueTitle, issueId);
            }
            else
            {
                markdown.Append(match.Value); // Append original text if issue not found
            }

            // Update the last index position
            lastIndex = match.Index + match.Length;
        }

        // Append remaining text after the last match
        if (lastIndex < text.Length)
        {
            markdown.Append(text.Substring(lastIndex));
        }

        return markdown.ToString();
    }
    
    public static String ExportFullMarkdown(Issue issue)
    {
        var markdown = new StringBuilder();
        ExportFullMarkdown(markdown, issue, issue);
        return markdown.ToString();
    }
    
    public static String ExportFullMarkdown(Issue issue, Issue rootIssue)
    {
        var markdown = new StringBuilder();
        ExportFullMarkdown(markdown, issue, rootIssue);
        return markdown.ToString();
    }
    
    public static void ExportFullMarkdown(StringBuilder markdown, Issue issue, Issue rootIssue) {
        ExportIssueMarkdown(markdown, issue, rootIssue);
        foreach (var subIssue in issue.SubIssues) {
            ExportFullMarkdown(markdown, subIssue, rootIssue);
        }
    }

    public static void ExportIssueMarkdown(StringBuilder markdown, Issue issue, Issue rootIssue) {
        markdown.AppendLine("<a name=\"" + issue.Id + "\"></a>");
        markdown.AppendLine($"# {ToIssueTitlePrefix(issue.Type, issue.Id, issue.Title)}");
        markdown.AppendLine();
        markdown.AppendLine(TransformIssueLinks(issue.Description, rootIssue));
        markdown.AppendLine();
        markdown.AppendLine("----");
        markdown.AppendLine();
    }

    public static string ToIssueTitlePrefix(IssueType type, string issueId, string title)
    {
        return type switch
        {
            IssueType.Bug => $"Bug {issueId} - {title}",
            IssueType.Feature => $"Feature {issueId} - {title}",
            IssueType.Task => $"Task {issueId} - {title}",
            IssueType.Root => $"{title}",
            IssueType.Story => $"Story {issueId} - {title}",
            IssueType.Epic => $"Epic {issueId} - {title}",
            IssueType.EpicList => $"{title}",
            IssueType.Requirement => $"Requirement {issueId} - {title}",
            IssueType.RequirementList => $"{title}",
            IssueType.Actor => $"Actor {issueId} - {title}",
            IssueType.ActorList => $"{title}",
            IssueType.Sprint => $"Sprint {issueId} - {title}",
            IssueType.SprintList => $"{title}",
            _ => ""
        };
    }
    
    /// <summary>
    /// Test partially equal issues, e.g. own properties which are not linked to other entities.
    /// </summary>
    /// <param name="toIssue"></param>
    /// <param name="fromIssue"></param>
    /// <returns></returns>
    public static bool IsPartiallyEqualIssue(Issue toIssue, Issue fromIssue)
    {
        return toIssue.Title == fromIssue.Title &&
               toIssue.ParentId == fromIssue.ParentId &&
               toIssue.Description == fromIssue.Description &&
               toIssue.Status == fromIssue.Status &&
               toIssue.Type == fromIssue.Type &&
               toIssue.CreatedAt == fromIssue.CreatedAt &&
               toIssue.UpdatedAt == fromIssue.UpdatedAt &&
               toIssue.DueDate == fromIssue.DueDate &&
               toIssue.MilestoneId == fromIssue.MilestoneId;
    }
    
}