using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.APM.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.Modules.APM.Views
{
    /// <summary>
    /// Interaction logic for ActionPlansModernView.xaml
    /// View MODERNA e OTIMIZADA para Action Plans
    /// Features: Virtualização, Async Paging, Lazy-Load Master/Detail
    /// </summary>
    public partial class ActionPlansModernView : UserControl
    {
        public ActionPlansModernView()
        {
            InitializeComponent();
            
            // DevExpress v19.2 não tem MasterRowExpanding
            // Solução: usar ShownEditor ou manipular via Loaded event do RowDetailsTemplate
            // Implementação via XAML com Loaded event
        }

        /// <summary>
        /// Chamado quando o RowDetailsTemplate é carregado (row expandida)
        /// Usar via XAML: Loaded="RowDetails_Loaded"
        /// </summary>
        public async void RowDetails_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                // Handle ActionPlan expansion
                if (element.DataContext is EditGridCellData cellData && cellData.Row is ActionPlanModernDto actionPlan)
                {
                    if (ViewModel != null && actionPlan.IsExpanded)
                    {
                        await ViewModel.LoadTasksForActionPlanAsync(actionPlan);
                    }
                }
                // Handle Task expansion (for sub-tasks)
                else if (element.DataContext is ActionPlanTaskModernDto task)
                {
                    if (ViewModel != null && task.IsExpanded)
                    {
                        await ViewModel.LoadSubTasksForTaskAsync(task);
                    }
                }
            }
        }

        /// <summary>
        /// Acesso ao ViewModel para configuração externa se necessário
        /// </summary>
        public ActionPlansModernViewModel ViewModel
        {
            get => DataContext as ActionPlansModernViewModel;
            set => DataContext = value;
        }
    }
}
