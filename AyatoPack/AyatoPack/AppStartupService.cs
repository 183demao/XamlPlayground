namespace AyatoPack;

using System;
using System.Threading;
using System.Threading.Tasks;

internal class AppStartupService<TApplication, TMainWindow> : IHostedService, IRecipient<RequestApplicationShutdownMessage>
    where TApplication : AyatoApp
    where TMainWindow : System.Windows.Window
{
    public AppStartupService(TApplication application, TMainWindow mainWindow)
    {
        this.application = application;
        this.mainWindow = mainWindow;

        WeakReferenceMessenger.Default.Register(this);
    }

    private readonly TApplication application;
    private readonly TMainWindow mainWindow;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        application.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
        application.Run(mainWindow);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        application.Shutdown(Environment.ExitCode);
        return Task.CompletedTask;
    }

    public void Receive(RequestApplicationShutdownMessage message) => AyatoHost.CurrentHost.StopAsync();
}
