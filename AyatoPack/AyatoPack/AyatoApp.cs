namespace AyatoPack;

using System;
using System.Windows;

public abstract class AyatoApp : Application
{
    public AyatoApp(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

    public static new AyatoApp Current => (AyatoApp)Application.Current;

    public static IServiceProvider ServiceProvider { get; private set; } = null!;
}
