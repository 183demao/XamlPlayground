namespace AyatoPack;

using System;
using System.Threading;

public static class AyatoHost
{
    public static void Run<TApplication, TMainWindow>(string[] args, Action<IHostBuilder> configHostBuilder)
        where TApplication : AyatoApp
        where TMainWindow : System.Windows.Window
    {
        var hostBuilder = Host.CreateDefaultBuilder(args);

        hostBuilder.ConfigureServices(services =>
        {
            services.AddSingleton<TApplication>();
            services.AddTransient<TMainWindow>();
            services.AddHostedService<AppStartupService<TApplication, TMainWindow>>();
        });

        configHostBuilder?.Invoke(hostBuilder);

        if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
        {
            Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
        }

        hostBuilder.Build().Run();
    }
}
