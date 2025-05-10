using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Pilotic.Core.Services;

namespace Pilotic.Tests.Core.Services;

public class ConfigServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly ConfigService _configService;

    public ConfigServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();

        // Inject mock configuration into ConfigService
        _configService = new ConfigService(_mockConfiguration.Object);
    }

    [Fact]
    public void GetConfig_Should_Return_Valid_Config_Object()
    {
        // Arrange: Create test config values
        var testConfig = new Dictionary<string, string>
        {
            { "AppSettings:ApiUrl", "https://api.example.com" },
            { "AppSettings:RetryCount", "3" }
        };

        var configurationRoot = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfig)
            .Build();

        _mockConfiguration.Setup(c => c.GetSection("AppSettings")).Returns(configurationRoot.GetSection("AppSettings"));

        // Act
        var result = _configService.GetConfig<TestConfig>("AppSettings");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("https://api.example.com", result.ApiUrl);
        Assert.Equal(3, result.RetryCount);
    }

    [Fact]
    public void GetConfig_Should_Return_Default_When_Section_Not_Found()
    {
        // Arrange: Empty configuration
        var configurationRoot = new ConfigurationBuilder().Build();

        _mockConfiguration.Setup(c => c.GetSection("NonExistentSection")).Returns(configurationRoot.GetSection("NonExistentSection"));

        // Act
        var result = _configService.GetConfig<TestConfig>("NonExistentSection");

        // Assert: Default values should be returned
        Assert.NotNull(result);
        Assert.Null(result.ApiUrl);
        Assert.Equal(0, result.RetryCount);
    }

    // Helper class for testing
    private class TestConfig
    {
        public string? ApiUrl { get; set; }
        public int RetryCount { get; set; }
    }
}
