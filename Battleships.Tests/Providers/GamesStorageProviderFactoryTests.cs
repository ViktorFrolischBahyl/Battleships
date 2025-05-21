using Battleships.Providers;
using Battleships.Providers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Battleships.Tests.Providers;

[TestClass]
public sealed class GamesStorageProviderFactoryTests
{
    public GamesStorageProviderFactoryTests()
    {
        var builder = Program.CreateHostBuilder(args: []);

        var host = builder.Build();

        this.GamesStorageProviderFactory = ActivatorUtilities.GetServiceOrCreateInstance<IGamesStorageProviderFactory>(host.Services);
    }

    private IGamesStorageProviderFactory GamesStorageProviderFactory { get; }

    [TestMethod]
    public void InstanceTest()
    {
        var service = this.GamesStorageProviderFactory;

        Assert.IsNotNull(service);
    }

    [TestMethod]
    public void GetProviderTest()
    {
        var service = this.GamesStorageProviderFactory;

        var provider = service.GetProvider();

        Assert.IsNotNull(provider);
        Assert.IsInstanceOfType(provider, typeof(InMemoryGamesStorageProvider));
    }
}