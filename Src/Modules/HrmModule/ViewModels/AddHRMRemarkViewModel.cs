using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.UI.CustomControls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using DevExpress.Xpf.Core;
using System.ServiceModel;
using System.Windows;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common;
using Prism.Logging;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    class AddHRMRemarkViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private string windowHeader;
        private bool isNormal = true;
        private string oldItemComment;
        private string newItemComment;
        //private SAMLogEntries commentsList;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        bool isSave;
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public string NewItemComment
        {
            get { return newItemComment; }
            set
            {
                newItemComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewItemComment"));
            }
        }
        #endregion

        #region Command  
        public ICommand CloseWindowCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        #endregion

        #region Constructor 

        public AddHRMRemarkViewModel()
        {

            CloseWindowCommand = new DelegateCommand<object>(CloseWindowAction);
            AcceptButtonCommand = new DelegateCommand<object>(AcceptWindowAction);
        }
        #endregion

        #region Methods
        private void CloseWindowAction(object obj)
        {
            try
            {
                IsSave = false;
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindowAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptWindowAction(object obj)      //[rdixit][GEOS2-6979][02.04.2025]
        {
            try
            {
                IsSave = true;
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindowAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
        }
        #endregion
    }
}
