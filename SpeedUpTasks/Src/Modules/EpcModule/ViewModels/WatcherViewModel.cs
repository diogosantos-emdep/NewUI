using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
   public class WatcherViewModel:NavigationViewModelBase
    {
        #region Services
        IEpcService _epcserviceControl;
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        #endregion

        #region ICommands
        public ICommand PrintButtonCommand { get; set; }
        public ICommand GridDoubleClickCommand { get; set; }
        public ICommand HyperlinkToProjectCodeCommand { get; set; }
        #endregion

        #region Collections
        ObservableCollection<ProjectTask> taskWatcherList = new ObservableCollection<ProjectTask>();

        public ObservableCollection<ProjectTask> TaskWatcherList
        {
            get { return taskWatcherList; }
            set { SetProperty(ref taskWatcherList, value, () => TaskWatcherList); }
        
        }

       

        #endregion

        #region Constructor
        public WatcherViewModel()
        {
            _epcserviceControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
          
            PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
            GridDoubleClickCommand = new DelegateCommand<RowDoubleClickEventArgs>(TaskOpenAction);
            HyperlinkToProjectCodeCommand = new DelegateCommand<object>(new Action<object>((project) =>
            {
                ShowProjectView(project);
            }));
            Init();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method For Initialization
        /// </summary>
        public void Init()
        {
            LoadWatcherTasks();
           // Task.Run(() => { });
        }
        public void LoadWatcherTasks()
        {
            TaskWatcherList = new ObservableCollection<ProjectTask>(_epcserviceControl.GetOpenTaskWatchersByUserId(GeosApplication.Instance.ActiveUser.IdUser).ToList());           
        }

        public void TaskOpenAction(RowDoubleClickEventArgs e)
        {
            TableView detailView = e.HitInfo.Column.View as TableView;
            ProjectTask data = (ProjectTask)(detailView.DataControl as GridControl).GetRow(e.HitInfo.RowHandle);
            Service.Navigate("Emdep.Geos.Modules.Epc.Views.TaskDetailsView", data, Service.Current);
        }

        public void ShowProjectView(object ProjectTask)
        {
            Project data = (Project)ProjectTask;
            Service.Navigate("Emdep.Geos.Modules.Epc.Views.ProductView", data, this);
        }
        /// <summary>
        /// Method For Print Report
        /// </summary>
        /// <param name="obj"></param>
        public void PrintAction(object obj)
        {
            try
            {

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["WatcherViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["WatcherViewCustomPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                pcl.ShowPrintPreview(window);
                // pcl.ShowPrintPreview(Application.Current.MainWindow);
            }
            catch(Exception ex)
            {

            }
        }
        #endregion
    }
}
