using Battleships.Models.Settings;
using Battleships.Services;
using Battleships.Services.Interfaces;
using NLog;
using NLog.Web;
using System.Reflection;
using Battleships.Providers;
using Battleships.Providers.Interfaces;

namespace Battleships;

/// <summary>
/// Program class for the Battleships application.
/// </summary>
public class Program
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public static void Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("Battleships main initialization started.");

        try
        {
            var builder = CreateHostBuilder(args);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Battleships main initialization ended with error.");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    /// <summary>
    /// Creates the host builder.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>
    /// Web application builder with services registered.
    /// </returns>
    public static WebApplicationBuilder CreateHostBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection(nameof(ApplicationSettings)));

        builder.Services.AddSingleton<ILoadShipsDefinitionsService, LoadShipsDefinitionsFromFileService>();

        builder.Services.AddKeyedSingleton<IGamesStorageProvider, InMemoryGamesStorageProvider>(nameof(GamesStorageProvider.InMemory));

        builder.Services.AddSingleton<IGamesStorageProviderFactory, GamesStorageProviderFactory>();

        builder.Services.AddSingleton<IBattleshipsService, BattleshipsService>();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        return builder;
    }
}