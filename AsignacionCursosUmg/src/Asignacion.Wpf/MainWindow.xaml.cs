using System.Windows;
using Asignacion.Wpf.ViewModels;

namespace Asignacion.Wpf;

public partial class MainWindow : Window
{
    public MainWindow(RootViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
