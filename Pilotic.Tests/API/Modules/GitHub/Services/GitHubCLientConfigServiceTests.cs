using Xunit;
using Moq;
using Pilotic.Core.Interfaces;
using Pilotic.API.Modules.GitHub.Models;
using Pilotic.API.Modules.GitHub.Services;

namespace Pilotic.Tests.API.Modules.GitHub.Services;

public class GitHubClientConfigServiceTests
{
    [Fact]
    public void Constructor_ValidConfig_SetsApiKeyProperty()
    {
        // Arrange
        var mockService = new Mock<IConfigService>();
        mockService
            .Setup(c => c.GetConfig<GitHubClientConfig>("GitHub"))
            .Returns(new GitHubClientConfig { ApiKey = "TestApiKey" });

        // Act
        var sut = new GitHubClientConfigService(mockService.Object);

        // Assert
        Assert.Equal("TestApiKey", sut.ApiKey);
    }
}
