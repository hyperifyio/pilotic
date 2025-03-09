using Xunit;
using Pilotic.Domain.Models;

namespace Pilotic.Tests.Domain.Models
{
    public class IssueTests
    {
        [Fact]
        public void Issue_Should_Have_Default_Values()
        {
            var issue = new Issue();
            Assert.Equal("", issue.Id);
            Assert.Equal("", issue.Title);
            Assert.Equal("", issue.Description);
            Assert.Equal(IssueStatus.Unknown, issue.Status);
            Assert.Equal(IssueType.Undefined, issue.Type);
            Assert.Null((object)issue.CreatedAt);
            Assert.Null((object)issue.UpdatedAt);
            Assert.Null((object)issue.DueDate);
            Assert.Empty(issue.Assignees);
            Assert.Empty(issue.Labels);
            Assert.Null((object)issue.ParentId);
            Assert.Null((object)issue.MilestoneId);
            Assert.Empty(issue.Comments);
            Assert.False(issue.Pinned);
            Assert.False(issue.ConversationLocked);
        }

        [Fact]
        public void UpdateTitle_Should_Change_Title()
        {
            var issue = new Issue();
            var newTitle = "New Title";
            issue.Title = newTitle;
            Assert.Equal(newTitle, issue.Title);
        }

        [Fact]
        public void UpdateDescription_Should_Change_Description()
        {
            var issue = new Issue();
            var newDescription = "New Description";
            issue.Description = newDescription;
            Assert.Equal(newDescription, issue.Description);
        }
    }
}
