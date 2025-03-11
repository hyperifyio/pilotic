using Xunit;
using Moq;
using Pilotic.Core.Interfaces;
using Pilotic.API.Modules.GitHub.Models;
using Pilotic.API.Modules.GitHub.Services;

namespace Pilotic.Tests.API.Modules.GitHub.Services;

public class GitHubConfigServiceTests
{
    [Fact]
    public void Constructor_ValidConfig_SetsApiKeyProperty()
    {
        // Arrange
        var mockService = new Mock<IConfigService>();
        mockService
            .Setup(c => c.GetConfig<GitHubConfig>("GitHub"))
            .Returns(new GitHubConfig { ApiKey = "TestApiKey" });

        // Act
        var sut = new GitHubConfigService(mockService.Object);

        // Assert
        Assert.Equal("TestApiKey", sut.ApiKey);
    }
}
