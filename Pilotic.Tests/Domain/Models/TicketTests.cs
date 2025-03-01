using Xunit;
using Pilotic.Domain.Models;

namespace Pilotic.Tests.Domain.Models
{
    public class TicketTests
    {
        [Fact]
        public void Ticket_Should_Have_Default_Values()
        {
            var ticket = new Ticket();
            Assert.Equal("", ticket.Id);
            Assert.Equal("", ticket.Title);
            Assert.Equal("", ticket.Description);
            Assert.Equal(TicketStatus.Unknown, ticket.Status);
            Assert.Equal(TicketType.Undefined, ticket.Type);
            Assert.Null((object)ticket.CreatedAt);
            Assert.Null((object)ticket.UpdatedAt);
            Assert.Null((object)ticket.DueDate);
            Assert.Empty(ticket.Assignees);
            Assert.Empty(ticket.Labels);
            Assert.Null((object)ticket.ParentId);
            Assert.Null((object)ticket.MilestoneId);
            Assert.Empty(ticket.Comments);
            Assert.False(ticket.Pinned);
            Assert.False(ticket.ConversationLocked);
        }

        [Fact]
        public void UpdateTitle_Should_Change_Title()
        {
            var ticket = new Ticket();
            var newTitle = "New Title";
            ticket.Title = newTitle;
            Assert.Equal(newTitle, ticket.Title);
        }

        [Fact]
        public void UpdateDescription_Should_Change_Description()
        {
            var ticket = new Ticket();
            var newDescription = "New Description";
            ticket.Description = newDescription;
            Assert.Equal(newDescription, ticket.Description);
        }
    }
}
