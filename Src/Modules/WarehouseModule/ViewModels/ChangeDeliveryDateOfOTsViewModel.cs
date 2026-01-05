using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class ChangeDeliveryDateOfOTsViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region  Task Log
        //[pramod.misal][GEOS2-5094][19-12-2023] 
        //|Display a new popup informing the user to change the delivery dates of the OTS with “Todo” Status in the list|
        #endregion

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        #endregion

        #region Declaration
        private bool isSave;
        private string windowHeader;
        private string error = string.Empty;
        ObservableCollection<Ots> todoOtList;
        List<Ots> updatedTodoOtList;
        DateTime deliveryDate;
        DateTime selecteddeliveryDate;
        bool isUpdated;
        #endregion

        #region Properties

        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value; OnPropertyChanged(new PropertyChangedEventArgs("IsSave")); }
        }

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

        public ObservableCollection<Ots> TodoOtList
        {
            get { return todoOtList; }
            set
            {
                todoOtList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TodoOtList"));
            }
        }

        //public List<Ots> UpdatedTodoOtList
        //{
        //    get { return updatedTodoOtList; }
        //    set
        //    {
        //        updatedTodoOtList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("UpdatedTodoOtList"));
        //    }
        //}

        public DateTime DeliveryDate
        {
            get { return deliveryDate; }

            set
            {
                deliveryDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeliveryDate"));
            }
        }

        public DateTime SelecteddeliveryDate
        {
            get { return selecteddeliveryDate; }

            set
            {
                selecteddeliveryDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelecteddeliveryDate"));
            }
        }
        #endregion

        #region Command

        public ICommand ChangeDeliveryDateOfOTsViewAcceptButtonCommand { get; set; }

        public ICommand ChangeDeliveryDateOfOTsViewCancelCommand { get; set; }

        public ICommand OnDateEditValueChangingCommand { get; set; }
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

        public ChangeDeliveryDateOfOTsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ChangeDeliveryDateOfOTsViewModel()...", category: Category.Info, priority: Priority.Low);

                ChangeDeliveryDateOfOTsViewCancelCommand = new RelayCommand(new Action<object>(CloseWindow));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                ChangeDeliveryDateOfOTsViewAcceptButtonCommand = new RelayCommand(new Action<object>(ChangeDeliveryDateOfOTs));
                GeosApplication.Instance.Logger.Log("Constructor ChangeDeliveryDateOfOTsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ChangeDeliveryDateOfOTsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
 
        #region Command Action

        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
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
                    me[BindableBase.GetPropertyName(() => DeliveryDate)];
               

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
                string OtsDeliveryDate = BindableBase.GetPropertyName(() => DeliveryDate);

                if (columnName == OtsDeliveryDate)
                    return DeliveryDateOfOTsValidation.GetErrorMessage(OtsDeliveryDate, DeliveryDate);

                return null;
            }
        }
        #endregion

        #region Method

        public void Init(List<Ots> SendEmailWorkOrderOtsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("ChangeDeliveryDateOfOTsViewTitle").ToString();
                DeliveryDate = DateTime.Now.AddDays(1);

                if (SendEmailWorkOrderOtsList != null)
                {
                    TodoOtList = new ObservableCollection<Ots>(
                        SendEmailWorkOrderOtsList
                          .Where(item => string.Equals(item.WorkflowStatus.Name, "To Do", StringComparison.OrdinalIgnoreCase))
                          );
                }


                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }


        private void OnDateEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                SelecteddeliveryDate = Convert.ToDateTime(obj.NewValue);
                DeliveryDate = SelecteddeliveryDate;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("DeliveryDate"));
                if (error != null)
                {
                    return;
                }

               
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeDeliveryDateOfOTs(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ChangeDeliveryDateOfOTs()...", category: Category.Info, priority: Priority.Low);
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("DeliveryDate"));

                 if (error != null)
                {
                    return;
                }
                else
                {
                    if (DeliveryDate != null && DeliveryDate <= DateTime.Now)
                        return;  
                }

                if (SelecteddeliveryDate != null)
                {
                    TodoOtList.ToList().ForEach(a => a.DeliveryDate = SelecteddeliveryDate);
                }

                Dictionary<Int64, DateTime? > dateIdDictionary = new Dictionary<Int64, DateTime?>();

                if (SelecteddeliveryDate != null)
                {                 
                    foreach (var todoOt in TodoOtList)
                    {
                        dateIdDictionary.Add(todoOt.IdOT,todoOt.DeliveryDate);
                    }
                }
                bool update = WarehouseService.UpdateDeliveryDateofTodoOts_V2470(dateIdDictionary);

                if (update == true)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeliveryDateUpdated").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("ChangeDeliveryDateOfOTs()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChangeDeliveryDateOfOTs()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
