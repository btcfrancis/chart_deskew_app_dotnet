using Microsoft.Extensions.DependencyInjection;

using ChartDeskewApp.Core.Interfaces;
using ChartDeskewApp.Core.Services;
using ChartDeskewApp.Core.Algorithms;
using ChartDeskewApp.UI.Forms;

namespace ChartDeskewApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Set up dependency injection
        var services = ConfigureServices();
        var serviceProvider = services.BuildServiceProvider();
        var mainForm = serviceProvider.GetRequiredService<MainForm>();

        Application.Run(mainForm);
    }

    private static ServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register interfaces and implementations
        services.AddSingleton<IImageProcessor, ImageProcessor>();
        services.AddSingleton<IChartAnalyzer, ChartAnalyzer>();
        services.AddSingleton<ICircleDetector, CircleDetectionAlgorithm>();
        services.AddSingleton<IDeskewAlgorithm, DeskewAlgorithm>();
        services.AddSingleton<IGeometryHelper, GeometryHelper>();

        // Register main form
        services.AddTransient<MainForm>();

        return services;
    }
}