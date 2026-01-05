using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.UI.Validations;
using System.Collections.ObjectModel;
using DevExpress.DataAccess.Native.ObjectBinding;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class PurchasingReportFilterViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {

        #region TaskLog
        // [nsatpute][21-01-2025][GEOS2-5725]
        #endregion


        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SrmService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SrmService = new SRMServiceController("localhost:6699");
        #endregion // Services

        #region ICommands
        public ICommand SalesReportAcceptButtonCommand { get; set; }
        public ICommand SalesReportCancelButtonCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; set; }
        public ICommand SelectedItemTemplateChangedCommand { get; set; }
        public ICommand OptionPopupClosedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }

        public ICommand ClearSelectedArticleCommand { get; set; }
        public ICommand ClearSelectedSupplierCommand { get; set; }
        #endregion // Commands

        #region Declaration
        ModuleFamily selectedFamily;
        ModuleSubFamily selectedSubFamily;
        ObservableCollection<Customer> groupList;
        List<Company> plantList;


        Int16 selectedIndexGroup;
        Int16 selectedIndexPlant;

        ObservableCollection<ModuleFamily> familyList;
        ObservableCollection<ModuleSubFamily> subFamilyList;
        Int16 selectedIndexTemplate;
        Int16 selectedIndexCpType;
        Int16 selectedIndexOption;
        private DateTime fromDate;
        private DateTime toDate;
        private ObservableCollection<Company> entireCompanyPlantList;
        private ObservableCollection<Warehouses> warehouseList;
        private Company selectedCompanyIndex;
        private ObservableCollection<Article> articleList;
        private Article selectedArticle;
        private ObservableCollection<ArticleSupplier> supplierList;
        public bool GenerateReport;
        private ArticleSupplier selectedSupplier;
        private bool isBusy;
        private Visibility supplierClearButtonVisibility;
        private Visibility articleClearButtonVisibility;

        #endregion // Declaration

        #region Properties



        public virtual List<object> SelectedItems { get; set; }
        public ObservableCollection<Warehouses> WarehouseList
        {
            get
            {
                return warehouseList;
            }
            set
            {
                warehouseList = value;
                OnPropertyChanged("WarehouseList");
            }
        }

        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set { entireCompanyPlantList = value; }
        }
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        public ObservableCollection<Customer> GroupList
        {
            get { return groupList; }
            set
            {
                groupList = value;
                OnPropertyChanged("GroupList");

            }
        }
        public ObservableCollection<Article> ArticleList
        {
            get { return articleList; }
            set
            {
                articleList = value;
                OnPropertyChanged("ArticleList");

            }
        }
        public Article SelectedArticle
        {
            get { return selectedArticle; }
            set
            {
                selectedArticle = value;
                OnPropertyChanged("SelectedArticle");
                if (SelectedArticle != null)
                    ArticleClearButtonVisibility = Visibility.Visible;
            }
        }
        public ObservableCollection<ArticleSupplier> SupplierList
        {
            get { return supplierList; }
            set
            {
                supplierList = value;
                OnPropertyChanged("SupplierList");

            }
        }
        public ArticleSupplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                selectedSupplier = value;
                OnPropertyChanged("SelectedSupplier");
                if (SelectedSupplier != null)
                    SupplierClearButtonVisibility = Visibility.Visible;
            }
        }

        public Visibility SupplierClearButtonVisibility
        {
            get { return supplierClearButtonVisibility; }
            set
            {
                supplierClearButtonVisibility = value;
                OnPropertyChanged("SupplierClearButtonVisibility");

            }
        }

        public Visibility ArticleClearButtonVisibility
        {
            get { return articleClearButtonVisibility; }
            set
            {
                articleClearButtonVisibility = value;
                OnPropertyChanged("ArticleClearButtonVisibility");

            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        #endregion // Properties

        #region Constructor

        public PurchasingReportFilterViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PurchasingReportFilterViewModel ...", category: Category.Info, priority: Priority.Low);
                Processing();
                SalesReportAcceptButtonCommand = new RelayCommand(new Action<object>(SalesReportAcceptAction));
                SalesReportCancelButtonCommand = new RelayCommand(new Action<object>(SalesReportCancelButtonCommandAction));
                ClearSelectedArticleCommand = new RelayCommand(new Action<object>(ClearSelectedArticleCommandAction));
                ClearSelectedSupplierCommand = new RelayCommand(new Action<object>(ClearSelectedSupplierCommandAction));
                FromDate = new DateTime(DateTime.Now.Year, 1, 1);
                ToDate = DateTime.Now.Date;
                SupplierList = new ObservableCollection<ArticleSupplier>();
                FillWarehouseList();
                FillSupplierList();
                FillArticleList();
                SupplierClearButtonVisibility = Visibility.Hidden;
                ArticleClearButtonVisibility = Visibility.Hidden;
                SelectedItems = new List<object>();
                SelectedItems.Add(WarehouseList[0]);
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor PurchasingReportFilterViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PurchasingReportFilterViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PurchasingReportFilterViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PurchasingReportFilterViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region public Events

        public event EventHandler RequestClose;
        // Property Change Logic 
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // Public Events

        #region validation

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
                    me[BindableBase.GetPropertyName(() => SelectedItems)];
                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                //if (!allowValidation) return null;
                string selectedItems = BindableBase.GetPropertyName(() => SelectedItems);

                if (columnName == selectedItems)
                    return PurchasingReportFilterValidation.GetErrorMessage(selectedItems, SelectedItems);

                return null;
            }
        }

        #endregion

        #region Methods

        private void OptionPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
        }

        private void FillSupplierList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSupplierList ...", category: Category.Info, priority: Priority.Low);
                SupplierList.AddRange(SrmService.GetAllSuppliersForPurchasingReport());
                GeosApplication.Instance.Logger.Log("Method FillSupplierList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSupplierList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSupplierList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSupplierList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillArticleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleList() ...", category: Category.Info, priority: Priority.Low);

                ArticleList = new ObservableCollection<Article>(SrmService.GetAllArticles()); ;

                GeosApplication.Instance.Logger.Log("Method FillArticleList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillWarehouseList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseList ...", category: Category.Info, priority: Priority.Low);
                WarehouseList = new ObservableCollection<Warehouses>(SRMCommon.Instance.WarehouseList);

                GeosApplication.Instance.Logger.Log("Method FillWarehouseList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SalesReportAcceptAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SalesReportAcceptAction ...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();

                if (error != null)
                    return;
                else
                {
                    GenerateReport = true;
                    RequestClose(null, null);
                }

                GeosApplication.Instance.Logger.Log("Method SalesReportAcceptAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesReportAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SalesReportCancelButtonCommandAction(object obj)
        {
            RequestClose(null, null);
        }
        private void ClearSelectedArticleCommandAction(object obj)
        {
            SelectedArticle = null;
            ArticleClearButtonVisibility = Visibility.Hidden;
        }
        private void ClearSelectedSupplierCommandAction(object obj)
        {
            SelectedSupplier = null;
            SupplierClearButtonVisibility = Visibility.Hidden;
        }
        private Action Processing()
        {
            IsBusy = true;
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
                    WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = true;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
            return null;
        }
        public void Dispose()
        {
        }

        #endregion
    }
}
