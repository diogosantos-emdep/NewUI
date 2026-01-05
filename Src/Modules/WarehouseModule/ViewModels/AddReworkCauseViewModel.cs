using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using Emdep.Geos.UI.Validations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using DevExpress.DataProcessing;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class AddReworkCauseViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        //[rdixit][GEOS2-5906][13.11.2024]
        #region Service

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        //IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController("localhost:6699");
        #endregion

        #region Declaration     
        private string name;
        private ProductInspectionReworkCauses newReworkCause;        
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
            set
            {
                if (value != null)
                {
                    name = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                }
            }
        }
        public ProductInspectionReworkCauses NewReworkCause
        {
            get { return newReworkCause; }
            set
            {
                if (value != null)
                {
                    newReworkCause = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("NewReworkCause"));
                }
            }
        }
        public bool IsSave { get; set; }
        #endregion
   
        #region ICommands

        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }

        #endregion

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

        #region Constructor
        public AddReworkCauseViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddReworkCauseViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AddAccept);
                CancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                NewReworkCause = new ProductInspectionReworkCauses();
                GeosApplication.Instance.Logger.Log("Constructor AddReworkCauseViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddReworkCauseViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void AddAccept(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" AddAccept() Method ...", category: Category.Info, priority: Priority.Low);

                if (!string.IsNullOrEmpty(Name))
                {                 
                    try
                    {                      
                        NewReworkCause.ReworkCause = Name;
                        NewReworkCause = WarehouseService.AddEditNewReworkCause_V2580(NewReworkCause);
                        IsSave = true;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in AddAccept() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    RequestClose(null, null);
                    GeosApplication.Instance.Logger.Log("Method AddAccept() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddAccept() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            IsSave = false;
            Name = string.Empty;
            RequestClose(null, null);
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
