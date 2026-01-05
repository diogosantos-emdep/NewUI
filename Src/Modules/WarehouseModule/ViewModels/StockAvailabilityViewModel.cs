using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.WMS;
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
using System.Linq;
using System.Runtime.InteropServices.Expando;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class StockAvailabilityViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        #endregion

        #region Declarations
        private double dialogHeight;
        private double dialogWidth;
        private List<object> selectedWarehouse;
        private ObservableCollection<Warehouses> allWarehousesList;
        private string articleReference;
        private int idArticle;
        private ObservableCollection<Warehouses> warehousesArticleStockList;
        private ObservableCollection<Warehouses> tempWarehousesArticleStockList;
        private string currentStockBgColor;
        private string currentStockFgColor;
        private Visibility warehousesArticleStockListVisibility;
        private bool isExpand;
        private ObservableCollection<RegionCountryWarehouse> regionCountryWarehouseMenuList;
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

        public List<object> SelectedWarehouse
        {
            get { return selectedWarehouse; }
            set
            {
                selectedWarehouse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWarehouse"));
            }
        }

        public ObservableCollection<Warehouses> AllWarehousesList
        {
            get { return allWarehousesList; }
            set
            {
                allWarehousesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllWarehousesList"));
            }
        }

        public ObservableCollection<Warehouses> WarehousesArticleStockList
        {
            get { return warehousesArticleStockList; }
            set
            {
                warehousesArticleStockList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehousesArticleStockList"));
            }
        }

        public ObservableCollection<Warehouses> TempWarehousesArticleStockList
        {
            get { return tempWarehousesArticleStockList; }
            set
            {
                tempWarehousesArticleStockList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempWarehousesArticleStockList"));
            }
        }


        public string ArticleReference
        {
            get { return articleReference; }
            set
            {
                articleReference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleReference"));
            }
        }

        public Int32 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdArticle"));
            }
        }

        public string CurrentStockBgColor
        {
            get { return currentStockBgColor; }
            set
            {
                currentStockBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentStockBgColor"));
            }
        }

        public string CurrentStockFgColor
        {
            get { return currentStockFgColor; }
            set
            {
                currentStockFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentStockFgColor"));
            }
        }


        public bool IsExpand
        {
            get
            {
                return isExpand;
            }

            set
            {
                isExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExpand"));
            }
        }


        public ObservableCollection<RegionCountryWarehouse> RegionCountryWarehouseMenuList
        {
            get
            {
                return regionCountryWarehouseMenuList;
            }

            set
            {
                regionCountryWarehouseMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegionCountryWarehouseMenuList"));
            }
        }
        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand SearchButtonCommand { get; set; }
        public ICommand LoadedEventCommand { get; set; }
        public ICommand ExpandAndCollapsWarehousesCommand { get; set; }
        #endregion

        #region Constructor

        public StockAvailabilityViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor StockAvailabilityViewModel....", category: Category.Info, priority: Priority.Low);

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

            DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 250;
            DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 100;
            CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
            ExpandAndCollapsWarehousesCommand = new DelegateCommand<object>(ExpandAndCollapsWarehousesCommandAction);//[cpatil][15.9.2025][GEOS2-8200]
            //SearchButtonCommand = new DelegateCommand<object>(SearchButtonCommandAction);
            SearchButtonCommand = new DelegateCommand<object>(SearchButtonCommandAction);
            LoadedEventCommand = new RelayCommand(new Action<object>(UserControl_Loaded));
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Constructor StockAvailabilityViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region methods

        public void Init(string articleDataReference, Int32 idArticle)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method StockAvailabilityViewModel.Init...", category: Category.Info, priority: Priority.Low);
            
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

                AllWarehousesList = new ObservableCollection<Warehouses>();
                SelectedWarehouse = new List<object>();
                WarehousesArticleStockList = new ObservableCollection<Warehouses>();
                FillRegionCountryWarehouseList();
                AllWarehousesList.AddRange(WarehouseService.GetAllWarehouses_V2080(GeosApplication.Instance.ActiveUser.IdUser));

                //SelectedWarehouse.AddRange(AllWarehousesList.Cast<Warehouses>()).ToList();
                IdArticle = idArticle;
                ArticleReference = articleDataReference;
                //AllWarehousesList[0].IsRegional = 1;
                //AllWarehousesList[1].IsRegional = 1;
                //AllWarehousesList[2].IsRegional = 1;
                //AllWarehousesList[3].IsRegional = 1;

                //WarehousesArticleStockList.AddRange(WarehouseService.GetSelectedWarehousesArticleStock(IdArticle, AllWarehousesList.ToList()));

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method StockAvailabilityViewModel.Init() executed successfully...", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in StockAvailabilityViewModel.Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in StockAvailabilityViewModel.Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in StockAvailabilityViewModel.Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-8200][18.09.2025]
        private void FillRegionCountryWarehouseList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegionCountryWarehouseList()...", category: Category.Info, priority: Priority.Low);

                RegionCountryWarehouseMenuList = new ObservableCollection<RegionCountryWarehouse>(WarehouseService.GetRegionCountrySiteWarehouseData(GeosApplication.Instance.ActiveUser.IdUser));
                if (RegionCountryWarehouseMenuList != null)
                {
                    var sumOfAll = RegionCountryWarehouseMenuList.Sum(i => i.ChildCount);
                    RegionCountryWarehouseMenuList.Insert(0, new RegionCountryWarehouse() { Key = "AllNode", ParentKey = null, Name = "All", ChildCount = sumOfAll, LevelType = "Root", NameWithCount = "" });
                    RegionCountryWarehouseMenuList = new ObservableCollection<RegionCountryWarehouse>(RegionCountryWarehouseMenuList);
                    foreach (var item in RegionCountryWarehouseMenuList)
                    {
                        if (string.IsNullOrEmpty(item.ParentKey))
                            item.ParentKey = "AllNode";
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillRegionCountryWarehouseList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRegionCountryWarehouseList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRegionCountryWarehouseList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillRegionCountryWarehouseList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonAction()...", category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CancelButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void SearchButtonCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method SearchButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                var ownerInfo = (obj as FrameworkElement);
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

                TempWarehousesArticleStockList = new ObservableCollection<Warehouses>();
                WarehousesArticleStockList = new ObservableCollection<Warehouses>();

                //[rdixit][GEOS2-8200][18.09.2025]
                var CheckedWarehouses = RegionCountryWarehouseMenuList?.Where(i => i.IsChecked).Select(j => j.Name).ToList();
                SelectedWarehouse = new List<object>();

                if (CheckedWarehouses?.Count > 0)
                {
                    var checkedSet = new HashSet<string>(CheckedWarehouses, StringComparer.OrdinalIgnoreCase);
                    SelectedWarehouse.AddRange(AllWarehousesList.Where(w => checkedSet.Contains(w.Name)).ToList());
                }

                var SelectedWarehouseList = (SelectedWarehouse.ConvertAll(x => (Warehouses)x)).ToList();
                // shubham[skadam] GEOS2-3047 Add in the “Error Message” the warehouse/s that did not respond when trying to collect the data in Check Availability 05 Aug 2022
                List<string> errorList = new List<string>();
                foreach (Warehouses SelectedWarehouseItem in SelectedWarehouse)
                {
                    try
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + SelectedWarehouseItem.Name;
                        List<Warehouses> Lstwarehouses = new List<Warehouses>();
                        Lstwarehouses.Add(SelectedWarehouseItem);
                        // shubham[skadam] GEOS2-3762 No reporta correctamente Check Availability (01IFRM06N13G1L)  09 Aug 2022
                        if (SelectedWarehouse != null)
                            TempWarehousesArticleStockList.AddRange(WarehouseService.GetSelectedWarehousesArticleStock_V2300(IdArticle, Lstwarehouses));
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", SelectedWarehouseItem.Name, " Failed");
                        errorList.Add(ex.Detail.ErrorMessage + " -  " + SelectedWarehouseItem.Name + "\n");
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", SelectedWarehouseItem.Name, " Failed");
                        errorList.Add(ex.Message + " -  " + SelectedWarehouseItem.Name + "\n");
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", SelectedWarehouseItem.Name, " Failed");
                        errorList.Add(ex.Message + " -  " + SelectedWarehouseItem.Name + "\n");
                    }
                }

               
                if (TempWarehousesArticleStockList != null)
                    WarehousesArticleStockList.AddRange(TempWarehousesArticleStockList.OrderByDescending(a => a.IsRegional).ThenByDescending(b => b.ArticleCurrentStock).ToList());

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                if (errorList.Count() != 0)
                {
                    // string.Format(System.Windows.Application.Current.FindResource("SODDropRecordMessage").ToString())
                    //CustomMessageBox.Show(String.Join("", errorList), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    CustomMessageBox.Show(String.Join("", errorList), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Method SearchButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in SearchButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in SearchButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in SearchButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// method to set ByDefault Selected All warehouse in dxe:ListBoxEdit
        /// </summary>
        /// <param name="obj"></param>
        private void UserControl_Loaded(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UserControl_Loaded()...", category: Category.Info, priority: Priority.Low);

                StockAvailabilityView ObjStockAvailabilityView = (StockAvailabilityView)obj;
               // ObjStockAvailabilityView.AvailableWarehouseListBox.SelectAll();
                //ObjStockAvailabilityView.BorderOfGridControl.Width = ObjStockAvailabilityView.BorderOfGridControl.ActualWidth;
                //ObjStockAvailabilityView.GridControlWarehousesArticleStockList.Width = ObjStockAvailabilityView.BorderOfGridControl.ActualWidth;
                //ObjStockAvailabilityView.GridControlWarehousesArticleStockList.View.Width = ObjStockAvailabilityView.BorderOfGridControl.ActualWidth;

                GeosApplication.Instance.Logger.Log("Method UserControl_Loaded()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in UserControl_Loaded() Method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex),
                    category: Category.Exception, priority: Priority.Low);
            }
        }



        //[15.09.2025][cpatil][GEOS2-8200]
        private void ExpandAndCollapsWarehousesCommandAction(object obj)
        {
            try
            {
                DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
                if (IsExpand)
                {
                    t.ExpandAllNodes();
                    IsExpand = false;
                }
                else
                {
                    t.CollapseAllNodes();
                    IsExpand = true;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExpandAndCollapsWarehousesCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
