using System.Windows.Controls;
using Asignacion.Wpf.ViewModels.Admin;

namespace Asignacion.Wpf.Views.Admin;

public partial class UsuarioEditView : UserControl
{
    public UsuarioEditView()
    {
        InitializeComponent();
    }

    private void PasswordBoxControl_OnPasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is UsuarioEditViewModel viewModel)
        {
            viewModel.Password = PasswordBoxControl.Password;
        }
    }
}
