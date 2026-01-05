using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
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

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class DeliveryNotePrintPreviewViewModel : INotifyPropertyChanged, IDisposable
    {

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services


        #region TaskLog
        //[000][skale][23-12-2019][GEOS2-1855] Allow to set a custom quantity when printing the DN item label in Delivery Note
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

        #endregion // End Of Events


        #region Declaration
        // [000] added
        private int quantity;
        //private int copies = 1;
        private int maxQuantity;
        private bool isAccept;
        //end
        #endregion

        #region Properties
        //[000]added
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(new PropertyChangedEventArgs("Quantity")); }
        }
        //public int Copies
        //{
        //    get { return copies; }
        //    set { copies = value; OnPropertyChanged(new PropertyChangedEventArgs("Copies")); }
        //}
        public bool IsAccept
        {
            get { return isAccept; }
            set { isAccept = value; OnPropertyChanged(new PropertyChangedEventArgs("IsAccept")); }
        }
        //end

        public int MaxQuantity
        {
            get { return maxQuantity; }
            set { maxQuantity = value; OnPropertyChanged(new PropertyChangedEventArgs("MaxQuantity")); }
        }
        #endregion

        #region Command       
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandPrintButton { get; set; }
        #endregion

        #region Constructor 
        public DeliveryNotePrintPreviewViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor DeliveryNotePrintPreviewViewModel....", category: Category.Info, priority: Priority.Low);
            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandPrintButton = new DelegateCommand<object>(PrintAction);
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Constructor DeliveryNotePrintPreviewViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion


        #region Command Action
        private void PrintAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PrintAction DeliveryNotePrintPreviewViewModel ....", category: Category.Info, priority: Priority.Low);


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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                isAccept = true;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("PrintAction DeliveryNotePrintPreviewViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                isAccept = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Cancel Action
        /// </summary>
        /// <param name="obj"></param>
        private void CommandCancelAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("CommandCancelAction DeliveryNotePrintPreviewViewModel....", category: Category.Info, priority: Priority.Low);
            isAccept = false;
            RequestClose(null, null);
            GeosApplication.Instance.Logger.Log("CommandCancelAction DeliveryNotePrintPreviewViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region Methods
        /// <summary>
        /// obj Dispose
        /// </summary>
        public void Dispose()
        {
            GeosApplication.Instance.Logger.Log("Dispose DeliveryNotePrintPreviewViewModel....", category: Category.Info, priority: Priority.Low);
            GC.SuppressFinalize(this);
            GeosApplication.Instance.Logger.Log("Dispose DeliveryNotePrintPreviewViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

    }
}
