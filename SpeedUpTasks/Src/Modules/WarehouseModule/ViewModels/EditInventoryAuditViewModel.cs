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
    public class EditInventoryAuditViewModel: IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region  Task Log
        //[000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        #endregion

        #region Declaration
        //[000] added
        private string windowHeader;
        private string name;
        private string error = string.Empty;
        private DateTime? startDate;
        private DateTime? endDate;
        private DateTime? minEndDate;
        private bool isUpdate;
        private WarehouseInventoryAudit updateWarehouseInventoryAuditDetails;
        //end

        #endregion

        #region Properties
        //[000] added
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
        public bool IsUpdate
        {
            get { return isUpdate; }
            set { isUpdate = value; OnPropertyChanged(new PropertyChangedEventArgs("IsUpdate")); }
        }
        public WarehouseInventoryAudit UpdateWarehouseInventoryAuditDetails
        {
            get { return updateWarehouseInventoryAuditDetails; }
            set
            {
                updateWarehouseInventoryAuditDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateWarehouseInventoryAuditDetails"));

            }
        }
        //end
        #endregion

        #region Command
        //[000] added
        public ICommand EditInventoryAuditViewCancelCommand { get; set; }
        public ICommand EditInventoryAuditViewAcceptButtonCommand { get; set; }
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

        #region  Constructor
        public EditInventoryAuditViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditInventoryAuditViewModel()...", category: Category.Info, priority: Priority.Low);

                EditInventoryAuditViewCancelCommand = new RelayCommand(new Action<object>(CloseWindow));

                EditInventoryAuditViewAcceptButtonCommand = new RelayCommand(new Action<object>(EditInventoryAudit));

                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);

                GeosApplication.Instance.Logger.Log("Constructor EditInventoryAuditViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EditInventoryAuditViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Command Action
        private void CloseWindow(object obj)
        {
            IsUpdate = false;
            RequestClose(null, null);
        }
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void EditInventoryAudit(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("EditInventoryAudit()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Name")); //[01] added
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
                if (UpdateWarehouseInventoryAuditDetails.StartDate == StartDate && UpdateWarehouseInventoryAuditDetails.Name == Name && UpdateWarehouseInventoryAuditDetails.EndDate == EndDate)
                {
                    IsUpdate = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateInventoryAuditSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                    return;
                }

                UpdateWarehouseInventoryAuditDetails.StartDate = StartDate;
                UpdateWarehouseInventoryAuditDetails.Name = Name;
                UpdateWarehouseInventoryAuditDetails.EndDate = endDate;
                UpdateWarehouseInventoryAuditDetails.IdModifier = GeosApplication.Instance.ActiveUser.IdUser;

                if (WarehouseService.UpdateWarehouseInventoryAudit(WarehouseCommon.Instance.Selectedwarehouse, UpdateWarehouseInventoryAuditDetails))
                {
                    IsUpdate = true;
                  CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateInventoryAuditSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
              
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("EditInventoryAudit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                IsUpdate = false;
                GeosApplication.Instance.Logger.Log("Get an error in EditInventoryAudit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        /// <param name="ExistwarehouseInventoryAuditDetails"></param>
        public void Init(WarehouseInventoryAudit ExistwarehouseInventoryAuditDetails)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("EditInventoryAudit").ToString();
                UpdateWarehouseInventoryAuditDetails = new WarehouseInventoryAudit();
                UpdateWarehouseInventoryAuditDetails = ExistwarehouseInventoryAuditDetails;

                StartDate = ExistwarehouseInventoryAuditDetails.StartDate;
                Name = ExistwarehouseInventoryAuditDetails.Name;
                EndDate = ExistwarehouseInventoryAuditDetails.EndDate;

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
