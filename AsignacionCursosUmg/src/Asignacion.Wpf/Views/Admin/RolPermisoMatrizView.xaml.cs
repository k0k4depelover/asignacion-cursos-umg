using System.Windows.Controls;
using System.Windows.Data;
using Asignacion.Wpf.ViewModels.Admin;

namespace Asignacion.Wpf.Views.Admin;

public partial class RolPermisoMatrizView : UserControl
{
    public RolPermisoMatrizView()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => Reconstruir();
    }

    private void Reconstruir()
    {
        if (DataContext is not RolPermisoMatrizViewModel viewModel)
        {
            return;
        }

        viewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(RolPermisoMatrizViewModel.Roles))
            {
                ReconstruirColumnas(viewModel);
            }
        };

        ReconstruirColumnas(viewModel);
    }

    private void ReconstruirColumnas(RolPermisoMatrizViewModel viewModel)
    {
        while (Matriz.Columns.Count > 1)
        {
            Matriz.Columns.RemoveAt(1);
        }

        for (var i = 0; i < viewModel.Roles.Count; i++)
        {
            var columna = new DataGridCheckBoxColumn
            {
                Header = viewModel.Roles[i].Nombre,
                Binding = new Binding($"Celdas[{i}].Asignado") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                Width = 110
            };
            Matriz.Columns.Add(columna);
        }
    }
}
