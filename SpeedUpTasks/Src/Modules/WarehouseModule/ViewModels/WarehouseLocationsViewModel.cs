using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
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
using System.Collections.ObjectModel;
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
    class WarehouseLocationsViewModel : INotifyPropertyChanged
    {

        #region TaskLog
        /// <summary>
        /// [WMS-M050-13] Add and edit warehouse locations in locations configuration[27/10/2018][adadibathina]
        /// </summary>
        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration
        private ObservableCollection<WarehouseLocation> warehouseLocationList;
        private WarehouseLocation selectedWarehouseLocation;
        #endregion

        #region Properties
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public ObservableCollection<WarehouseLocation> WarehouseLocationList
        {
            get { return warehouseLocationList; }
            set
            {
                warehouseLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocationList"));
            }
        }

        public WarehouseLocation SelectedWarehouseLocation
        {
            get { return selectedWarehouseLocation; }
            set
            {
                selectedWarehouseLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWarehouseLocation"));
            }
        }
        #endregion

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Public Commands
        public ICommand RefreshWarehouseLocationViewCommand { get; set; }
        public ICommand PrintWarehouseLocationViewCommand { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand ExportWarehouseLocationViewCommand { get; set; }

        //[WMS-M050-13]
        public ICommand AddLocationCommand { get; set; }
        public ICommand EditLocationDoubleCommand { get; set; }
        #endregion

        #region Constructor
        public WarehouseLocationsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WarehouseLocationsViewModel()...", category: Category.Info, priority: Priority.Low);
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
                RefreshWarehouseLocationViewCommand = new RelayCommand(new Action<object>(RefreshWarehouseLocationList));
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
                PrintWarehouseLocationViewCommand = new RelayCommand(new Action<object>(PrintWareHouseLocationList));
                ExportWarehouseLocationViewCommand = new RelayCommand(new Action<object>(ExportWarehouseLocationList));
                AddLocationCommand = new RelayCommand(new Action<object>(AddLocation));
                EditLocationDoubleCommand = new RelayCommand(new Action<object>(EditLocation));


                FillWarehouseLocationsList();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor WarehouseLocationsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor WarehouseLocationsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

        #region Methods
        /// <summary>
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        public void FillWarehouseLocationsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseLocationsList...", category: Category.Info, priority: Priority.Low);
                // [001] Changed Service method
                WarehouseLocationList = new ObservableCollection<WarehouseLocation>(WarehouseService.GetWarehouseLocationsByIdWarehouse_V2034(WarehouseCommon.Instance.Selectedwarehouse));


                // WarehouseLocationList=new ObservableCollection<WarehouseLocation>(WarehouseService.GetAllWarehouseLocationById(Warehouse.IdWarehouse));
                //SelectedWarehouseLocation = WarehouseLocationList[0];
                GeosApplication.Instance.Logger.Log("Method FillWarehouseLocationsList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseLocationsList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseLocationsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseLocationsList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void RefreshWarehouseLocationList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWarehouseLocationList...", category: Category.Info, priority: Priority.Low);
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
                FillWarehouseLocationsList();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshWarehouseLocationList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWarehouseLocationList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void WarehouseEditValueChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);
                //When setting the warehouse from default the data should not be refreshed
                if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                    return;
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


                FillWarehouseLocationsList();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WarehouseEditValueChangedCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintWareHouseLocationList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWareHouseLocationList...", category: Category.Info, priority: Priority.Low);
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

                PrintableControlLink pcl = new PrintableControlLink((TreeListView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TreeListView)obj).Resources["WarehouseLocationListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TreeListView)obj).Resources["WarehouseLocationListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;

                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintWareHouseLocationList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintWareHouseLocationList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void ExportWarehouseLocationList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLocationList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Location List";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {

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

                    ResultFileName = (saveFile.FileName);
                    TreeListView locationTableView = ((TreeListView)obj);
                    locationTableView.ShowTotalSummary = false;
                    locationTableView.ShowFixedTotalSummary = false;
                    locationTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    locationTableView.ShowTotalSummary = true;

                    locationTableView.ShowFixedTotalSummary = true;

                    GeosApplication.Instance.Logger.Log("Method ExportLocationList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportLocationList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  //[WMS-M050-13]
        /// </summary>
        /// <param name="obj"></param>

        private void AddLocation(object obj)
        {
            try
            {
                long NewLocationIdWarehouseLocation;
                GeosApplication.Instance.Logger.Log("LocationViewModel Method AddLocation()...", category: Category.Info, priority: Priority.Low);
                TreeListView detailView = (TreeListView)obj;
                AddLocationView addLocationView = new AddLocationView();
                AddLocationViewModel addLocationViewModel = new AddLocationViewModel();
                EventHandler handle = delegate { addLocationView.Close(); };
                addLocationViewModel.RequestClose += handle;
                addLocationView.DataContext = addLocationViewModel;
                addLocationViewModel.Init();
                addLocationViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddLocation").ToString();
                addLocationViewModel.IsNew = true;
                var ownerInfo = (detailView as FrameworkElement);
                addLocationView.Owner = Window.GetWindow(ownerInfo);
                addLocationView.ShowDialogWindow();
                if (addLocationViewModel.IsSave == true)
                {
                    WarehouseLocationList.Add(addLocationViewModel.NewLocation);

                    NewLocationIdWarehouseLocation = addLocationViewModel.NewLocation.IdWarehouseLocation;
                    // Refreshing grid for (calculated or reindxed) positions
                    RefreshWarehouseLocationList(new object());
                    SelectedWarehouseLocation = WarehouseLocationList.FirstOrDefault(x => x.IdWarehouseLocation == NewLocationIdWarehouseLocation);

                }
                GeosApplication.Instance.Logger.Log(" LocationViewModel Method AddLocation()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LocationViewModel  Method AddLocation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Edit a location
        /// </summary>
        /// <param name="obj"></param>
        public void EditLocation(object obj)
        {
            try
            {
                long EditLocationIdWarehouseLocation;
                GeosApplication.Instance.Logger.Log("LocationViewModel Method Edit()...", category: Category.Info, priority: Priority.Low);
                TreeListView detailView = (TreeListView)obj;
                WarehouseLocation warehouseLocation = (WarehouseLocation)detailView.FocusedRow;
                SelectedWarehouseLocation = warehouseLocation;
                AddLocationView editLocationView = new AddLocationView();
                AddLocationViewModel editLocationViewModel = new AddLocationViewModel();
                EventHandler handle = delegate { editLocationView.Close(); };
                editLocationViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditLocation").ToString();
                editLocationViewModel.RequestClose += handle;
                editLocationView.DataContext = editLocationViewModel;

                if (SelectedWarehouseLocation.Parent != 0 && (WarehouseLocationList.FirstOrDefault(x => x.IdWarehouseLocation == SelectedWarehouseLocation.Parent).IsLead == 1))
                {
                    editLocationViewModel.WarehouseLocationList.Add(WarehouseLocationList.FirstOrDefault(x => x.IdWarehouseLocation == SelectedWarehouseLocation.Parent));
                }
                editLocationViewModel.Init(warehouseLocation);
                var ownerInfo = (detailView as FrameworkElement);
                editLocationView.Owner = Window.GetWindow(ownerInfo);
                editLocationView.ShowDialog();
                if (editLocationViewModel.IsSave)
                {
                    SelectedWarehouseLocation.FullName = editLocationViewModel.NewLocation.FullName;
                    SelectedWarehouseLocation.HtmlColor = editLocationViewModel.NewLocation.HtmlColor;
                    SelectedWarehouseLocation.IdWarehouseLocation = editLocationViewModel.NewLocation.IdWarehouseLocation;
                    SelectedWarehouseLocation.InUse = editLocationViewModel.NewLocation.InUse;
                    SelectedWarehouseLocation.Name = editLocationViewModel.NewLocation.Name;
                    SelectedWarehouseLocation.Parent = editLocationViewModel.NewLocation.Parent;
                    SelectedWarehouseLocation.Position = editLocationViewModel.NewLocation.Position;
                    SelectedWarehouseLocation.IdDirection = editLocationViewModel.NewLocation.IdDirection;
                    SelectedWarehouseLocation.Direction = editLocationViewModel.NewLocation.Direction;
                    SelectedWarehouseLocation.IsLead = editLocationViewModel.IsLeaf;
                    // Refreshing grid for (calculated or reindxed) positions
                    RefreshWarehouseLocationList(new object());
                    EditLocationIdWarehouseLocation = editLocationViewModel.NewLocation.IdWarehouseLocation;
                    SelectedWarehouseLocation = WarehouseLocationList.FirstOrDefault(x => x.IdWarehouseLocation == EditLocationIdWarehouseLocation);
                }
                detailView.Focus();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditLocation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }




        #endregion
    }
}
