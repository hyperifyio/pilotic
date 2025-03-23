using System.Collections.Generic;
using Pilotic.Domain.Models;
using Xunit;

namespace Pilotic.Tests
{
    public class IssueTests
    {
        [Fact]
        public void TestIssueCreation()
        {
            var issue = new Issue
            {
                Id = "1",
                Title = "Test Issue",
                Description = "Test Description",
                Labels = new List<Label> { new Label(){Id = "Test"} }
            };

            Assert.Equal("1", issue.Id);
            Assert.Equal("Test Issue", issue.Title);
            Assert.Equal("Test Description", issue.Description);
            Assert.Single(issue.Labels);
            Assert.Contains(issue.Labels, label => label.Id.Equals("test", StringComparison.OrdinalIgnoreCase));
        }
    }
} 