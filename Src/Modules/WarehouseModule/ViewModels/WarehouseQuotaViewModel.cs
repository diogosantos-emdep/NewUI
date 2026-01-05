using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common.SAM;
using DevExpress.Xpf.Core.Serialization;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class WarehouseQuotaViewModel: NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        //[pramod.misal][GEOS2-4229][11.05.2023][New ViewModel for WarehouseQuota in Configuration]

        #region Services
     
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        #endregion // Services

        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public List<WarehouseQuota> warehouseQuota { get; set; }
        DataTable dtWarehouseQuotaCopy;

        #endregion // End Of Events 

        #region Declaration
        private bool isInit;
        private string filterString;
        bool isBusy;
        private List<OTs> mainWarehouseList = new List<OTs>();
        public string Warehouse_WarehouseQuota_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "Warehouse_ConfigurationCategoriesGrid_Setting.Xml";
        private bool isCategoriesColumnChooserVisible;
        private ObservableCollection<WarehouseQuota> warehouseQuotaList;
        private WarehouseQuota selectedWarehouse;
        int isButtonStatus;
        Visibility isCalendarVisible;
        private Currency selectedCurrency;

        string warehouseQuotaEUR;


        #endregion

        #region Public Properties
        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }

        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterString"));
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public virtual bool DialogResult { get; set; }

        public virtual string ResultFileName { get; set; }

        public List<OTs> MainWarehouseList
        {
            get { return mainWarehouseList; }
            set
            {
                mainWarehouseList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainWarehouseList"));
            }
        }

        public bool IsCategoriesColumnChooserVisible
        {
            get { return isCategoriesColumnChooserVisible; }
            set
            {
                isCategoriesColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCategoriesColumnChooserVisible"));
            }
        }

        public ObservableCollection<WarehouseQuota> WarehouseQuotaList
        {
            get
            {
                return warehouseQuotaList;
            }

            set
            {
                warehouseQuotaList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseQuotaList"));
            }
        }

        public WarehouseQuota SelectedDepartment
        {
            get
            {
                return selectedWarehouse;
            }

            set
            {
                selectedWarehouse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDepartment"));
            }
        }

        public int IsButtonStatus
        {
            get
            {
                return isButtonStatus;
            }

            set
            {
                isButtonStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonStatus"));
            }
        }

        public Visibility IsCalendarVisible
        {
            get
            {
                return isCalendarVisible;
            }

            set
            {
                isCalendarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalendarVisible"));
            }
        }

        private List<Currency> currencies;

        public string WarehouseQuotaEUR
        {
            get
            {
                return warehouseQuotaEUR;
            }

            set
            {
                warehouseQuotaEUR = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseQuotaEUR"));
            }
        }

        public List<Currency> Currencies
        {
            get
            {
                return currencies;
            }

            set
            {
                currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currencies"));
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

        #endregion

        #region ICommands
        public ICommand RefreshItemCommand { get; set; }
        public ICommand AddNewWarehouseQuotaTargetCommand { get; set; }
        public ICommand PrintItemCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        #endregion

        #region Constructot

        public WarehouseQuotaViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WarehouseQuotaViewModel....", category: Category.Info, priority: Priority.Low);

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
               
                Init();
                RefreshItemCommand = new RelayCommand(new Action<object>(RefreshWarehouseQuota));
                PrintItemCommand = new RelayCommand(new Action<object>(PrintWarehouseQuota));
                CommandGridDoubleClick = new DelegateCommand<object>(EditWarehouseQuotaViewWindowShow);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportWarehouseQuotaList)); 
                 AddNewWarehouseQuotaTargetCommand = new RelayCommand(new Action<object>(AddNewWarehousetargetInformation));
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor  WarehouseQuotaViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  WarehouseQuotaViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            IsInit = false;

        }

        #endregion

        #region Methods
        private void Init()
        {
            FillWarehouseQuota();
        }

        private void FillWarehouseQuota()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
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



             

                Currencies = GeosApplication.Instance.Currencies.ToList();
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCurrency_Warehouse"))
                {
                    SelectedCurrency = Currencies.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                    
                }


                WarehouseQuotaList = new ObservableCollection<WarehouseQuota>(WarehouseService.GetWarehouseQuota_V2390(SelectedCurrency.IdCurrency));

                foreach (WarehouseQuota item in WarehouseQuotaList)
                {
                    item.SalesQuotaAmountwithSymbol =  item.SalesQuotaAmountwithSymbol+ SelectedCurrency.Symbol;
                }
                if (WarehouseQuotaList.Count > 0)
                {
                    SelectedDepartment = WarehouseQuotaList[0];
                }


                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCurrency_Warehouse"))
                {
                    string test = "("+ GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]+")";

                    WarehouseQuotaEUR = string.Format(System.Windows.Application.Current.FindResource("WarehouseQuotaEUR").ToString(), test);
                }

                //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshWarehouseQuota(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWarehouseQuota()...", category: Category.Info, priority: Priority.Low);

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

                FillWarehouseQuota();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                var tableView = (TableView)obj;
                tableView.BeginInit();
                tableView.SearchString = null;
                FilterString = string.Empty;
                tableView.EndInit();



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshWarehouseQuota()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWarehouseQuota() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
               
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWarehouseQuota() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWarehouseQuota()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-4551][07.06.2023]
        private void PrintWarehouseQuota(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintDepartmentList()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PageHeader"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PageFooter"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintDepartmentList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintDepartmentList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditWarehouseQuotaViewWindowShow(object obj)
        {


            GeosApplication.Instance.Logger.Log("Method EditWarehouseQuotaViewWindowShow ...", category: Category.Info, priority: Priority.Low);
            string shortName = string.Empty;
            try
            {

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                List<Company> TempCompany = new List<Company>();
                WarehouseQuota selectedRecord = (WarehouseQuota)detailView.FocusedRow;
                EditWarehouseQuotaView editWarehouseQuotaView = new EditWarehouseQuotaView();

               
                EditWarehouseQuotaViewModel editWarehouseQuotaViewModel = new EditWarehouseQuotaViewModel();
                int maxyear = WarehouseQuotaList.Select(i => i.Year).Max();
                editWarehouseQuotaViewModel.InIt(maxyear, selectedRecord);

                //editWarehouseQuotaViewModel.EditWarehouseQuotaInformation(obj, selectedRecord);
                 EventHandler handle = delegate { editWarehouseQuotaView.Close(); };
                editWarehouseQuotaViewModel.Isexist = Visibility.Visible; ;
                editWarehouseQuotaViewModel.IsNew = Visibility.Hidden;
                editWarehouseQuotaViewModel.RequestClose += handle;
                editWarehouseQuotaViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditWarehouseQuotaViewEdit").ToString()+" "+ selectedRecord.Name +" "+ System.Windows.Application.Current.FindResource("EditWarehouseQuotaViewTarget").ToString();
                editWarehouseQuotaView.DataContext = editWarehouseQuotaViewModel;

                //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                editWarehouseQuotaView.ShowDialogWindow();

                if (editWarehouseQuotaViewModel.IsUpdated==true)
                {
                    FillWarehouseQuota();
                }


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditWarehouseQuotaViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }

        }

        //[pramod.misal][GEOS2-4229][15.05.2023]
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(Warehouse_WarehouseQuota_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(Warehouse_WarehouseQuota_SettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(Warehouse_WarehouseQuota_SettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsCategoriesColumnChooserVisible = true;
                }
                else
                {
                    IsCategoriesColumnChooserVisible = false;
                }
                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(Warehouse_WarehouseQuota_SettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsCategoriesColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(Warehouse_WarehouseQuota_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportWarehouseQuotaList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportDepartmentList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "WarehouseTarget List";
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
                    IsBusy = true;
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
                    TableView departmentTableView = ((TableView)obj);
                    departmentTableView.ShowTotalSummary = false;
                    departmentTableView.ShowFixedTotalSummary = false;
                    departmentTableView.ExportToXlsx(ResultFileName);

                    IsBusy = false;
                    departmentTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Method ExportDepartmentList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportDepartmentList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewWarehousetargetInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewWarehousetargetInformation()...", category: Category.Info, priority: Priority.Low);


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
              //  WarehouseQuota selectedRecord = (WarehouseQuota)detailView.FocusedRow;
                EditWarehouseQuotaViewModel editWarehouseQuotaViewModel = new EditWarehouseQuotaViewModel();
                EditWarehouseQuotaView editWarehouseQuotaView = new EditWarehouseQuotaView();
                EventHandler handle1 = delegate { editWarehouseQuotaView.Close(); };
                editWarehouseQuotaViewModel.RequestClose += handle1;
                //editWarehouseQuotaViewModel.IsNew = true;
                editWarehouseQuotaViewModel.Isexist = Visibility.Hidden; ;
                editWarehouseQuotaViewModel.IsNew = Visibility.Visible;
                editWarehouseQuotaViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddAddWarehouseTarget").ToString();
                editWarehouseQuotaViewModel.AddInit();
                editWarehouseQuotaView.DataContext = editWarehouseQuotaViewModel;
                editWarehouseQuotaView.ShowDialogWindow();


                if (editWarehouseQuotaViewModel.IsUpdated == true)
                {
                    FillWarehouseQuota();
                }




                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method AddNewWarehousetargetInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewWarehousetargetInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {

        }


        #endregion
    }
}
