using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class ReceptionReportFilterViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region TaskLog
        ///[pramod.misal][20-05-2025][GEOS2-5727]
        #endregion

        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SrmService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SrmService = new SRMServiceController("localhost:6699");
        #endregion // Services

        #region ICommands

        public ICommand ReceptionReportCancelButtonCommand { get; set; }

        public ICommand ClearSelectedArticleCommand { get; set; }

        public ICommand ReceptionReportAcceptButtonCommand { get; set; }


        #endregion // Commands

        #region Declaration
        ObservableCollection<Article> articleList;
        ObservableCollection<Warehouses> warehouseList;
        public virtual List<object> SelectedItems { get; set; }
        private bool isBusy;
        private DateTime fromDate;
        private DateTime toDate;
        private Article selectedArticle;
        private Visibility articleClearButtonVisibility;
        public bool GenerateReport;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        public ObservableCollection<Article> ArticleList
        {
            get
            {
                return articleList;
            }
            set
            {
                articleList = value;
                OnPropertyChanged("ArticleList");
            }
        }

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

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
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

        public Visibility ArticleClearButtonVisibility
        {
            get { return articleClearButtonVisibility; }
            set
            {
                articleClearButtonVisibility = value;
                OnPropertyChanged("ArticleClearButtonVisibility");

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

        #endregion

        #region Constructor
        public ReceptionReportFilterViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ReceptionReportFilterViewModel ...", category: Category.Info, priority: Priority.Low);
                Processing();
                FromDate = new DateTime(DateTime.Now.Year, 1, 1);
                ToDate = DateTime.Now.Date;
                ReceptionReportCancelButtonCommand = new RelayCommand(new Action<object>(ReceptionReportCancelButtonCommandAction));
                ClearSelectedArticleCommand = new RelayCommand(new Action<object>(ClearSelectedArticleCommandAction));
                ReceptionReportAcceptButtonCommand = new RelayCommand(new Action<object>(ReceptionReportAcceptAction));
                FillArticleList();
                FillWarehouseList();
                ArticleClearButtonVisibility = Visibility.Hidden;

                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor ReceptionReportFilterViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
               
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReceptionReportFilterViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
               
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReceptionReportFilterViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
               
                GeosApplication.Instance.Logger.Log("Get an error in ReceptionReportFilterViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }
        #endregion

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



        #region Methods

        public void ReceptionReportAcceptAction(object obj)
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

        private void ClearSelectedArticleCommandAction(object obj)
        {
            SelectedArticle = null;
            ArticleClearButtonVisibility = Visibility.Hidden;
        }

        private void FillArticleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleList() ...", category: Category.Info, priority: Priority.Low);
               
                ArticleList = new ObservableCollection<Article>(SrmService.GetAllArticles()); ;
                ArticleList.Insert(0, new Article { Reference = "---" });
                SelectedArticle = ArticleList[0];
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
                SelectedItems = new List<object>();
                SelectedItems.Add(WarehouseList[0]);
                
                GeosApplication.Instance.Logger.Log("Method FillWarehouseList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                    DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = true;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
            return null;
        }

        public void ReceptionReportCancelButtonCommandAction(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
           
        }
        #endregion

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




    }
}
