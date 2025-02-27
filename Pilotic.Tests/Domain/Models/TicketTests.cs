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
