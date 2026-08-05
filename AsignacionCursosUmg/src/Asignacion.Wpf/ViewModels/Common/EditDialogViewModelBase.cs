using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Common;

public abstract partial class EditDialogViewModelBase : ObservableObject, IEditDialogViewModel
{
    public event Action<bool>? SolicitarCierre;

    public abstract string TituloDialogo { get; }

    [ObservableProperty]
    private string? mensajeError;

    [RelayCommand]
    private async Task GuardarAsync()
    {
        MensajeError = null;
        try
        {
            if (!Validar(out var error))
            {
                MensajeError = error;
                return;
            }

            await GuardarInternoAsync();
            SolicitarCierre?.Invoke(true);
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    [RelayCommand]
    private void Cancelar()
    {
        SolicitarCierre?.Invoke(false);
    }

    protected virtual bool Validar(out string? error)
    {
        error = null;
        return true;
    }

    protected abstract Task GuardarInternoAsync();
}
