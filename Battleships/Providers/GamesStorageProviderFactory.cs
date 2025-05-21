using Battleships.Models.Settings;
using Battleships.Providers.Interfaces;
using Microsoft.Extensions.Options;

namespace Battleships.Providers;

/// <summary>
/// Implementation of the <see cref="IGamesStorageProviderFactory" /> interface for creating games storage providers.
/// </summary>
/// <seealso cref="Battleships.Providers.Interfaces.IGamesStorageProviderFactory" />
public class GamesStorageProviderFactory(
    ILogger<GamesStorageProviderFactory> logger,
    IOptions<ApplicationSettings> settings,
    IServiceProvider services) 
    : IGamesStorageProviderFactory
{
    /// <summary>
    /// Gets the settings.
    /// </summary>
    /// <value>
    /// The settings.
    /// </value>
    private ApplicationSettings Settings { get; } = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    private ILogger<GamesStorageProviderFactory> Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    private IServiceProvider Services { get; } = services ?? throw new ArgumentNullException(nameof(services));

    /// <inheritdoc />
    public IGamesStorageProvider GetProvider()
    {
        this.Logger.LogDebug($"Method {nameof(this.GetProvider)} started.");

        var service = this.Services.GetKeyedService<IGamesStorageProvider>(this.Settings.GamesStorageProvider.ToString());

        if (service is null)
        {
            throw new InvalidOperationException(
                $"No service with key {this.Settings.GamesStorageProvider.ToString()} found");
        }

        this.Logger.LogDebug($"Method {nameof(this.GetProvider)} ended with service '{service.GetType().FullName}' found.");

        return service;
    }
}