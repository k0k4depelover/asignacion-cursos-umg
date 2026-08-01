namespace Asignacion.Wpf.ViewModels.Common;

public interface IEditDialogViewModel
{
    /// <summary>Raised when the dialog should close; true = saved, false = cancelled.</summary>
    event Action<bool>? SolicitarCierre;

    string TituloDialogo { get; }
}
