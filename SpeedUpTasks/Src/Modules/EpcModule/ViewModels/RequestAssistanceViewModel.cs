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
  public class RequestAssistanceViewModel:NavigationViewModelBase,IDisposable
    {
        #region Services
        IEpcService _epcserviceControl;
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        #endregion

        #region ICommand
        public ICommand PrintButtonCommand { get; set; }
        public ICommand GridDoubleClickCommand { get; set; }
        public ICommand HyperlinkToProjectCodeCommand { get; set; }
        #endregion

        #region Collections

        ObservableCollection<ProjectTask> _tasList = new ObservableCollection<ProjectTask>();      
        public ObservableCollection<ProjectTask> TaskList
        {
            get { return _tasList; }
            set { SetProperty(ref _tasList, value, () => TaskList); }
        }

        #endregion

        #region Constructor
        public RequestAssistanceViewModel()
        {
            _epcserviceControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            Init();
            PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
            GridDoubleClickCommand = new DelegateCommand<RowDoubleClickEventArgs>(TaskOpenAction);
            HyperlinkToProjectCodeCommand = new DelegateCommand<object>(new Action<object>((project) =>
            {
                ShowProjectView(project);
            }));
        
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method For Initialization
        /// </summary>
        public void Init()
        {
            Task.Run(() => { LoadRequestAssistanceTasks(); });
        }

        public void LoadRequestAssistanceTasks()
        {
           // TaskList = new ObservableCollection<ProjectTask>(_epcserviceControl.GetRequestAssistanceByRequestedFrom(GeosApplication.Instance.ActiveUser.IdUser).ToList());
            TaskList = new ObservableCollection<ProjectTask>(_epcserviceControl.GetRequestAssistanceByRequestedTo(GeosApplication.Instance.ActiveUser.IdUser).ToList());
            //TaskList[0].TaskPriority.IdImage
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
            PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
            pcl.Margins.Bottom = 5;
            pcl.Margins.Top = 5;
            pcl.Margins.Left = 5;
            pcl.Margins.Right = 5;
            pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["RequestAssistanceViewCustomPrintHeaderTemplate"];
            pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["RequestAssistanceViewCustomPrintFooterTemplate"];
            pcl.Landscape = true;
            pcl.CreateDocument(false);
            // pcl.ShowPrintPreview(Application.Current.MainWindow);
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            pcl.ShowPrintPreview(window);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
