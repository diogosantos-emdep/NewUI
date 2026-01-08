using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.APM.ViewModels;
using Prism.Logging;
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
            try
            {
                if (sender is FrameworkElement element)
                {
                    // Handle ActionPlan expansion
                    if (element.DataContext is EditGridCellData cellData && cellData.Row is ActionPlanModernDto actionPlan)
                    {
                        Emdep.Geos.UI.Common.GeosApplication.Instance.Logger?.Log(
                            $"[RowDetails_Loaded] ActionPlan={actionPlan.Code}, IsExpanded={actionPlan.IsExpanded}, TasksCount={actionPlan.TasksCount}",
                            Category.Info,
                            Priority.Low);

                        // SÓ carregar se IsExpanded=true (evitar carregamento prematuro quando Loaded é chamado ao renderizar a grid)
                        if (ViewModel != null && actionPlan.IsExpanded && (actionPlan.Tasks == null || actionPlan.Tasks.Count == 0))
                        {
                            await ViewModel.LoadTasksForActionPlanAsync(actionPlan);
                        }
                    }
                    // Handle Task expansion (for sub-tasks)
                    else if (element.DataContext is ActionPlanTaskModernDto task)
                    {
                        Emdep.Geos.UI.Common.GeosApplication.Instance.Logger?.Log(
                            $"[RowDetails_Loaded] Task={task.TaskNumber}, IsExpanded={task.IsExpanded}, TotalSubTasks={task.TotalSubTasks}",
                            Category.Info,
                            Priority.Low);

                        // SÓ carregar se IsExpanded=true
                        if (ViewModel != null && task.IsExpanded && (task.SubTasks == null || task.SubTasks.Count == 0))
                        {
                            await ViewModel.LoadSubTasksForTaskAsync(task);
                        }
                    }
                    else
                    {
                        // Carregamento prematuro (antes de expandir) - ignorar
                        Emdep.Geos.UI.Common.GeosApplication.Instance.Logger?.Log(
                            $"[RowDetails_Loaded] Ignored - DataContext type: {element.DataContext?.GetType().Name}, not expanded yet",
                            Category.Debug,
                            Priority.Low);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Emdep.Geos.UI.Common.GeosApplication.Instance.Logger?.Log(
                    $"[RowDetails_Loaded] EXCEPTION: {ex.Message}\n{ex.StackTrace}",
                    Category.Exception,
                    Priority.High);
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
