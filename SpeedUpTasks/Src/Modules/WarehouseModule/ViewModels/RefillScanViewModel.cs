using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class RefillScanViewModel : INotifyPropertyChanged
    {

        #region TaskLog

        /// <summary>
        ///WMS	M057-13	Add scan location start point in Refill [adaibathina]
        /// </summary>

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

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services

        #region Declaration
        private string locationBarcode;
        private List<LocationRefill> locationsToRefillList;
        private Visibility refillErrorVisibility;
        private string refillBarcodeError;
        private string refillErrorColour;
        #endregion

        #region Properties
        public string LocationBarcode
        {
            get { return locationBarcode; }
            set
            {
                locationBarcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationBarcode"));
            }
        }

        public List<LocationRefill> LocationsToRefillList
        {
            get { return locationsToRefillList; }
            set
            {
                locationsToRefillList = value;

            }
        }
        public Visibility RefillErrorVisibility
        {
            get { return refillErrorVisibility; }
            set
            {
                refillErrorVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RefillErrorVisibility"));
            }

        }
        public string RefillBarcodeError
        {
            get { return refillBarcodeError; }
            set
            {
                refillBarcodeError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RefillBarcodeError"));
            }
        }

        public string RefillErrorColour
        {
            get { return refillErrorColour; }
            set
            {
                refillErrorColour = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RefillErrorColour"));
            }
        }
      
        #endregion

        #region Command       
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandSkipButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandOnLoaded { get; set; }

        #endregion

        #region Constructor 
        public RefillScanViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor RefillScanViewModel....", category: Category.Info, priority: Priority.Low);

            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandScanBarcode = new RelayCommand(new Action<object>(ScanBarcodeAction));
            CommandSkipButton = new DelegateCommand<object>(SkipScan);
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);


        }
        #endregion

        #region Methods

        public void Init()
        {
            GeosApplication.Instance.Logger.Log("RefillScanViewModel in SkipScan....", category: Category.Info, priority: Priority.Low);

            RefillErrorVisibility = Visibility.Hidden;
            LocationsToRefillList = new List<LocationRefill>();

            GeosApplication.Instance.Logger.Log("RefillScanViewModel SkipScan executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void SkipScan(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("RefillScanViewModel in SkipScan....", category: Category.Info, priority: Priority.Low);

                LocationsRefillView locationsRefillView = new LocationsRefillView();
                LocationsRefillViewModel locationsRefillViewModel = new LocationsRefillViewModel();
                EventHandler handle = delegate { locationsRefillView.Close(); };

                locationsRefillViewModel.RequestClose += handle;
                FillLocationRefillArticles();
                locationsRefillViewModel.Init(LocationsToRefillList);
                // No items to scan
                if (locationsRefillViewModel.RefillList.Count == 0)
                {
                    SetViewInErrorState();
                    return;
                }

                locationsRefillView.DataContext = locationsRefillViewModel;
                var ownerInfo = (obj as FrameworkElement);
                locationsRefillView.Owner = Window.GetWindow(ownerInfo);
                locationsRefillView.ShowDialog();
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("RefillScanViewModel SkipScan executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefillScanViewModel SkipScan() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Scaning bare code
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("RefillScanViewModel in ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);

                if (((KeyEventArgs)obj).Key == Key.Enter)
                {
                    if(string.IsNullOrEmpty(LocationBarcode))
                    {
                        SetViewInErrorState();
                        return;
                    }

                    LocationsRefillView locationsRefillView = new LocationsRefillView();
                    LocationsRefillViewModel locationsRefillViewModel = new LocationsRefillViewModel();
                    EventHandler handle = delegate { locationsRefillView.Close(); };
                    locationsRefillViewModel.RequestClose += handle;
                    FillLocationRefillArticles();

                    //To list not found
                    if (LocationsToRefillList.Count == 0)
                    {
                        SetViewInErrorState();
                        return;

                    }

                    locationsRefillViewModel.Init(LocationsToRefillList);
                    // to list found but from list not found
                    if (locationsRefillViewModel.RefillList.Count == 0)
                    {
                        SetViewInErrorState();
                        return;
                    }
                    locationsRefillView.DataContext = locationsRefillViewModel;
                    locationsRefillView.ShowDialog();
                    RequestClose(null, null);
                }

                GeosApplication.Instance.Logger.Log("RefillScanViewModel ScanBarcodeAction executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefillScanViewModel ScanBarcodeAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Cancel Action
        /// </summary>
        /// <param name="obj"></param>
        /// 
        private void CommandCancelAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("CommandCancelAction RefillScanViewModel....", category: Category.Info, priority: Priority.Low);

            RequestClose(null, null);

            GeosApplication.Instance.Logger.Log("CommandCancelAction RefillScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// obj Dispose
        /// </summary>
        public void Dispose()
        {
            GeosApplication.Instance.Logger.Log("Dispose RefillScanViewModel....", category: Category.Info, priority: Priority.Low);

            GC.SuppressFinalize(this);

            GeosApplication.Instance.Logger.Log("Dispose RefillScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// [001][skale][24-07-2019][GEOS2-1667]Priorize first empty locations in Refill
        /// [002][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        private void FillLocationRefillArticles()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method FillLocationRefillArticles....", category: Category.Info, priority: Priority.Low);

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

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    //Int64 _idwarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                    if (!string.IsNullOrEmpty(LocationBarcode))
                    {
                        //[001] added
                        if (WarehouseCommon.Instance.IsRefillEmptyLocationsFirst)
                        {

                            // [002] Changed Service method
                            LocationsToRefillList = WarehouseService.GetRefillToListByFullNameSortByStock_V2034(WarehouseCommon.Instance.Selectedwarehouse, locationBarcode);
                        }
                        else
                        {

                            // [002] Changed Service method
                            LocationsToRefillList = WarehouseService.GetRefillToListByFullName_V2034(WarehouseCommon.Instance.Selectedwarehouse, locationBarcode);
                            LocationsToRefillList = LocationsToRefillList.OrderBy(x => x.FullName).ToList();
                        }
                    }
                    else
                    {
                        //[001] added
                        if (WarehouseCommon.Instance.IsRefillEmptyLocationsFirst)
                        {

                            // [002] Changed Service method
                            LocationsToRefillList = WarehouseService.GetRefillToListSortByStock_V2034(WarehouseCommon.Instance.Selectedwarehouse);
                        }
                        else
                        {

                            // [002] Changed Service method
                            LocationsToRefillList = WarehouseService.GetRefillToList_V2034(WarehouseCommon.Instance.Selectedwarehouse);
                            LocationsToRefillList = LocationsToRefillList.OrderBy(x => x.FullName).ToList();
                        }
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method FillLocationRefillArticles....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method  FillLocationRefillArticles() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method FillLocationRefillArticles() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method FillLocationRefillArticles...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }

        private void SetViewInErrorState()
        {
            RefillBarcodeError = " No items to Refill at Location " + LocationBarcode;
            LocationBarcode = string.Empty;
            RefillErrorColour = "#FFFF0000";
            RefillErrorVisibility = Visibility.Visible;
        }
      
        #endregion
    }
}
