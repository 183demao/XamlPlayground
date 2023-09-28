namespace AyatoAppDemo;

using AyatoPack;
using System;
using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosed(EventArgs e) => AyatoApp.Current.Shutdown();
}
