using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.APM.ViewModels;
using Prism.Logging;
using System.Windows;
using System.Windows.Controls;
using System;
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
        public void RowDetails_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is FrameworkElement element)
                {
                    // --- ACTION PLAN EXPANSION ---
                    if (element.DataContext is EditGridCellData cellData && cellData.Row is ActionPlanModernDto actionPlan)
                    {
                        // Só carregar se necessário
                        if (ViewModel != null && actionPlan.IsExpanded && (actionPlan.Tasks == null || actionPlan.Tasks.Count == 0))
                        {
                            // CORREÇÃO: Usar Dispatcher para sair do ciclo de Layout atual
                            Dispatcher.BeginInvoke(new Action(async () =>
                            {
                                try
                                {
                                    await ViewModel.LoadTasksForActionPlanAsync(actionPlan);
                                }
                                catch (Exception ex)
                                {
                                    Emdep.Geos.UI.Common.GeosApplication.Instance.Logger?.Log($"Error loading Plan tasks: {ex.Message}", Category.Exception, Priority.High);
                                }
                            }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                        }
                    }
                    // --- TASK EXPANSION (SUB-TASKS) ---
                    else if (element.DataContext is ActionPlanTaskModernDto task)
                    {
                        // Só carregar se necessário
                        if (ViewModel != null && task.IsExpanded && (task.SubTasks == null || task.SubTasks.Count == 0))
                        {
                            // CORREÇÃO: Dispatcher aqui também
                            Dispatcher.BeginInvoke(new Action(async () =>
                            {
                                try
                                {
                                    await ViewModel.LoadSubTasksForTaskAsync(task);
                                }
                                catch (Exception ex)
                                {
                                    Emdep.Geos.UI.Common.GeosApplication.Instance.Logger?.Log($"Error loading SubTasks: {ex.Message}", Category.Exception, Priority.High);
                                }
                            }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Emdep.Geos.UI.Common.GeosApplication.Instance.Logger?.Log(
                    $"[RowDetails_Loaded] EXCEPTION: {ex.Message}",
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
