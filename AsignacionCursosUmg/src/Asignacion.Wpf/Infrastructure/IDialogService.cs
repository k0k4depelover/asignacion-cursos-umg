using System.Windows;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.Infrastructure;

public interface IDialogService
{
    /// <summary>Hosts the given ViewModel behind its DataTemplate-matched edit view as a
    /// modal dialog. Returns true if the user saved, false if cancelled/closed.</summary>
    bool ShowEditDialog(IEditDialogViewModel viewModel);

    void MostrarMensaje(string mensaje, string titulo = "Aviso");

    bool Confirmar(string mensaje, string titulo = "Confirmar");
}

public class DialogService : IDialogService
{
    public bool ShowEditDialog(IEditDialogViewModel viewModel)
    {
        var resultado = false;

        void Handler(bool guardado) => resultado = guardado;
        viewModel.SolicitarCierre += Handler;

        var window = new Window
        {
            Title = viewModel.TituloDialogo,
            Content = viewModel,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ResizeMode = ResizeMode.NoResize,
            Owner = Application.Current.MainWindow
        };

        void CloseHandler(bool guardado)
        {
            resultado = guardado;
            window.Close();
        }

        viewModel.SolicitarCierre -= Handler;
        viewModel.SolicitarCierre += CloseHandler;

        window.ShowDialog();

        viewModel.SolicitarCierre -= CloseHandler;
        return resultado;
    }

    public void MostrarMensaje(string mensaje, string titulo = "Aviso")
    {
        MessageBox.Show(Application.Current.MainWindow ?? default, mensaje, titulo, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public bool Confirmar(string mensaje, string titulo = "Confirmar")
    {
        var resultado = MessageBox.Show(Application.Current.MainWindow ?? default, mensaje, titulo, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return resultado == MessageBoxResult.Yes;
    }
}
