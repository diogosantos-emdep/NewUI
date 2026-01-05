using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;


namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class EditWarehouseQuotaViewModel: NavigationViewModelBase, IDisposable, IDataErrorInfo, INotifyPropertyChanged
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declartion 
        ObservableCollection<int> yearList;
        //private int selectedCurrency;
        private string windowHeader;
        private double currencyConversionRate;
        private double selectedTargetAmount;
        private List<Currency> currencies;
        private Currency editSelectedCurrency;
        private int year;
        private UInt32 editCurrency;
        private int editYear;
        private double editargetAmount;
        DateTime? exchangeRateDate;
        private Currency selectedCurrency;
        private WarehouseQuota selectedAmount;
        private List<WarehouseQuota> warehouseQuotalist;
        UInt32 selectedtargetIdCurrency;

        //[pramod.misal][GEOS2-4529][30/05/2023]
       
        bool isSaved; 
        private Visibility isNew;
        private Visibility isexist;
        private Visibility isadd;
        string informationError;
        //List<int> addyearsList;



        #endregion

        #region Public Properties
        public int WarehouseId { get; set; }

        public List<int> AddyearsList { get; set; }

        public int Year
        {
            get
            {
                return year;
            }

            set
            {
                year = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Year"));
            }
        }


        public int EditYear
        {
            get
            {
                return editYear;
            }

            set
            {
                editYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditYear"));
            }
        }

        public double EditargetAmount
        {
            get
            {
                return editargetAmount;
            }

            set
            {
                editargetAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditargetAmount"));
            }
        }

        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
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

        public bool IsUpdated { get; set; }

        public bool Isadd { get; set; }
        



        public Currency EditSelectedCurrency
        {
            get
            {
                return editSelectedCurrency;
            }

            set
            {
                editSelectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditSelectedCurrency"));
            }
        }


        public uint IdWarehouse { get; set; }


        public ObservableCollection<int> YearList
        {
            get { return yearList; }
            set
            {
                yearList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("YearList"));
            }
        }


        public double SelectedTargetAmount
        {
            get
            {
                return selectedTargetAmount;
            }

            set
            {
                selectedTargetAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTargetAmount"));
            }
        }

    

        //[pramod.misal][GEOS2-4529][30/05/2023]

        public Visibility IsNew
        {
            get { return isNew; }
            set { isNew = value; OnPropertyChanged(new PropertyChangedEventArgs("IsNew")); }
        }
        //[pramod.misal][GEOS2-4529][30/05/2023]


        public Visibility Isexist
        {
            get { return isexist; }
            set { isexist = value; OnPropertyChanged(new PropertyChangedEventArgs("Isexist")); }
        }

        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }


        #endregion

        #region ICommands

        public ICommand EditWarehouseQuotaViewCancelButtonCommand { get; set; }
        public ICommand EditWarehouseQuotaViewAcceptButtonCommand { get; set; }
        public ICommand EditWarehouseQuotaCancelCommand { get; set; }

        #endregion

        #region Constructor

        public EditWarehouseQuotaViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditWarehouseQuotaViewModel ...", category: Category.Info, priority: Priority.Low);
                
                EditWarehouseQuotaViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                EditWarehouseQuotaViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(EditWarehouseQuotaInformation);
                EditWarehouseQuotaCancelCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                //CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                //EditYear = selectedRecord.Year;
               
                GeosApplication.Instance.Logger.Log("Constructor EditWarehouseQuotaViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

               
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditWarehouseQuotaViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void ShortcutAction(KeyEventArgs obj)
        //{

        //    GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
        //    try
        //    {
        //        if (GeosApplication.Instance.IsPermissionReadOnly)
        //        {
        //            return;
        //        }
        //        //CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

        //        GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
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

                    me[BindableBase.GetPropertyName(() => EditargetAmount)];
                   

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
               
                string amountProp = BindableBase.GetPropertyName(() => EditargetAmount);
                
              
                    return RequiredValidationRule.GetErrorMessage(amountProp, EditargetAmount);

                //return null;
            }
        }

        #endregion

        #region public Events
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

        #region Methods

  
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

    
        public void Dispose()
        {
            
        }

        public void InIt(int MaxYear, WarehouseQuota selectedRecord)
        {
            GeosApplication.Instance.Logger.Log("EditWarehouseQuotaViewModel Method InIt ...", category: Category.Info, priority: Priority.Low);

            try
            {
                selectedAmount = selectedRecord;
                EditargetAmount= selectedAmount.TargetAmount;

                EditYear = selectedAmount.Year;
                EditSelectedCurrency = new Currency();
                EditSelectedCurrency = GeosApplication.Instance.Currencies.ToList().FirstOrDefault(i => i.IdCurrency == selectedAmount.TargetIdCurrency);
                YearList = new ObservableCollection<int>();

                for (int i = MaxYear; i <= DateTime.Now.Year; i++)
                {
                    YearList.Add(i);
                }

               

            }
            catch (Exception ex)
            {

                
            }
   
        }

        private void EditWarehouseQuotaInformation(object obj)
        {

            InformationError = null;
            allowValidation = true;
            string error = EnableValidationAndGetError();
            PropertyChanged(this, new PropertyChangedEventArgs("EditargetAmount"));
            if (string.IsNullOrEmpty(error))
                InformationError = null;
            else
                InformationError = "";

            if (error != null)
            {
                return;
            }

            Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;


            WarehouseId = Convert.ToInt32(warehouse.IdWarehouse);
            WarehouseQuota selectedAmount1 = new WarehouseQuota();
            if (Isadd)
            {

                selectedAmount1.TargetAmount = EditargetAmount;
                selectedAmount1.TargetIdCurrency = EditSelectedCurrency.IdCurrency;
                selectedAmount1.IdWarehouse =Convert.ToUInt32(WarehouseId);
                selectedAmount1.Year = EditYear;

            }

            else
            {
                //WarehouseQuota selectedAmount1 = new WarehouseQuota();
                selectedAmount1.TargetAmount = EditargetAmount;
                selectedAmount1.TargetIdCurrency = EditSelectedCurrency.IdCurrency;
                selectedAmount1.IdWarehouse = selectedAmount.IdWarehouse;
                selectedAmount1.Year = selectedAmount.Year;

            }

            try
            {
                
                IsUpdated = WarehouseService.UpdateWarehouseQuota_V2400(selectedAmount1);

                if (IsUpdated && Isadd)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddWarehouseQuotaViewSuccessMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else if (IsUpdated && Isadd)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddWarehouseQuotaViewFailMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }



                if (IsUpdated && Isadd == false)
                {
                   
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditWarehouseQuotaViewSuccessMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else if (IsUpdated && Isadd==false)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditWarehouseQuotaViewFailMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
          
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditWarehouseQuotaInformation() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditWarehouseQuotaInformation() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditWarehouseQuotaInformation() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            RequestClose(null, null);
        }


        //[pramod.misal][GEOS2-4529][30/05/2023]
        public void AddInit()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddInit()...", category: Category.Info, priority: Priority.Low);

                Isadd = true;
                IsNew = Visibility.Visible;

                int currentYear = DateTime.Now.Year;
                AddyearsList = new List<int>();

                for (int year = 2017; year <= currentYear; year++)
                {
                    AddyearsList.Add(year);
                }

                EditYear = 2017;

                EditSelectedCurrency = new Currency();
                EditSelectedCurrency = GeosApplication.Instance.Currencies.ToList().FirstOrDefault();


                GeosApplication.Instance.Logger.Log("Method AddInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddInit() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddInit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion


    }
}
