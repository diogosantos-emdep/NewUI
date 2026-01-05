using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    class EditArticleSupplierViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {

        }

        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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

        #endregion // end public events region

        #region Declaration

        private bool isSaveChanges;
        private double dialogHeight;
        private double dialogWidth;
        bool isMoreNeeded = true;
        private ArticleSupplier articleSupplier;
        public string CreatedDays { get; set; }
        public string CreatedDaysStr { get; set; }
        #endregion

        #region Properties
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        public ArticleSupplier ArticleSupplier
        {
            get
            {
                return articleSupplier;
            }

            set
            {
                articleSupplier = value;
            }
        }
        #endregion

        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand ArticleHyperlinkClickCommand { get; set; }
        

        #endregion

        #region Constructor

        public EditArticleSupplierViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditArticleSupplierViewModel()...", category: Category.Info, priority: Priority.Low);

                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 130;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 50;

                CancelButtonCommand = new DelegateCommand<object>(CancelButtonAction);
                ArticleHyperlinkClickCommand = new DelegateCommand<object>(ArticleHyperlinkClickAction);
                
                GeosApplication.Instance.Logger.Log("Constructor EditArticleSupplierViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditArticleSupplierViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods

        public void ArticleHyperlinkClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleHyperlinkClickAction....", category: Category.Info, priority: Priority.Low);
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
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TableView detailView = (TableView)obj;
                ArticleBySupplier otItem = (ArticleBySupplier)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = SRMCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                int ArticleSleepDays = SRMCommon.Instance.ArticleSleepDays;
                articleDetailsViewModel.Init_SRM(otItem.Reference, warehouseId, warehouse, ArticleSleepDays);
                articleDetailsView.DataContext = articleDetailsViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (obj as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                //[001] added
                if (articleDetailsViewModel.IsResult)
                {
                    //if (articleDetailsViewModel.UpdateArticle.MyWarehouse != null)
                    //otItem.ArticleMinimumStock = articleDetailsViewModel.UpdateArticle.MyWarehouse.MinimumStock;
                }
                //end
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ArticleHyperlinkClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ArticleHyperlinkClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init(ulong IdArticleSupplier, Warehouses objWarehouse)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                ArticleSupplier = SRMService.GetArticleSupplierByIdArticleSupplier(objWarehouse,IdArticleSupplier);
                //ArticleSupplier.Age = Math.Round((double)((GeosApplication.Instance.ServerDateTime.Month - ArticleSupplier.CreatedIn.Month) + 12 * (GeosApplication.Instance.ServerDateTime.Year - ArticleSupplier.CreatedIn.Year)) / 12, 1);
                DateCalculateInYearAndMonthHelper dateCalculateInYearAndMonth = new DateCalculateInYearAndMonthHelper(GeosApplication.Instance.ServerDateTime.Date, ArticleSupplier.CreatedIn.Date);

                if (dateCalculateInYearAndMonth.Years > 0)
                {
                    CreatedDays = dateCalculateInYearAndMonth.Years.ToString() + "+";
                    CreatedDaysStr = string.Format(System.Windows.Application.Current.FindResource("SRMEditSupplierYears").ToString());
                }
                else
                {
                    if ((GeosApplication.Instance.ServerDateTime.Date - ArticleSupplier.CreatedIn.Date).Days > 99)
                       CreatedDays = "99+";
                    else
                        CreatedDays = (GeosApplication.Instance.ServerDateTime.Date - ArticleSupplier.CreatedIn.Date).Days.ToString();

                    CreatedDaysStr = string.Format(System.Windows.Application.Current.FindResource("SRMEditSupplierDays").ToString());
                }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PoCancelButton()...", category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method PoCancelButton()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PoCancelButton()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
