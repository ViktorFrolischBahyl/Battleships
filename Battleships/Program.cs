using Battleships.Models.Settings;
using Battleships.Services;
using Battleships.Services.Interfaces;
using NLog;
using NLog.Web;

namespace Battleships;

public class Program
{
    // TODO > Add more tests
    // TODO > Add logging
    // TODO > Add error handling + try catch

    public static void Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("Battleships main inicialization started.");

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
            logger.Error(ex, "Battleships main inicialization ended with error.");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    public static WebApplicationBuilder CreateHostBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection(nameof(ApplicationSettings)));

        builder.Services.AddSingleton<IBattleshipsService, BattleshipsService>();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        return builder;
    }
}