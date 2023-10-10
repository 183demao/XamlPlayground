namespace AyatoAppDemo;

using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IHostApplicationLifetime applicationLifetime)
    {
        InitializeComponent();
        this.applicationLifetime = applicationLifetime;
    }

    private readonly IHostApplicationLifetime applicationLifetime;

    protected override void OnClosed(EventArgs e) => applicationLifetime.StopApplication();
}
