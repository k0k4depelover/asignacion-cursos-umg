using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Asignacion.Wpf.Infrastructure;

/// <summary>
/// Dictionary-based ViewModel-first navigation: resolves the target ViewModel from the
/// DI container and exposes it for a ContentControl to render via DataTemplate matching.
/// Registered Transient so each Shell ViewModel gets its own independent instance.
/// </summary>
public partial class NavigationService(IServiceProvider provider) : ObservableObject, INavigationService
{
    [ObservableProperty]
    private object? currentViewModel;

    public void NavigateTo<TViewModel>() where TViewModel : notnull
    {
        CurrentViewModel = provider.GetRequiredService<TViewModel>();
    }
}
