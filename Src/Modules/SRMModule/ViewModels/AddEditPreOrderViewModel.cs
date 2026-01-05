
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.SRM.Views;
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
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class AddEditPreOrderViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null)
            ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl
            : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private bool isEnabled;
        private bool isSave;
        private bool isNew;
        private string informationError;
        List<Warehouses> warehousesList;
        Warehouses selectedWarehouse;
        DateTime preOrderDate;
        List<LookupValue> logisticList;
        LookupValue selectedLogistic;
        int selectedCurrencyIndex;
        List<Currency> currencyList;
        Currency selectedCurrency;
        string observationText;
        string windowHeader;
        int selectedLogisticIndex;
        #endregion

        #region Properties
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; OnPropertyChanged(new PropertyChangedEventArgs("IsEnabled")); }
        }
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
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
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
            }
        }
        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; OnPropertyChanged(new PropertyChangedEventArgs("IsNew")); }
        }
        public List<Currency> CurrencyList
        {
            get
            {
                return currencyList;
            }

            set
            {
                currencyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyList"));
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
        public List<Warehouses> WarehousesList
        {
            get
            {
                return warehousesList;
            }

            set
            {
                warehousesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehousesList"));
            }
        }
        public Warehouses SelectedWarehouse
        {
            get
            {
                return selectedWarehouse;
            }

            set
            {
                selectedWarehouse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWarehouse"));
            }
        }
        public LookupValue SelectedLogistic
        {
            get
            {
                return selectedLogistic;
            }

            set
            {
                selectedLogistic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLogistic"));
            }
        }
        public List<LookupValue> LogisticList
        {
            get
            {
                return logisticList;
            }

            set
            {
                logisticList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LogisticList"));
            }
        }
        public DateTime PreOrderDate
        {
            get
            {
                return preOrderDate;
            }

            set
            {
                preOrderDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreOrderDate"));
            }
        }
        public int SelectedCurrencyIndex
        {
            get
            {
                return selectedCurrencyIndex;
            }

            set
            {
                selectedCurrencyIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrencyIndex"));
            }
        }

        public int SelectedLogisticIndex
        {
            get
            {
                return selectedLogisticIndex;
            }

            set
            {
                selectedLogisticIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLogisticIndex"));
            }
        }
        public string ObservationText
        {
            get
            {
                return observationText;
            }

            set
            {
                observationText = value?.Trim();
                OnPropertyChanged(new PropertyChangedEventArgs("ObservationText"));
            }
        }
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

        public void Dispose()
        {
        }
        #endregion

        #region Commands
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AddEditPreOrderViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditPreOrderViewModel...", category: Category.Info, priority: Priority.Low);
               
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptAction));
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                GeosApplication.Instance.Logger.Log("Constructor AddEditPreOrderViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in AddEditPreOrderViewModel() Constructor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                PreOrderDate = DateTime.Now;
                GetCurrencies();
                FillLogistic();
                FillWarehouse();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
 
        //[rdixit][GEOS2-8252][31.10.2025]
        public void EditInit(PreOrder SelectedReOrder)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                PreOrderDate = DateTime.Now;
                GetCurrencies();
                FillLogistic();
                FillWarehouse();
                SelectedCurrency = SelectedReOrder.Currency;
                selectedLogisticIndex = LogisticList.FindIndex(i => i.IdLookupValue == SelectedReOrder.Logistic.IdLookupValue);
                ObservationText = SelectedReOrder.Observation;
                SelectedWarehouse = SelectedReOrder.Warehouse;
                PreOrderDate = SelectedReOrder.CreationDate;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetCurrencies()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCurrencies()...", category: Category.Info, priority: Priority.Low);
          
                var currencyList = new List<Currency>(CrmStartUp.GetCurrencies_V2580());
              
                if (currencyList != null)
                {
                    foreach (var bpItem in currencyList.GroupBy(tpa => tpa.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                        bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage);
                    }
                }
                currencyList.Insert(0, new Currency() { Name = "---", IdCurrency = 0 });
                var warehouseCurrency = SRMCommon.Instance.Selectedwarehouse != null ? SRMCommon.Instance.Selectedwarehouse?.Currency?.IdCurrency : (byte)0;
                CurrencyList = currencyList.Where(i => i.IdCurrency == warehouseCurrency).ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i=>i.IdCurrency == warehouseCurrency);              
                GeosApplication.Instance.Logger.Log("Method GetCurrencies()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetCurrencies() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetCurrencies() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetCurrencies() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillLogistic()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLogistic()...", category: Category.Info, priority: Priority.Low);
                LogisticList = new List<LookupValue>(CrmStartUp.GetLookupValues(101));
                LogisticList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });
                SelectedLogistic = LogisticList.FirstOrDefault();
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogistic()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogistic()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogistic()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillWarehouse()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLogistic()...", category: Category.Info, priority: Priority.Low);
                WarehousesList = SRMCommon.Instance.WarehouseList.Select(i => (Warehouses)i.Clone()).ToList();
                SelectedWarehouse = WarehousesList.FirstOrDefault(i => i.IdWarehouse == SRMCommon.Instance.Selectedwarehouse.IdWarehouse);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogistic()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogistic()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogistic()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        private void AcceptAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAcceptAction()...", category: Category.Info, priority: Priority.Low);

                InformationError = null;

                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCurrencyIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLogisticIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("ObservationText"));
                if (error != null)
                {
                    return;
                }

                PreOrder preOrder = new PreOrder();
                preOrder.CreationDate = PreOrderDate;
                preOrder.Warehouse = SelectedWarehouse;
                preOrder.Logistic = SelectedLogistic;
                preOrder.Currency = SelectedCurrency;
                preOrder.Observation = ObservationText;
                preOrder.CreatedId = GeosApplication.Instance.ActiveUser.IdUser;
                preOrder.Currency.CurrencyIconImage = null;

                preOrder.Code = GeneratePreOrderCodeAsync(PreOrderDate, SelectedWarehouse.Name);
                if(string.IsNullOrEmpty(preOrder.Code))
                {
                    return;
                }
                if ((!string.IsNullOrEmpty(preOrder.Code)) && preOrder.Warehouse != null && preOrder.Logistic != null && preOrder.Currency != null)
                {
                    ShowProcessing();
                    preOrder = SRMService.AddPreOrder_V2680(preOrder);
                    if (preOrder.IdWarehouseReOrder > 0)
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("AddPreOrderSuccess").ToString(), preOrder.Code), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }                    
                }

                IsSave = true;
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AcceptAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }


        private string GeneratePreOrderCodeAsync(DateTime preOrderDate, string warehouseCode)
        {
            string finalCode = string.Empty;
            try
            {
                string baseCode = $"{warehouseCode}{preOrderDate:yyyyMMdd}";
                // Fetch all existing PreOrder codes for this base
                List<string> existingCodes = SRMService.GetExistingPreOrderCodes_V2680(baseCode);

                // CASE 1: No existing preorder for this day → safe to use base code
                if (existingCodes == null || existingCodes.Count == 0)
                    return baseCode;

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["PreOrderWarning"].ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.No)
                    return null;


                int nextIndex = 2;

                var suffixNumbers = existingCodes
                    .Where(c => c.StartsWith(baseCode)).Select(c =>
                    {
                        var parts = c.Split('.');
                        return parts.Length > 1 && int.TryParse(parts[1], out int n) ? n : 1;
                    }).ToList();

                if (suffixNumbers.Count > 0)
                    nextIndex = suffixNumbers.Max() + 1;

                finalCode = $"{baseCode}.{nextIndex:D2}";
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method GeneratePreOrderCodeAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return finalCode;
        }
        public void ShowProcessing()
        {
            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = ResizeMode.NoResize,
                        AllowsTransparency = true,
                        Background = new SolidColorBrush(Colors.Transparent),
                        ShowInTaskbar = false,
                        Topmost = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    };
                    DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
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
                    me[BindableBase.GetPropertyName(() => SelectedCurrencyIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedLogisticIndex)] +
                    me[BindableBase.GetPropertyName(() => ObservationText)];

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
                string selectedCurrencyIndex = GetPropertyName(() => SelectedCurrencyIndex);
                string observationText = GetPropertyName(() => ObservationText);
                string selectedLogistic = GetPropertyName(() => SelectedLogisticIndex);

                if (columnName == selectedCurrencyIndex)
                    return PreOrderValidation.GetErrorMessage(selectedCurrencyIndex, SelectedCurrencyIndex);
                else if (columnName == observationText)
                    return PreOrderValidation.GetErrorMessage(observationText, ObservationText);
                else if (columnName == selectedLogistic)
                    return PreOrderValidation.GetErrorMessage(selectedLogistic, SelectedLogisticIndex);

                return null;
            }
        }

        #endregion
    }
}
