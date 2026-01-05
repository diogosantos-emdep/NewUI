using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class AddNewInventoryAuditViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion


        #region  Task Log
        //[000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        #endregion

        #region Declaration
        //[000]added
        private string windowHeader;
        private string name;
        private string error = string.Empty;
        private DateTime? startDate;
        private DateTime? endDate;
        private DateTime ? minEndDate;
        private bool isSave;
        private WarehouseInventoryAudit warehouseInventoryAuditDetails;
        //end
        #endregion

        #region Properties
        //[000]added
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
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));

            }
        }

        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged(new PropertyChangedEventArgs("EndDate")); }
         
        }
        public DateTime? MinEndDate
        {
            get { return minEndDate; }
            set { minEndDate = value; OnPropertyChanged(new PropertyChangedEventArgs("MinEndDate")); }
        }
        public bool IsSave
        {
            get { return isSave; }
            set {  isSave = value;  OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));}
        }
        public WarehouseInventoryAudit WarehouseInventoryAuditDetails
        {
            get { return warehouseInventoryAuditDetails; }
            set
            {
                warehouseInventoryAuditDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseInventoryAuditDetails"));

            }
        }
        //end
        #endregion

        #region Command
       // [000]added
        public ICommand AddNewInventoryAuditViewCancelCommand { get; set; }
        public ICommand AddNewInventoryAuditViewAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
      //end
        
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
        public AddNewInventoryAuditViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddNewInventoryAuditsViewModel()...", category: Category.Info, priority: Priority.Low);

                AddNewInventoryAuditViewCancelCommand = new RelayCommand(new Action<object>(CloseWindow));

                AddNewInventoryAuditViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddInventoryAudit));

                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);

                GeosApplication.Instance.Logger.Log("Constructor AddNewInventoryAuditsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddNewInventoryAuditsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


        #region Command Action
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void AddInventoryAudit(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("AddInventoryAudit()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Name")); 
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));

                if (error != null)
                {
                    return;
                }
                else
                {
                    if (StartDate > EndDate)
                        return;
                }

                WarehouseInventoryAuditDetails = new WarehouseInventoryAudit()
                {
                    Name = Name,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    TotalItems = 0,
                    TotalLocations = 0,
                    OKItems = 0,
                    NOKItems = 0,
                    SuccessRate = 0,
                    BalanceAmount = 0,
                    IdCreator = GeosApplication.Instance.ActiveUser.IdUser,
                    IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse

                };

                WarehouseInventoryAuditDetails = WarehouseService.AddWarehouseInventoryAudit(WarehouseCommon.Instance.Selectedwarehouse, WarehouseInventoryAuditDetails);

                if (WarehouseInventoryAuditDetails.IdWarehouseInventoryAudit != 0)
                {
                    IsSave = true;

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddInventoryAuditSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("AddInventoryAudit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                IsSave = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddInventoryAudit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="e"></param>
        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                if (StartDate != null)
                    MinEndDate = StartDate;
                else
                    MinEndDate = null;

                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }


            return null;
        }


        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => Name)] +
                    me[BindableBase.GetPropertyName(() => StartDate)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string InventoryAuditName = BindableBase.GetPropertyName(() => Name);
                string inventoryAuditStartDate = BindableBase.GetPropertyName(() => StartDate);

                if (columnName == InventoryAuditName)
                    return WarehouseInventoryAuditValidation.GetErrorMessage(InventoryAuditName, Name);
                if (columnName == inventoryAuditStartDate)
                  return WarehouseInventoryAuditValidation.GetErrorMessage(inventoryAuditStartDate, StartDate);

                return null;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("AddNewInventoryAudit").ToString();
                StartDate = GeosApplication.Instance.ServerDateTime.Date;
                Name = String.Format("Audit_{0}", GeosApplication.Instance.ServerDateTime.ToString("yyyyMMddHHmmss"));
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
         
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
