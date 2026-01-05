using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    //[001][kshinde][07/06/2022][GEOS2-3270]
    public class EditArticleComponentsViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }



        #endregion // End Of Events 

        #region ICommands
        public ICommand ImportWarehouseItemsToPCMCommand { get; set; }
        #endregion

        #region Constructor
        public EditArticleComponentsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditArticleComponentsViewModel ...", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor EditArticleComponentsViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
