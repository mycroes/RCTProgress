using System.Windows;
using System.Windows.Controls;
using RCTProgress.UI.ViewModels;

namespace RCTProgress.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        AppBootstrapper = new AppBootstrapper();
        DataContext = AppBootstrapper;
    }

    public AppBootstrapper AppBootstrapper { get; }
}
