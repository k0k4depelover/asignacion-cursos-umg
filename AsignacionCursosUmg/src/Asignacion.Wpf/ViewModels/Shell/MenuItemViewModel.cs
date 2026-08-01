using System.Windows.Input;

namespace Asignacion.Wpf.ViewModels.Shell;

public class MenuItemViewModel(string titulo, ICommand comando)
{
    public string Titulo { get; } = titulo;
    public ICommand Comando { get; } = comando;
}
