using System.Windows;
using Asignacion.Wpf.ViewModels;

namespace Asignacion.Wpf.Views.Common;

public partial class ConexionSetupWindow : Window
{
    private readonly ConexionSetupViewModel _viewModel;

    public ConexionSetupWindow(ConexionSetupViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;

        PasswordBox.PasswordChanged += (_, _) => viewModel.Password = PasswordBox.Password;
        viewModel.SolicitarCierre += () => Close();
    }

    public bool GuardarYContinuar => _viewModel.GuardarYContinuar;
}
