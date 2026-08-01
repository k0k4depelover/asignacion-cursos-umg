using System.ComponentModel;

namespace Asignacion.Wpf.Infrastructure;

public interface INavigationService : INotifyPropertyChanged
{
    object? CurrentViewModel { get; }
    void NavigateTo<TViewModel>() where TViewModel : notnull;
}
