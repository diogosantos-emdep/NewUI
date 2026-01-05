using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Spreadsheet;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class StockBySupplierViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        IWarehouseService WmsService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration
        bool isBusy;
        private Visibility isGridViewVisible;
        private ObservableCollection<StockBySupplier> stockbysupplierList = new ObservableCollection<StockBySupplier>();
        #endregion

        #region Properties
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public Visibility IsGridViewVisible
        {
            get
            {
                return isGridViewVisible;
            }

            set
            {
                isGridViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridViewVisible"));
            }
        }

        public ObservableCollection<string> Names { get; set; }

        public ObservableCollection<StockBySupplier> StockbysupplierList
        {
            get
            {
                return stockbysupplierList;
            }

            set
            {
                stockbysupplierList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StockbysupplierList"));
            }
        }

        #endregion

        #region Event Handlers
        //[001][Ranjana Dixit][GEOS2-3627][13.04.2022]
        public event EventHandler RequestClose;


        #endregion

        #region Icommands
        public ICommand AcceptCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        #endregion //End Of Icommand

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Constructor

        public StockBySupplierViewModel()
        {
            WarehouseCommon.Instance.SelectedStockBySupplierwarehouse = WarehouseCommon.Instance.WarehouseList.FirstOrDefault(a => a.Name == "EWHQ");
            GeosApplication.Instance.Logger.Log("Constructor StockBySupplierViewModel....", category: Category.Info, priority: Priority.Low);
            AcceptCommand = new DelegateCommand<object>(AcceptCommandAction);
            CancelCommand = new RelayCommand(new Action<object>(CancelCommandAction));
            GeosApplication.Instance.Logger.Log("Constructor StockBySupplierViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion //End Of Constructor

        #region Methods
        private void AcceptCommandAction(object obj)
        {
            try
            {
               StockBySupplierGridViewModel TEMP = new StockBySupplierGridViewModel(); //new StockBySupplierGridViewModel();
            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
                RequestClose(null, null);
                TEMP.FillStockBySupplier();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in StockBySupplierViewModel AcceptCommandAction method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("StockBySupplierViewModel AcceptCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        //[001][Ranjana Dixit][GEOS2-3627][14.04.2022]
        private void CancelCommandAction(object obj)
        {
            //MenuFlyout menu = (MenuFlyout)obj;
            //menu.IsOpen = false;
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelCommandAction()..."), category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        private void ShowSupplierGridRowItemWindowAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSupplierGridRowItemWindowAction....", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSupplierGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion //End Of Methods
    }
}
