using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class PurchaseOrderItemDetailsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Task log
        // [000][avpawar][19-08-2019][GEOS2-1692] [Responsive View purchase order]
        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // End Services

        #region Declaration
        public event EventHandler RequestClose;
        private WarehousePurchaseOrder warehousePurchaseOrder;
        private MemoryStream pdfDoc;

        bool isNotMoreNeeded = false;
        bool isMoreNeeded = true;
        private Visibility isVisible;
        private double dialogHeight;

        //[000] added
        private double dialogWidth;
        #endregion 

        #region Public ICommands

        public ICommand PoCancelButtonCommand { get; set; }
        public ICommand PopupMenuShowingCommand { get; private set; }
        public ICommand CommandGridClick { get; set; }
        public ICommand NewDeliveryNoteClick { get; set; }

        public ICommand CommandDeliveryNoteGridDoubleClick { get; set; }

        #endregion //Public Commands

        #region Properties

        public Visibility IsVisible
        {
            get
            {
                return isVisible;
            }

            set
            {
                isVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisible"));
            }
        }
        public bool IsNotMoreNeeded
        {
            get
            {
                return isNotMoreNeeded;
            }
            set
            {
                isNotMoreNeeded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNotMoreNeeded"));
            }
        }

        public bool IsMoreNeeded
        {
            get
            {
                return isMoreNeeded;
            }
            set
            {
                isMoreNeeded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMoreNeeded"));
            }
        }

        public WarehousePurchaseOrder WarehousePurchaseOrder
        {
            get
            {
                return warehousePurchaseOrder;
            }
            set
            {
                warehousePurchaseOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehousePurchaseOrder"));
            }
        }

        public MemoryStream PdfDoc
        {
            get { return pdfDoc; }
            set
            {
                pdfDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PdfDoc"));
            }
        }
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        //[000] added
        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        #endregion // Properties

        #region Constructor

        public PurchaseOrderItemDetailsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PurchaseOrderItemDetailsViewModel()...", category: Category.Info, priority: Priority.Low);
                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 130;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 130;  //[000] added
                PoCancelButtonCommand = new DelegateCommand<object>(PoCancelButton);
                PopupMenuShowingCommand = new DelegateCommand<OpenPopupEventArgs>(OpenPopupDateEditAction);
                CommandGridClick = new DelegateCommand<object>(DeliveryNoteViewpdf);
                NewDeliveryNoteClick = new RelayCommand(new Action<object>(DeliveryNoteViewWindowShow));
                CommandDeliveryNoteGridDoubleClick = new DelegateCommand<object>(DeliveryNoteGridDoubleClickAction);

                GeosApplication.Instance.Logger.Log("Constructor PurchaseOrderItemDetailsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PurchaseOrderItemDetailsViewModel PendingReceptionItemsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion //Constructor

        #region Methods

        private void OpenPopupDateEditAction(OpenPopupEventArgs obj)
        {
            obj.Cancel = true;
            obj.Handled = true;
        }

        /// <summary>
        /// This method Initializes Warehouse Purchase Order
        /// </summary>
        public void Init(long idWarehousePurchaseOrder, Warehouses objWarehouse)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WarehousePurchaseOrder = WarehouseService.GetPurchaseOrderPendingReceptionByIdWarehousePurchaseOrder(idWarehousePurchaseOrder, objWarehouse);
                WarehousePurchaseOrder.Warehouse = objWarehouse;

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method for Purchase Order Cancel Button
        /// </summary>
        private void PoCancelButton(object obj)
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

        private void DeliveryNoteGridDoubleClickAction(object obj)
        {
            try
            {
                GridControl gridControl = (GridControl)obj;

                WarehouseDeliveryNote warehouseDeliveryNote = (WarehouseDeliveryNote)gridControl.CurrentItem;

                DXSplashScreen.Show<SplashScreenView>();
                EditDeliveryNoteView editDeliveryNoteView = new EditDeliveryNoteView();
                EditDeliveryNoteViewModel editDeliveryNoteViewModel = new EditDeliveryNoteViewModel();
                EventHandler handle = delegate { editDeliveryNoteView.Close(); };
                editDeliveryNoteViewModel.RequestClose += handle;

                //42822
                WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById(warehouseDeliveryNote.IdWarehouseDeliveryNote);
                wdn.WarehousePurchaseOrder.Warehouse = WarehousePurchaseOrder.Warehouse;

                editDeliveryNoteViewModel.Init(wdn);
                editDeliveryNoteView.DataContext = editDeliveryNoteViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                var ownerInfo = (gridControl as FrameworkElement);
                editDeliveryNoteView.Owner = Window.GetWindow(ownerInfo);
                editDeliveryNoteView.ShowDialog();
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method DeliveryNoteGridDoubleClickAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeliveryNoteViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeliveryNoteViewWindowShow()...", category: Category.Info, priority: Priority.Low);

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
                //DXSplashScreen.Show<SplashScreenView>();
                DeliveryNoteView deliveryNoteView = new DeliveryNoteView();
                DeliveryNoteViewModel deliveryNoteViewModel = new DeliveryNoteViewModel();
                EventHandler handle = delegate { deliveryNoteView.Close(); };
                deliveryNoteViewModel.RequestClose += handle;
                deliveryNoteViewModel.Init(WarehousePurchaseOrder);
                deliveryNoteView.DataContext = deliveryNoteViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                var ownerInfo = (obj as FrameworkElement);
                deliveryNoteView.Owner = Window.GetWindow(ownerInfo);
                deliveryNoteView.ShowDialog();
                if (deliveryNoteViewModel.IsDeliveryNoteSaved)
                {
                    WarehousePurchaseOrder = WarehouseService.GetPurchaseOrderPendingReceptionByIdWarehousePurchaseOrder(WarehousePurchaseOrder.IdWarehousePurchaseOrder, WarehousePurchaseOrder.Warehouse);

                    foreach (var item in WarehousePurchaseOrder.WarehousePurchaseOrderItems)
                    {
                        if (item.Quantity == item.ReceivedQuantity)
                            IsNotMoreNeeded = true;
                        else
                        {
                            IsNotMoreNeeded = false;
                            break;
                        }

                    }

                    if (!IsNotMoreNeeded)
                    {
                 
                            IsMoreNeeded = true;
                            IsVisible = Visibility.Visible;
                    }
                    else
                    {
                            IsMoreNeeded = false;
                            IsVisible = Visibility.Hidden;
                    }
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DeliveryNoteViewWindowShow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method DeliveryNoteViewWindowShow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeliveryNoteViewpdf(object obj)
        {
            GridControl deliveryNoteGrid = (GridControl)obj;
            WarehouseDeliveryNote warehouseDeliveryNote = (WarehouseDeliveryNote)deliveryNoteGrid.CurrentItem;

            try
            {
                GeosApplication.Instance.Logger.Log("Method DeliveryNoteViewpdf()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DeliveryNotePDFView DeliveryNotePDFView = new DeliveryNotePDFView();
                DeliveryNotePDFViewModel DeliveryNotePDFViewModel = new DeliveryNotePDFViewModel();
                DeliveryNotePDFViewModel.OpenPdfByCode(WarehousePurchaseOrder, warehouseDeliveryNote);
                DeliveryNotePDFView.DataContext = DeliveryNotePDFViewModel;
                DeliveryNotePDFView.Show();

                GeosApplication.Instance.Logger.Log("Method DeliveryNoteViewpdf()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeliveryNoteViewpdf() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //warehouseDeliveryNote.PdfFilePath
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeliveryNoteViewpdf() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                CustomMessageBox.Show(string.Format("Could not find file '{0}'.", warehouseDeliveryNote.PdfFilePath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeliveryNoteViewpdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {

        }

        #endregion // Methods

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // end public events region
    }
}
