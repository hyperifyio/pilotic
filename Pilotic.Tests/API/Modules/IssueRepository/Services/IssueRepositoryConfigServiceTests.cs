// `Pilotic.Tests/IssueRepository/Services/IssueRepositoryConfigServiceTests.cs`
using Xunit;
using Moq;
using Pilotic.Core.Interfaces;
using Pilotic.API.Modules.IssueRepository.Models;
using Pilotic.API.Modules.IssueRepository.Services;

namespace Pilotic.Tests.API.Modules.IssueRepository.Services;

public class IssueRepositoryConfigServiceTests
{
    [Fact]
    public void Constructor_ValidConfig_SetsRepositoryValues()
    {
        // Arrange
        var mockService = new Mock<IConfigService>();
        mockService
            .Setup(c => c.GetConfig<IssueRepositoryConfig>("IssueRepository"))
            .Returns(new IssueRepositoryConfig
            {
                Repository = "test/repo",
                RootIssueId = "100"
            });

        // Act
        var sut = new IssueRepositoryConfigService(mockService.Object);

        // Assert
        Assert.Equal("test/repo", sut.Repository);
        Assert.Equal("100", sut.RootIssueId);
    }
}
