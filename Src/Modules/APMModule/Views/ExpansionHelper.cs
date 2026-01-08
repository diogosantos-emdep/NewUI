using System.Windows;
using DevExpress.Xpf.Grid;

namespace Emdep.Geos.Modules.APM.Views // O mesmo namespace da tua View
{
    public static class ExpansionHelper
    {
        // Esta é a propriedade que vamos usar no XAML: "ExpansionHelper.IsExpanded"
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.RegisterAttached(
                "IsExpanded",
                typeof(bool),
                typeof(ExpansionHelper),
                new PropertyMetadata(false, OnIsExpandedChanged));

        // Getters e Setters obrigatórios
        public static bool GetIsExpanded(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsExpandedProperty);
        }

        public static void SetIsExpanded(DependencyObject obj, bool value)
        {
            obj.SetValue(IsExpandedProperty, value);
        }

        // Ocorre quando a propriedade muda (true/false) no ViewModel
        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RowControl rowControl && rowControl.DataContext is RowData rowData)
            {
                var view = rowData.View as TableView;
                if (view == null) return;

                var grid = view.Grid;
                var rowHandle = rowData.RowHandle.Value;
                bool shouldExpand = (bool)e.NewValue;

                // Verifica o estado atual para evitar chamadas repetidas
                bool isCurrentlyExpanded = grid.IsMasterRowExpanded(rowHandle);

                if (shouldExpand && !isCurrentlyExpanded)
                {
                    grid.ExpandMasterRow(rowHandle);
                }
                else if (!shouldExpand && isCurrentlyExpanded)
                {
                    grid.CollapseMasterRow(rowHandle);
                }
            }
        }
    }
}