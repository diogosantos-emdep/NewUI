using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class DashboardSaleViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration

        private List<Company> copyCompanies;
        List<Customer> customersFromPlant;
        List<CustomerSort> lstsort;
        private List<Customer> customersOfferDashboard;

        private string customerTotal;
        Dictionary<string, double> customSummary;

        private ObservableCollection<SalesStatusType> listSalesStatusType = new ObservableCollection<SalesStatusType>();

        DataTable dtTable;
        DataTable graphDataTable;
        DataTable copyGraphDataTable;
        DataTable copyMainGraphDataTable;

        private string myFilterString;

        //private bool isInit;

        private bool isBusy;
        List<string> failedPlants;
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;
        #endregion // Declaration

        #region public Properties

        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }

        public Boolean IsShowFailedPlantWarning
        {
            get { return isShowFailedPlantWarning; }
            set
            {
                isShowFailedPlantWarning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowFailedPlantWarning"));
            }
        }

        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
            }
        }

        public IList<Offer> OfferDashboard { get; set; }

        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public List<Company> companies { get; set; }

        public List<Customer> CustomersFromPlant
        {
            get { return customersFromPlant; }
            set { customersFromPlant = value; }
        }

        public List<CustomerSort> Lstsort
        {
            get { return lstsort; }
            set { lstsort = value; }
        }

        public List<Customer> CustomersOfferDashboard
        {
            get { return customersOfferDashboard; }
            set { customersOfferDashboard = value; }
        }
        public List<Company> Companies
        {
            get { return companies; }
            set { companies = value; }
        }


        public List<Company> CopyCompanies
        {
            get { return copyCompanies; }
            set { copyCompanies = value; }
        }

        public string CustomerTotal
        {
            get { return customerTotal; }
            set
            {
                customerTotal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerTotal"));
            }
        }

        public DataTable DtTable
        {
            get { return dtTable; }
            set
            {
                dtTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtTable"));
            }
        }

        public DataTable GraphDataTable
        {
            get { return graphDataTable; }
            set
            {
                graphDataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable"));
            }
        }

        public DataTable CopyGraphDataTable
        {
            get { return copyGraphDataTable; }
            set
            {
                copyGraphDataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CopyGraphDataTable"));
            }
        }

        public DataTable CopyMainGraphDataTable
        {
            get { return copyMainGraphDataTable; }
            set
            {
                copyMainGraphDataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CopyMainGraphDataTable"));
            }
        }

        public Dictionary<string, double> CustomSummary
        {
            get { return customSummary; }
            set { customSummary = value; }
        }

        public ObservableCollection<SalesStatusType> ListSalesStatusType
        {
            get { return listSalesStatusType; }
            set
            {
                SetProperty(ref listSalesStatusType, value, () => ListSalesStatusType);
            }
        }

        IList<Offer> salesPerformanceList;
        public IList<Offer> SalesPerformanceList
        {
            get { return salesPerformanceList; }
            set
            {
                salesPerformanceList = value;
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

        public ObservableCollection<UI.Helper.Summary> TotalSummary { get; private set; }

        // Export Excel .xlsx
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }

        #endregion // Properties

        #region Commands

        public ICommand CustomCellAppearanceCommand { get; private set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ChartDashboardSalebyCustomerLoadCommand { get; set; }
        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }
        public ICommand ChartDashboardGridNodeCustomSummaryCommand { get; set; }
        public ICommand ChartDashboardGridNodeExpandedCommand { get; set; }
        public ICommand ChartDashboardGridNodeCollapsedCommand { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshDashboardSalesViewCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }

        #endregion // Commands

        #region public event

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Constructor

        public DashboardSaleViewModel()
        {
            try
            {


                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                GeosApplication.Instance.Logger.Log("Constructor DashboardSaleViewModel ...", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                ChartDashboardSalebyCustomerLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardSalebyCustomerAction);
                ChartDashboardSaleCustomDrawCrosshairCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);
                ChartDashboardGridNodeCustomSummaryCommand = new DevExpress.Mvvm.DelegateCommand<TreeListCustomSummaryEventArgs>(ChartDashboardGridNodeCustomSummarydAction);
                ChartDashboardGridNodeExpandedCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardGridNodeExpandedAction);
                ChartDashboardGridNodeCollapsedCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardGridNodeCollapsedAction);
                CustomCellAppearanceCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(CustomCellAppearance);
                PrintButtonCommand = new DelegateCommand<object>(PrintGrid);
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                RefreshDashboardSalesViewCommand = new DelegateCommand<object>(RefreshOrderDetails);
                ExportButtonCommand = new DelegateCommand<object>(ExportLeadsGridButtonCommandAction);

                /* This Section for Sales Owner*/
                FillCmbSalesOwner();
                /* This Section for Sales Owner*/

                GeosApplication.Instance.Logger.Log("Constructor DashboardSaleViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in DashboardSaleViewModel() Constructor " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in DashboardSaleViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DashboardSaleViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        #endregion

        #region  Methods

        /// <summary>
        /// Method for fill Dashboard Sales details.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ListSalesStatusType = new ObservableCollection<SalesStatusType>(crmControl.GetAllSalesStatusType().AsEnumerable());
                GraphCreateTable();
                AddColumnsToDataTable();

                // for Sales Plant Manager View
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    // Called for 1st Time.
                    FillDataByUser();
                    FillDashboard();
                }
                //Sales Global Manager View
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    FillDataPlantOwner();
                    FillDashboard();
                }
                //Sales Assistant View
                else
                {
                    FillDataByManager();
                    FillDashboard();
                }

                GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCmbSalesOwner() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                //DXSplashScreen.Show<SplashScreenView>(); 
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

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillDataByUser();
                FillDashboard();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {

                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }
            }
            else
            {
                GraphDataTable.Rows.Clear();
                DataTable temp = GraphDataTable.Copy();
                GraphDataTable = temp;

                DtTable.Rows.Clear();

                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

            }
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                //  DXSplashScreen.Show<SplashScreenView>(); 
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

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillDataPlantOwner();
                FillDashboard();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {

                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }
            }
            else
            {
                GraphDataTable.Rows.Clear();
                DataTable temp = GraphDataTable.Copy();
                GraphDataTable = temp;

                DtTable.Rows.Clear();

                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// Method for refresh DashboardSales From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshOrderDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshOrderDetails ...", category: Category.Info, priority: Priority.Low);

            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;

            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                //DXSplashScreen.Show<SplashScreenView>(); 
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

            MyFilterString = string.Empty;
            FillCmbSalesOwner();
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                DXSplashScreen.Close();
            }

            GeosApplication.Instance.Logger.Log("Method RefreshOrderDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void ChartDashboardGridNodeExpandedAction(object obj)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Method ChartDashboardGridNodeExpandedAction ...", category: Category.Info, priority: Priority.Low);

                TreeListView treeListView = (TreeListView)(((RoutedEventArgs)(obj)).Source);
                int nodeCount = 0;
                CopyGraphDataTable = GraphDataTable.Copy();
                CopyGraphDataTable.Rows.Clear();

                foreach (TreeListNode node in treeListView.Nodes)
                {
                    if (node.IsExpanded)
                    {
                        nodeCount++;
                        var dtRow = ((System.Data.DataRowView)(node.Content)).Row;

                        foreach (Customer customer in CustomersOfferDashboard.Where(vbn => vbn.CustomerName == dtRow["CustomerName"].ToString()).ToList())
                        {
                            if (customer != null)
                            {
                                foreach (var company in customer.Companies)
                                {
                                    DataRow graph = CopyGraphDataTable.NewRow();
                                    graph["CustomerName"] = string.Concat(customer.CustomerName, "\n", company.SiteNameWithoutCountry, "\n", company.Country.Name);
                                    //graph["TARGET"] = company.SalesTargetBySite != null ? company.SalesTargetBySite.TargetAmountWithExchangeRate : 0;
                                    graph["TARGET"] = CopyCompanies.Where(cl => cl.IdCompany == company.IdCompany).Select(complst => complst.SalesTargetBySite.TargetAmountWithExchangeRate).FirstOrDefault();

                                    foreach (var salesStatus in company.SalesStatusTypes)
                                    {
                                        if (salesStatus != null)
                                        {
                                            graph[salesStatus.Name] = salesStatus.TotalAmount;
                                            graph[salesStatus.Name] = salesStatus.TotalAmount;
                                        }
                                    }

                                    CopyGraphDataTable.Rows.Add(graph);
                                }
                            }
                        }
                    }
                }

                if (nodeCount == 0)     // if any node is not expanded then show data like first dashboard click.
                {
                    GraphDataTable = CopyMainGraphDataTable;
                }
                else                    // else nodes are expanded then show expanded nodes in graph.
                {
                    GraphDataTable = CopyGraphDataTable;
                }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method ChartDashboardGridNodeExpandedAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardGridNodeExpandedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void ChartDashboardGridNodeCollapsedAction(object obj)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Method ChartDashboardGridNodeCollapsedAction ...", category: Category.Info, priority: Priority.Low);

                CopyGraphDataTable = GraphDataTable.Copy();

                var objTreeListNode = (DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs)obj;
                var kl = (System.Data.DataRowView)objTreeListNode.Row;
                DataRow row = (System.Data.DataRow)kl.Row;

                TreeListView treeListView = (TreeListView)(((RoutedEventArgs)(obj)).Source);

                int nodeCount = 0;
                CopyGraphDataTable.Rows.Clear();
                foreach (TreeListNode node in treeListView.Nodes)
                {
                    if (node.IsExpanded)
                    {
                        nodeCount++;
                        var dtRow = ((System.Data.DataRowView)(node.Content)).Row;

                        foreach (Customer customer in CustomersOfferDashboard.Where(vbn => vbn.CustomerName == dtRow["CustomerName"].ToString()).ToList())
                        {
                            if (customer != null)
                            {
                                foreach (var company in customer.Companies)
                                {
                                    DataRow graph = CopyGraphDataTable.NewRow();
                                    graph["CustomerName"] = string.Concat(customer.CustomerName, "\n", company.SiteNameWithoutCountry, "\n", company.Country.Name);
                                    //graph["TARGET"] = company.SalesTargetBySite != null ? company.SalesTargetBySite.TargetAmountWithExchangeRate : 0;
                                    graph["TARGET"] = CopyCompanies.Where(cl => cl.IdCompany == company.IdCompany).Select(complst => complst.SalesTargetBySite.TargetAmountWithExchangeRate).FirstOrDefault();

                                    foreach (var salesStatus in company.SalesStatusTypes)
                                    {
                                        if (salesStatus != null)
                                        {
                                            graph[salesStatus.Name] = salesStatus.TotalAmount;
                                            graph[salesStatus.Name] = salesStatus.TotalAmount;
                                        }
                                    }

                                    CopyGraphDataTable.Rows.Add(graph);
                                }
                            }
                        }
                    }
                }

                if (nodeCount == 0)     // if any node is not expanded then show data like first dashboard click.
                {
                    GraphDataTable = CopyMainGraphDataTable;
                }
                else                    // else nodes are expanded then show expanded nodes in graph.
                {
                    GraphDataTable = CopyGraphDataTable;
                }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method ChartDashboardGridNodeCollapsedAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardGridNodeCollapsedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void ChartDashboardSalebyCustomerAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartDashboardSalebyCustomerAction ...", category: Category.Info, priority: Priority.Low);

                ChartControl chartcontrol = (ChartControl)obj;
                chartcontrol.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartcontrol.Diagram = diagram;
                diagram.ActualAxisX.Title = new AxisTitle() { Content = "Customer" };
                diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.Currency;

                // Changed list as per requirement
                List<SalesStatusType> CopyListSalesStatusType = new List<SalesStatusType>();
                CopyListSalesStatusType.Add(ListSalesStatusType.Where(item => item.IdSalesStatusType == 5).SingleOrDefault());
                foreach (var salesStatus in ListSalesStatusType.Where(item => item.IdSalesStatusType != 5).ToList())
                {
                    CopyListSalesStatusType.Add(salesStatus);
                }
                CopyListSalesStatusType.Reverse();

                foreach (var item in CopyListSalesStatusType)
                {
                    if (item != null)
                    {
                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2D = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                        barSideBySideStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                        barSideBySideStackedSeries2D.DisplayName = item.Name;
                        barSideBySideStackedSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                        barSideBySideStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                        barSideBySideStackedSeries2D.ArgumentDataMember = "CustomerName";
                        barSideBySideStackedSeries2D.ValueDataMember = item.Name;
                        barSideBySideStackedSeries2D.Model = new DevExpress.Xpf.Charts.SimpleBar2DModel();
                        barSideBySideStackedSeries2D.ShowInLegend = false;

                        barSideBySideStackedSeries2D.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                        Bar2DSlideFromTopAnimation seriesAnimation = new Bar2DSlideFromTopAnimation();
                        seriesAnimation.Duration = new TimeSpan(0, 0, 2);
                        barSideBySideStackedSeries2D.PointAnimation = seriesAnimation;

                        chartcontrol.Diagram.Series.Add(barSideBySideStackedSeries2D);

                        BarSideBySideStackedSeries2D barSideBySideStackedSeries2Dhidden = new BarSideBySideStackedSeries2D();
                        barSideBySideStackedSeries2Dhidden.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(item.HtmlColor.ToString()));
                        chartcontrol.Diagram.Series.Insert(0, barSideBySideStackedSeries2Dhidden);
                        barSideBySideStackedSeries2Dhidden.DisplayName = item.Name;
                    }
                }

                diagram.SecondaryAxesY.Clear();
                SecondaryAxisY2D Yaxis = new SecondaryAxisY2D();
                Yaxis.Title = new AxisTitle() { Content = "Target" };
                Yaxis.NumericScaleOptions = new ContinuousNumericScaleOptions();
                Yaxis.NumericScaleOptions.AutoGrid = false;
                Yaxis.Label = new AxisLabel();
                Yaxis.NumericOptions = new NumericOptions();
                Yaxis.NumericOptions.Format = NumericFormat.Currency;
                diagram.SecondaryAxesY.Add(Yaxis);

                LineStackedSeries2D lineStackedSeries2D = new DevExpress.Xpf.Charts.LineStackedSeries2D();
                lineStackedSeries2D.ArgumentScaleType = ScaleType.Auto;
                lineStackedSeries2D.ValueScaleType = ScaleType.Numerical;
                lineStackedSeries2D.DisplayName = "TARGET";
                lineStackedSeries2D.CrosshairLabelPattern = "{S} : {V:c2}";
                lineStackedSeries2D.Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F50"));
                lineStackedSeries2D.ArgumentDataMember = "CustomerName";
                lineStackedSeries2D.ValueDataMember = "TARGET";
                lineStackedSeries2D.AxisY = Yaxis;
                chartcontrol.Diagram.Series.Add(lineStackedSeries2D);

                chartcontrol.EndInit();
                chartcontrol.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartcontrol.Animate();
                GeosApplication.Instance.Logger.Log("Method ChartDashboardSalebyCustomerAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSalebyCustomerAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to display data on hover of Bar. (order of CrosshairElements is reversed)
        /// </summary>
        /// <param name="obj"></param>
        private void ChartDashboardSaleCustomDrawCrosshairCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ChartDashboardSaleCustomDrawCrosshairCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                CustomDrawCrosshairEventArgs e = (CustomDrawCrosshairEventArgs)obj;
                foreach (var group in e.CrosshairElementGroups)
                {
                    var reverseList = group.CrosshairElements.ToList();
                    group.CrosshairElements.Clear();
                    foreach (var item in reverseList)
                    {
                        if (item.Series.DisplayName == "TARGET")
                            group.CrosshairElements.Add(item);
                        else
                            group.CrosshairElements.Insert(0, item);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ChartDashboardSaleCustomDrawCrosshairCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSaleCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void GraphCreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GraphCreateTable ...", category: Category.Info, priority: Priority.Low);

                GraphDataTable = new DataTable();
                GraphDataTable.Columns.Add("IdCompany", typeof(string));
                GraphDataTable.Columns.Add("IdCustomer", typeof(string));
                GraphDataTable.Columns.Add("CustomerName");
                GraphDataTable.Columns.Add("TARGET", typeof(double)).DefaultValue = 0;

                CopyGraphDataTable = new DataTable();
                CopyMainGraphDataTable = new DataTable();
                //CopyGraphDataTable.Columns.Add("Customer");
                //CopyGraphDataTable.Columns.Add("TARGET", typeof(double)).DefaultValue = 0;

                foreach (var item in ListSalesStatusType)
                {
                    GraphDataTable.Columns.Add(item.Name, typeof(double)).DefaultValue = 0;
                    //CopyGraphDataTable.Columns.Add(item.Name, typeof(double)).DefaultValue = 0;
                }

                GeosApplication.Instance.Logger.Log("Method GraphCreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GraphCreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void CustomCellAppearance(RoutedEventArgs obj)
        {
            TableView detailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
            detailView.BestFitColumns();
        }

        private void AddColumnsToDataTable()
        {
            GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);

            CustomerTotal = string.Format(System.Windows.Application.Current.FindResource("DashboardSaleTopCustomer").ToString(), GeosApplication.Instance.CrmTopCustomers);

            Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>() {
                    new Emdep.Geos.UI.Helper.Column() { FieldName="IdCompany", HeaderText="Code", Settings = SettingsType.Default, AllowCellMerge = true, Width = 85, AllowEditing = false, Visible = false, IsVertical = false },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="IdCustomer", HeaderText="Country", Settings = SettingsType.Default, AllowCellMerge = true, Width = 85, AllowEditing = false, Visible = false, IsVertical = false},
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerName", HeaderText="GROUP/PLANT", Settings = SettingsType.Array, AllowCellMerge = true, Width = 200, AllowEditing = false, Visible = true, IsVertical = false},

               };

            DtTable = new DataTable();
            DtTable.Columns.Add("IdCompany", typeof(string));
            DtTable.Columns.Add("IdCustomer", typeof(string));
            DtTable.Columns.Add("CustomerName", typeof(string));
            DtTable.Columns.Add("TARGET", typeof(double)).DefaultValue = 0;

            TotalSummary = new ObservableCollection<UI.Helper.Summary>();
            TotalSummary.Add(new UI.Helper.Summary() { Type = SummaryItemType.Custom, FieldName = "TARGET", DisplayFormat = "{0:C}" });

            CustomSummary = new Dictionary<string, double>();
            CustomSummary.Add("TARGET", 0.0);

            List<SalesStatusType> TempListSalesStatusType = new List<SalesStatusType>(ListSalesStatusType.ToList());

            List<Int64> TempListSalesStatusId = new List<Int64>() { 4, 3, 6, 2, 1, 5 };

            TempListSalesStatusType = TempListSalesStatusType.OrderBy(Or => TempListSalesStatusId.IndexOf(Or.IdSalesStatusType)).ToList();

            foreach (var item in TempListSalesStatusType)
            {
                if (item != null)
                {
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = item.Name.ToString(), HeaderText = item.Name.ToString(), Settings = SettingsType.Amount, AllowCellMerge = true, Width = 180, AllowEditing = false, Visible = true, IsVertical = false });
                    DtTable.Columns.Add(item.Name.ToString(), typeof(double)).DefaultValue = 0;
                    TotalSummary.Add(new UI.Helper.Summary() { Type = SummaryItemType.Custom, FieldName = item.Name.ToString(), DisplayFormat = "{0:C}" });
                    CustomSummary.Add(item.Name.ToString(), 0.0);
                }
            }

            Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "TARGET", HeaderText = "TARGET", Settings = SettingsType.Amount, AllowCellMerge = true, Width = 150, AllowEditing = false, Visible = true, IsVertical = false });

            GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is used to Fill Data in List By Active User.
        /// [001][cpatil][GEOS2-5299][26-02-2024]
        /// </summary>
        private void FillDataByManager()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataByManager ...", category: Category.Info, priority: Priority.Low);

                CustomersFromPlant = new List<Customer>();
                Lstsort = new List<CustomerSort>();
                CustomersOfferDashboard = new List<Customer>();
                Companies = new List<Company>();
                // [001] Changed Service method
                //Service GetAllCompaniesDetails_V2490 updated with GetAllCompaniesDetails_V2500 by [GEOS2-5556][27.03.2024][rdixit]
                foreach (var companyDetails in crmControl.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser))
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + companyDetails.Alias;
                        #region Service Logs
                        //CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_V2110(GeosApplication.Instance.IdCurrencyByRegion,
                        //                                                                              GeosApplication.Instance.ActiveUser.IdUser,
                        //                                                                              GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                        //                                                                              GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                        //                                                                              GeosApplication.Instance.CrmTopCustomers,
                        //                                                                              companyDetails, GeosApplication.Instance.IdUserPermission).ToList());
                        //[rdixit][GEOS2-3517][30-06-2022][Ignore the “To” date in the “DASHBOARD->Top Accounts” section for opportunities info]

                        //shubham[skadam] GEOS2-3359 top customer showing wrong in CRM dashboard report 10 OCT 2022
                        //CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_IgnoreOfferCloseDate_V2320(GeosApplication.Instance.IdCurrencyByRegion,
                        //                                                                              GeosApplication.Instance.ActiveUser.IdUser,
                        //                                                                              GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                        //                                                                              GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                        //                                                                              GeosApplication.Instance.CrmTopCustomers,
                        //                                                                              companyDetails, GeosApplication.Instance.IdUserPermission).ToList());
                        //service GetDashboard2Details_IgnoreOfferCloseDate_V2360 updated with GetDashboard2Details_IgnoreOfferCloseDate_V2420  [rdixit][GEOS2-4712][22.08.2023]
                        #endregion
                        //[rdixit][GEOS2-4223][03.03.2023]
                        //CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_IgnoreOfferCloseDate_V2420(GeosApplication.Instance.IdCurrencyByRegion,
                        //                                                                          GeosApplication.Instance.ActiveUser.IdUser,
                        //                                                                          GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                        //                                                                          GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                        //                                                                          GeosApplication.Instance.CrmTopCustomers,
                        //                                                                          companyDetails, GeosApplication.Instance.IdUserPermission).ToList());

                        //[Sudhir.Jangra][GEOS2-5310]

                        CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_IgnoreOfferCloseDate_V2490(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                  GeosApplication.Instance.ActiveUser.IdUser,
                                                                                                  GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                                                                                                  GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                  GeosApplication.Instance.CrmTopCustomers,
                                                                                                  companyDetails, GeosApplication.Instance.IdUserPermission).ToList());


                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", companyDetails.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(companyDetails.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log("Get an error in FillDataByManager() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                        //CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", companyDetails.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(companyDetails.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log("Get an error in FillDataByManager() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                        //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", companyDetails.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(companyDetails.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log("Get an error in FillDataByManager() Method - Exception " + ex.Message, category: Category.Info, priority: Priority.Low);
                    }
                }

                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }

                FillListByPlant();
                if (CustomersOfferDashboard.Count > 0)
                {
                    FillCompleteListOfTopCustomers();
                }
                //service GetSitesTargetNewCurrencyConversion updated with GetSitesTargetNewCurrencyConversion_V2420  [rdixit][GEOS2-4712][22.08.2023]
                //Companies = crmControl.GetSitesTarget(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.IdUserPermission);
                Companies = crmControl.GetSitesTargetNewCurrencyConversion_V2420(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.IdUserPermission);
                CopyCompanies = new List<Company>(Companies);

                GeosApplication.Instance.Logger.Log("Method FillDataByManager() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByManager() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                //CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByManager() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByManager() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        /// <summary>
        /// This method is used to Fill Data in List By Active User.
        /// </summary>
        private void FillDataPlantOwner()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataPlantOwner ...", category: Category.Info, priority: Priority.Low);

                CustomersFromPlant = new List<Customer>();
                Lstsort = new List<CustomerSort>();
                CustomersOfferDashboard = new List<Customer>();
                Companies = new List<Company>();

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.ShortName;
                            #region Previos Logs
                            //CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_V2110(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                                               GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                                              GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                            //                                                                               GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                               GeosApplication.Instance.CrmTopCustomers, itemPlantOwnerUsers,
                            //                                                                               GeosApplication.Instance.IdUserPermission).ToList());
                            //[rdixit][GEOS2-3517][30-06-2022][Ignore the “To” date in the “DASHBOARD->Top Accounts” section for opportunities info]
                            //shubham[skadam] GEOS2-3359 top customer showing wrong in CRM dashboard report 10 OCT 2022
                            //CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_IgnoreOfferCloseDate_V2320(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                                              GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                                             GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                            //                                                                              GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                              GeosApplication.Instance.CrmTopCustomers, itemPlantOwnerUsers,
                            //                                                                              GeosApplication.Instance.IdUserPermission).ToList());
                            //service GetDashboard2Details_IgnoreOfferCloseDate_V2360 updated with GetDashboard2Details_IgnoreOfferCloseDate_V2420  [rdixit][GEOS2-4712][22.08.2023]
                            #endregion
                            //[rdixit][GEOS2-4223][03.03.2023]
                            //CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_IgnoreOfferCloseDate_V2420(GeosApplication.Instance.IdCurrencyByRegion,
                            //                                                                           GeosApplication.Instance.ActiveUser.IdUser,
                            //                                                                           GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                            //                                                                           GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                           GeosApplication.Instance.CrmTopCustomers, itemPlantOwnerUsers,
                            //                                                                           GeosApplication.Instance.IdUserPermission).ToList());


                            //[Sudhir.Jangra][GEOS2-5310]

                            CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_IgnoreOfferCloseDate_V2490(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                      GeosApplication.Instance.ActiveUser.IdUser,
                                                                                                      GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                                                                                                      GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                      GeosApplication.Instance.CrmTopCustomers, itemPlantOwnerUsers,
                                                                                                      GeosApplication.Instance.IdUserPermission).ToList());

                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log("Get an error in FillDataPlantOwner() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                            //CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log("Get an error in FillDataPlantOwner() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                            //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log("Get an error in FillDataPlantOwner() Method - Exception " + ex.Message, category: Category.Info, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {

                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }

                    FillListByPlant();
                    if (CustomersOfferDashboard.Count > 0)
                    {
                        FillCompleteListOfTopCustomers();
                    }                    
                    // Companies = crmControl.GetSitesTargetByPlant(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.CrmOfferYear, plantOwnersIds);
                    Companies = crmControl.GetSitesTargetByPlantNewCurrencyConversion(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, plantOwnersIds);
                    CopyCompanies = new List<Company>(Companies);
                }
                GeosApplication.Instance.Logger.Log("Method FillDataPlantOwner() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDataPlantOwner() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                //CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDataPlantOwner() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDataPlantOwner() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        /// <summary>
        /// Get Dashboard2 details by Assigned Users from SaleOwner Combobox.
        /// [001][cpatil][GEOS2-5299][26-02-2024]
        /// </summary>
        private void FillDataByUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataByUser()...", category: Category.Info, priority: Priority.Low);

                CustomersFromPlant = new List<Customer>();
                Lstsort = new List<CustomerSort>();
                CustomersOfferDashboard = new List<Customer>();
                Companies = new List<Company>();

                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    PreviouslySelectedSalesOwners = salesOwnersIds;
                    // [001] Changed Service method
                    //Service GetAllCompaniesDetails_V2490 updated with GetAllCompaniesDetails_V2500 by [GEOS2-5556][27.03.2024][rdixit]
                    foreach (var items in crmControl.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser))
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + items.Alias;
                            #region Previous Logs
                            //CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_V2110(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, salesOwnersIds,
                            //                                                                                         GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                                         GeosApplication.Instance.CrmTopCustomers,
                            //                                                                                         items,
                            //                                                                                         GeosApplication.Instance.IdUserPermission));
                            //[rdixit][GEOS2-3517][30-06-2022][Ignore the “To” date in the “DASHBOARD->Top Accounts” section for opportunities info]


                            //shubham[skadam] GEOS2-3359 top customer showing wrong in CRM dashboard report 10 OCT 2022
                            //CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_IgnoreOfferCloseDate_V2320(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, salesOwnersIds,
                            //                                                                                         GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                                         GeosApplication.Instance.CrmTopCustomers,
                            //                                                                                         items,
                            //                                                                                         GeosApplication.Instance.IdUserPermission));
                            //service GetDashboard2Details_IgnoreOfferCloseDate_V2360 updated with GetDashboard2Details_IgnoreOfferCloseDate_V2420  [rdixit][GEOS2-4712][22.08.2023]
                            #endregion                          
                            //CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_IgnoreOfferCloseDate_V2420(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, salesOwnersIds,
                            //                                                                                       GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                            //                                                                                       GeosApplication.Instance.CrmTopCustomers,
                            //                                                                                       items,
                            //                                                                                       GeosApplication.Instance.IdUserPermission));


                            //[Sudhir.Jangra][GEOS2-5310]
                            CustomersFromPlant.AddRange(crmControl.GetDashboard2Details_IgnoreOfferCloseDate_V2490(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, salesOwnersIds,
                                                                                                                   GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                                   GeosApplication.Instance.CrmTopCustomers,
                                                                                                                   items,
                                                                                                                   GeosApplication.Instance.IdUserPermission));

                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", items.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(items.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log("Get an error in FillDataByUser() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                            //CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", items.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(items.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log("Get an error in FillDataByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                            //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", items.Alias, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(items.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", items.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {
                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }

                    FillListByPlant();
                    if (CustomersOfferDashboard.Count > 0)
                    {
                        FillCompleteListOfTopCustomers();
                    }

                    //Companies = crmControl.GetSelectedUsersSitesTarget(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.CrmOfferYear, salesOwnersIds);
                    //service GetSelectedUsersSitesTargetNewCurrencyConversion updated with GetSelectedUsersSitesTargetNewCurrencyConversion_V2420  [rdixit][GEOS2-4712][22.08.2023]
                    Companies = crmControl.GetSelectedUsersSitesTargetNewCurrencyConversion_V2420(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate.Year, GeosApplication.Instance.SelectedyearEndDate.Year, salesOwnersIds);
                    CopyCompanies = new List<Company>(Companies);

                    GeosApplication.Instance.Logger.Log("Method FillDataByUser() executed successfully", category: Category.Info, priority: Priority.Low);
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByUser() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByUser() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        /// <summary>
        /// After Receiving Data from all plant, create Own list.
        /// </summary>
        private void FillListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillListByPlant ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in CustomersFromPlant)
                {
                    foreach (var itemCompany in item.Companies)
                    {
                        foreach (var itemSalesStatusType in itemCompany.SalesStatusTypes)
                        {
                            if (CustomersOfferDashboard.Any(cust => cust.IdCustomer == item.IdCustomer))
                            {
                                Customer custold = new Customer();
                                custold = CustomersOfferDashboard.Where(cust => cust.IdCustomer == item.IdCustomer).SingleOrDefault();

                                if (custold.Companies.Any(comp => comp.IdCompany == itemCompany.IdCompany))
                                {
                                    Company compold = new Company();
                                    compold = custold.Companies.Where(comp => comp.IdCompany == itemCompany.IdCompany).SingleOrDefault();

                                    if (compold.SalesStatusTypes.Any(ss => ss.Name == itemSalesStatusType.Name))
                                    {
                                        SalesStatusType salesStatusType = new SalesStatusType();
                                        salesStatusType = compold.SalesStatusTypes.Where(ss => ss.Name == itemSalesStatusType.Name).SingleOrDefault();
                                        salesStatusType.TotalAmount = salesStatusType.TotalAmount + Convert.ToDouble(itemSalesStatusType.TotalAmount);
                                        if (salesStatusType.IdSalesStatusType == 4 && item.CustomerName != "OTHERS")
                                        {
                                            if (Lstsort.Any(ls => ls.IdCustomer == custold.IdCustomer && ls.IdSite == compold.IdCompany))
                                            {
                                                CustomerSort custsort = Lstsort.Where(ls => ls.IdCustomer == custold.IdCustomer && ls.IdSite == compold.IdCompany).FirstOrDefault();
                                                custsort.Amount = custsort.Amount + Convert.ToDouble(itemSalesStatusType.TotalAmount);
                                            }
                                            else
                                            {
                                                CustomerSort custsort = new CustomerSort();
                                                custsort.IdCustomer = custold.IdCustomer;
                                                custsort.IdSite = compold.IdCompany;
                                                custsort.Amount = Convert.ToDouble(itemSalesStatusType.TotalAmount);
                                                Lstsort.Add(custsort);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        SalesStatusType salesStatusType = new SalesStatusType();
                                        salesStatusType.IdSalesStatusType = itemSalesStatusType.IdSalesStatusType;
                                        salesStatusType.Name = itemSalesStatusType.Name;
                                        salesStatusType.TotalAmount = itemSalesStatusType.TotalAmount;

                                        compold.SalesStatusTypes.Add(salesStatusType);
                                        if (salesStatusType.IdSalesStatusType == 4 && item.CustomerName != "OTHERS")
                                        {
                                            if (Lstsort.Any(ls => ls.IdCustomer == custold.IdCustomer && ls.IdSite == compold.IdCompany))
                                            {
                                                CustomerSort custsort = Lstsort.Where(ls => ls.IdCustomer == custold.IdCustomer && ls.IdSite == compold.IdCompany).FirstOrDefault();
                                                custsort.Amount = custsort.Amount + itemSalesStatusType.TotalAmount;
                                            }
                                            else
                                            {
                                                CustomerSort custsort = new CustomerSort();
                                                custsort.IdCustomer = custold.IdCustomer;
                                                custsort.IdSite = compold.IdCompany;
                                                custsort.Amount = itemSalesStatusType.TotalAmount;
                                                Lstsort.Add(custsort);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Company company = new Company();
                                    company.IdCompany = itemCompany.IdCompany;
                                    company.Name = itemCompany.Name;
                                    company.SiteNameWithoutCountry = itemCompany.SiteNameWithoutCountry;
                                    company.Country = new Country { Name = itemCompany.Country.Name };

                                    company.SalesStatusTypes = new List<SalesStatusType>() { new SalesStatusType { IdSalesStatusType = itemSalesStatusType.IdSalesStatusType, Name = itemSalesStatusType.Name, TotalAmount = itemSalesStatusType.TotalAmount } };
                                    if (itemSalesStatusType.IdSalesStatusType.ToString() == "4" && item.CustomerName != "OTHERS")
                                    {
                                        if (Lstsort.Any(ls => ls.IdCustomer == custold.IdCustomer && ls.IdSite == company.IdCompany))
                                        {
                                            CustomerSort custsort = Lstsort.Where(ls => ls.IdCustomer == custold.IdCustomer && ls.IdSite == company.IdCompany).FirstOrDefault();
                                            custsort.Amount = custsort.Amount + Convert.ToDouble(itemSalesStatusType.TotalAmount);
                                        }
                                        else
                                        {
                                            CustomerSort custsort = new CustomerSort();
                                            custsort.IdCustomer = custold.IdCustomer;
                                            custsort.IdSite = company.IdCompany;
                                            custsort.Amount = Convert.ToDouble(itemSalesStatusType.TotalAmount);
                                            Lstsort.Add(custsort);
                                        }
                                    }
                                    custold.Companies.Add(company);
                                }
                            }
                            else
                            {
                                Customer customer = new Customer();
                                customer.IdCustomer = item.IdCustomer;
                                customer.IdCompany = itemCompany.IdCompany;
                                customer.CustomerName = item.CustomerName;
                                customer.Companies = new List<Company>();
                                Company company = new Company();
                                company.IdCompany = itemCompany.IdCompany;
                                company.Name = itemCompany.Name;
                                company.SiteNameWithoutCountry = itemCompany.SiteNameWithoutCountry;
                                company.Country = new Country { Name = itemCompany.Country != null ? itemCompany.Country.Name : "" };

                                company.SalesStatusTypes = new List<SalesStatusType>() { new SalesStatusType { IdSalesStatusType = itemSalesStatusType.IdSalesStatusType, Name = itemSalesStatusType.Name, TotalAmount = itemSalesStatusType.TotalAmount } };
                                if (itemSalesStatusType.IdSalesStatusType.ToString() == "4" && item.CustomerName != "OTHERS")
                                {
                                    if (Lstsort.Any(ls => ls.IdCustomer == customer.IdCustomer && ls.IdSite == company.IdCompany))
                                    {
                                        CustomerSort custsort = Lstsort.Where(ls => ls.IdCustomer == customer.IdCustomer && ls.IdSite == company.IdCompany).FirstOrDefault();
                                        custsort.Amount = custsort.Amount + itemSalesStatusType.TotalAmount;
                                    }
                                    else
                                    {
                                        CustomerSort custsort = new CustomerSort();
                                        custsort.IdCustomer = customer.IdCustomer;
                                        custsort.IdSite = company.IdCompany;
                                        custsort.Amount = itemSalesStatusType.TotalAmount;
                                        Lstsort.Add(custsort);
                                    }
                                }

                                customer.Companies.Add(company);
                                CustomersOfferDashboard.Add(customer);
                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillListByPlant() ...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListByPlant() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// After creating our own list, Get Top Customers from same list.
        /// </summary>
        private void FillCompleteListOfTopCustomers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompleteListOfTopCustomers ...", category: Category.Info, priority: Priority.Low);

                List<Customer> remainingcustomerlst = new List<Customer>();
                Customer customer = new Customer();

                List<SalesStatusType> Lstsst = ListSalesStatusType.ToList(); // crmControl.GetAllSalesStatusType().ToList();

                remainingcustomerlst = CustomersOfferDashboard.Where(x => x.CustomerName != "OTHERS").ToList();

                List<Customer> sortedcustomerlst = new List<Customer>();

                var ddf = Lstsort.GroupBy(hj => new { hj.IdCustomer }).Select(hyt => new { IdCustomer = hyt.Key.IdCustomer, Amount = hyt.Sum(h => h.Amount) });

                if (ddf.Count() > GeosApplication.Instance.CrmTopCustomers)
                {
                    foreach (var itemsorted in ddf.OrderByDescending(hjk => hjk.Amount).Take(GeosApplication.Instance.CrmTopCustomers))
                    {
                        Customer sortedcustomer = remainingcustomerlst.Where(fgh => fgh.IdCustomer == itemsorted.IdCustomer).FirstOrDefault();
                        remainingcustomerlst.Remove(sortedcustomer);
                        sortedcustomerlst.Add(sortedcustomer);
                    }
                }
                else
                {
                    foreach (var itemsorted in ddf.OrderByDescending(hjk => hjk.Amount).Take(GeosApplication.Instance.CrmTopCustomers))
                    {
                        Customer sortedcustomer = remainingcustomerlst.Where(fgh => fgh.IdCustomer == itemsorted.IdCustomer).FirstOrDefault();
                        remainingcustomerlst.Remove(sortedcustomer);
                        sortedcustomerlst.Add(sortedcustomer);
                    }

                    if (sortedcustomerlst.Count() < GeosApplication.Instance.CrmTopCustomers)
                    {
                        List<Customer> custs = remainingcustomerlst.Take(GeosApplication.Instance.CrmTopCustomers - sortedcustomerlst.Count()).ToList();
                        foreach (var itemremaining in custs)
                        {
                            sortedcustomerlst.Add(itemremaining);
                            remainingcustomerlst.Remove(itemremaining);
                        }
                    }
                }

                List<Customer> custtopCust = CustomersOfferDashboard.Where(x => x.CustomerName == "OTHERS").ToList();
                CustomersOfferDashboard = sortedcustomerlst;
                if (custtopCust.Count > 0)
                {
                    Customer custother = null;
                    custother = new Customer();
                    custother.IdCustomer = 0;
                    custother.IdCompany = 0;
                    custother.CustomerName = "OTHERS";
                    custother.Companies = new List<Company>();
                    Company company = new Company();
                    company.IdCompany = 0;
                    company.Name = "";

                    company.SalesStatusTypes = new List<SalesStatusType>();
                    custother.Companies.Add(company);
                    foreach (var item in custtopCust)
                    {
                        foreach (SalesStatusType itemsst in Lstsst)
                        {
                            if (item.Companies[0].SalesStatusTypes.Any(ss => ss.Name == itemsst.Name.ToString()))
                            {
                                if (company.SalesStatusTypes.Any(ss => ss.Name == itemsst.Name.ToString()))
                                {
                                    SalesStatusType salesStatusTypeother = new SalesStatusType();
                                    salesStatusTypeother = company.SalesStatusTypes.Where(ss => ss.Name == itemsst.Name.ToString()).SingleOrDefault();
                                    SalesStatusType salesStatusType = new SalesStatusType();
                                    salesStatusType = item.Companies[0].SalesStatusTypes.Where(ss => ss.Name == itemsst.Name.ToString()).SingleOrDefault();
                                    salesStatusTypeother.TotalAmount = salesStatusTypeother.TotalAmount + salesStatusType.TotalAmount;
                                }
                                else
                                {
                                    SalesStatusType salesStatusType = new SalesStatusType();
                                    salesStatusType = item.Companies[0].SalesStatusTypes.Where(ss => ss.Name == itemsst.Name.ToString()).SingleOrDefault();
                                    company.SalesStatusTypes.Add(salesStatusType);
                                }
                            }
                        }
                    }
                    CustomersOfferDashboard.Add(custother);
                }

                if (remainingcustomerlst.Count() > 0)
                {
                    if (CustomersOfferDashboard.Any(cust => cust.CustomerName == "OTHERS"))
                    {
                        Customer getcustother = CustomersOfferDashboard.Where(cust => cust.CustomerName == "OTHERS").FirstOrDefault();
                        foreach (SalesStatusType itemsst in Lstsst)
                        {
                            if (getcustother.Companies[0].SalesStatusTypes.Any(ss => ss.Name == itemsst.Name.ToString()))
                            {
                                SalesStatusType salesStatusType = new SalesStatusType();
                                salesStatusType = getcustother.Companies[0].SalesStatusTypes.Where(ss => ss.Name == itemsst.Name.ToString()).SingleOrDefault();
                                salesStatusType.TotalAmount = salesStatusType.TotalAmount + remainingcustomerlst.SelectMany(rcl => rcl.Companies.SelectMany(ghju => ghju.SalesStatusTypes.Where(dwe => dwe.IdSalesStatusType == itemsst.IdSalesStatusType).Select(jg => jg.TotalAmount))).Sum();
                            }
                            else
                            {
                                SalesStatusType salesStatusType = new SalesStatusType();
                                salesStatusType.IdSalesStatusType = itemsst.IdSalesStatusType;
                                salesStatusType.Name = itemsst.Name.ToString();
                                salesStatusType.TotalAmount = remainingcustomerlst.SelectMany(rcl => rcl.Companies.SelectMany(ghju => ghju.SalesStatusTypes.Where(dwe => dwe.IdSalesStatusType == itemsst.IdSalesStatusType).Select(jg => jg.TotalAmount))).Sum();
                                getcustother.Companies[0].SalesStatusTypes.Add(salesStatusType);
                            }
                        }
                    }
                    else
                    {
                        customer = new Customer();
                        customer.IdCustomer = 0;
                        customer.IdCompany = 0;
                        customer.CustomerName = "OTHERS";
                        customer.Companies = new List<Company>();
                        Company company = new Company();
                        company.IdCompany = 0;
                        company.Name = "";

                        company.SalesStatusTypes = new List<SalesStatusType>();

                        foreach (SalesStatusType itemsst in Lstsst)
                        {
                            SalesStatusType salesStatusType = new SalesStatusType();
                            salesStatusType.IdSalesStatusType = itemsst.IdSalesStatusType;
                            salesStatusType.Name = itemsst.Name;
                            salesStatusType.TotalAmount = remainingcustomerlst.SelectMany(rcl => rcl.Companies.SelectMany(ghju => ghju.SalesStatusTypes.Where(dwe => dwe.IdSalesStatusType == itemsst.IdSalesStatusType).Select(jg => jg.TotalAmount))).Sum();
                            company.SalesStatusTypes.Add(salesStatusType);
                        }

                        if (company.SalesStatusTypes.Count > 0)
                        {
                            customer.Companies.Add(company);
                            CustomersOfferDashboard.Add(customer);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillCompleteListOfTopCustomers()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompleteListOfTopCustomers() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fill Dashboard is used to fill data in DataTable.
        /// </summary>
        private void FillDashboard()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
                GraphDataTable.Rows.Clear();
                DtTable.Rows.Clear();
                string groupName;
                int i = 1;

                foreach (Customer item in CustomersOfferDashboard)
                {
                    DataRow dtRowParent = DtTable.NewRow();     // Parent
                    DataRow graph = GraphDataTable.NewRow();

                    if (item.Companies.Count > 1)
                    {
                        groupName = "Group" + i.ToString();
                        dtRowParent["IdCompany"] = groupName;
                        dtRowParent["IdCustomer"] = 0;
                        dtRowParent["CustomerName"] = item.CustomerName.ToString();

                        graph["IdCompany"] = groupName;
                        graph["IdCustomer"] = 0;
                        graph["CustomerName"] = item.CustomerName.ToString();

                        DtTable.Rows.Add(dtRowParent);
                        GraphDataTable.Rows.Add(graph);

                        foreach (var company in item.Companies)     // Add Childs in Parent
                        {
                            DataRow dtRowChild = DtTable.NewRow();

                            dtRowChild["IdCompany"] = company.IdCompany.ToString();
                            dtRowChild["IdCustomer"] = groupName;
                            dtRowChild["CustomerName"] = company.Name.ToString();

                            if (Companies.Any(cl => cl.IdCompany == company.IdCompany))
                            {
                                dtRowChild["TARGET"] = Convert.ToDouble(Companies.Where(cl => cl.IdCompany == company.IdCompany).Select(complst => complst.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum());
                                if (dtRowParent["TARGET"].ToString() == "" || dtRowParent["TARGET"].ToString() == null)
                                {
                                    dtRowParent["TARGET"] = Companies.Where(cl => cl.IdCompany == company.IdCompany).Select(complst => complst.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum();
                                    graph["TARGET"] = Companies.Where(cl => cl.IdCompany == company.IdCompany).Select(complst => complst.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum();
                                }
                                else
                                {
                                    dtRowParent["TARGET"] = Convert.ToDecimal(dtRowParent["TARGET"].ToString()) + Companies.Where(cl => cl.IdCompany == company.IdCompany).Select(complst => complst.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum();
                                    graph["TARGET"] = Convert.ToDecimal(graph["TARGET"].ToString()) + Companies.Where(cl => cl.IdCompany == company.IdCompany).Select(complst => complst.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum(); ;
                                }
                                //foreach (var itemrem in Companies.Where(cl => cl.IdCompany == company.IdCompany).ToList())
                                //{
                                //    Companies.Remove(itemrem);
                                //}
                                Companies.RemoveAll(cl => cl.IdCompany == company.IdCompany);
                            }

                            foreach (var salesStatus in company.SalesStatusTypes)
                            {
                                if (salesStatus.Name != null)
                                {
                                    dtRowChild[salesStatus.Name] = salesStatus.TotalAmount;
                                    if (dtRowParent[salesStatus.Name].ToString() == "" || dtRowParent[salesStatus.Name].ToString() == null)
                                    {
                                        dtRowParent[salesStatus.Name] = salesStatus.TotalAmount;
                                        graph[salesStatus.Name] = salesStatus.TotalAmount;
                                    }
                                    else
                                    {
                                        dtRowParent[salesStatus.Name] = Convert.ToDouble(dtRowParent[salesStatus.Name].ToString()) + salesStatus.TotalAmount;
                                        graph[salesStatus.Name] = Convert.ToDouble(graph[salesStatus.Name].ToString()) + salesStatus.TotalAmount;
                                    }
                                }
                            }

                            DtTable.Rows.Add(dtRowChild);
                        }

                        i++;
                    }
                    else
                    {
                        groupName = "Group" + i.ToString();
                        if (item.CustomerName != "OTHERS")      // Comapany group contains single customer
                        {
                            DataRow dtRowParentSingleChild = DtTable.NewRow();    // Used to add parent which contain only one child.

                            dtRowParentSingleChild["IdCompany"] = groupName;
                            dtRowParentSingleChild["IdCustomer"] = 0;
                            dtRowParentSingleChild["CustomerName"] = item.CustomerName.ToString();

                            graph["IdCompany"] = groupName;
                            graph["IdCustomer"] = 0;

                            DataRow dtRowChild = DtTable.NewRow();
                            //Companies.Select(I=>I.SalesTargetBySite.TargetAmountWithExchangeRate).Sum()
                            if (Companies.Any(cl => cl.IdCompany == item.Companies[0].IdCompany))
                            {
                                dtRowParentSingleChild["TARGET"] = Convert.ToDouble(Companies.Where(cl => cl.IdCompany == item.Companies[0].IdCompany).Select(complst => complst.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum());
                                graph["TARGET"] = Convert.ToDouble(dtRowParentSingleChild["TARGET"]);
                                dtRowChild["TARGET"] = Convert.ToDouble(dtRowParentSingleChild["TARGET"]);
                                //foreach (var itemrem in Companies.Where(cl => cl.IdCompany == item.Companies[0].IdCompany).ToList())
                                //{
                                //    Companies.Remove(itemrem);
                                //}
                                Companies.RemoveAll(cl => cl.IdCompany == item.Companies[0].IdCompany);
                            }

                            DtTable.Rows.Add(dtRowParentSingleChild);
                            i++;

                            dtRowChild["IdCompany"] = item.IdCompany.ToString();
                            dtRowChild["IdCustomer"] = groupName;
                            dtRowChild["CustomerName"] = item.Companies[0].Name.ToString();

                            graph["CustomerName"] = item.CustomerName.ToString();

                            foreach (var salesStatus in item.Companies[0].SalesStatusTypes)
                            {
                                dtRowParentSingleChild[salesStatus.Name] = item.Companies[0].SalesStatusTypes.Where(CO => CO.Name == salesStatus.Name).Select(vb => vb.TotalAmount).Sum();
                                if (salesStatus.Name != null)
                                {
                                    dtRowChild[salesStatus.Name] = salesStatus.TotalAmount;
                                    graph[salesStatus.Name] = salesStatus.TotalAmount;
                                }
                            }

                            DtTable.Rows.Add(dtRowChild);
                            GraphDataTable.Rows.Add(graph);
                        }
                        else    // Comapany group contains single customer Others
                        {
                            DataRow dtRowParentOthers = DtTable.NewRow();

                            dtRowParentOthers["IdCompany"] = "GroupOther";
                            dtRowParentOthers["IdCustomer"] = -1;
                            dtRowParentOthers["CustomerName"] = "OTHERS";

                            graph["IdCompany"] = "GroupOther";
                            graph["IdCustomer"] = -1;
                            graph["CustomerName"] = "OTHERS";

                            if (Companies.Count > 0)
                            {
                                dtRowParentOthers["TARGET"] = Convert.ToDouble(Companies.Select(comp => comp.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum());
                                graph["TARGET"] = Convert.ToDouble(Companies.Select(comp => comp.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum());
                            }

                            foreach (var salesStatus in item.Companies[0].SalesStatusTypes)
                            {
                                if (salesStatus.Name != null)
                                {
                                    dtRowParentOthers[salesStatus.Name] = salesStatus.TotalAmount;
                                    graph[salesStatus.Name] = salesStatus.TotalAmount;
                                }
                            }

                            DtTable.Rows.Add(dtRowParentOthers);
                            GraphDataTable.Rows.Add(graph);
                        }
                    }
                }

                // This part is added because Target values are not matching in Dashboard1 and Dashboard2.
                // Because 
                if (Companies != null && Companies.Count > 0)
                {
                    DataRow dataRow = DtTable.AsEnumerable().FirstOrDefault(row => row["CustomerName"].ToString() == "OTHERS");
                    if (dataRow == null)
                    {
                        DataRow dtRowParentOthers = DtTable.NewRow();
                        dtRowParentOthers["IdCompany"] = "GroupOther";  // groupName;
                        dtRowParentOthers["IdCustomer"] = -1;
                        dtRowParentOthers["CustomerName"] = "OTHERS";

                        DataRow graph = GraphDataTable.NewRow();
                        graph["IdCompany"] = "GroupOther";              // groupName;
                        graph["IdCustomer"] = -1;
                        graph["CustomerName"] = "OTHERS";

                        if (Companies.Count > 0)
                        {
                            dtRowParentOthers["TARGET"] = Companies.Select(comp => comp.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum();
                            graph["TARGET"] = Companies.Select(comp => comp.SalesTargetBySite.TargetAmountWithExchangeRate).ToList().Sum();
                        }

                        DtTable.Rows.Add(dtRowParentOthers);
                        GraphDataTable.Rows.Add(graph);
                    }
                }

                CopyMainGraphDataTable = GraphDataTable.Copy();
                GraphDataTable = CopyMainGraphDataTable;
                GeosApplication.Instance.Logger.Log("Method FillDashboard() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is currently unused.
        /// </summary>
        /// <param name="e"></param>
        private void ChartDashboardGridNodeCustomSummarydAction(TreeListCustomSummaryEventArgs e)
        {
            //DevExpress.Xpf.Grid.TreeList.TreeListCustomSummaryEventArgs e = (DevExpress.Xpf.Grid.TreeList.TreeListCustomSummaryEventArgs)obj;

            if (e.SummaryProcess == CustomSummaryProcess.Start && e.Node == null)
            {
                CustomSummary[((GridSummaryItem)e.SummaryItem).FieldName] = 0.0;
            }

            if (e.SummaryProcess == CustomSummaryProcess.Calculate && e.Node.Level == 0)
            {
                CustomSummary[((GridSummaryItem)e.SummaryItem).FieldName] = CustomSummary[((GridSummaryItem)e.SummaryItem).FieldName] + (double)e.FieldValue;
                e.TotalValue = CustomSummary[((GridSummaryItem)e.SummaryItem).FieldName];
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize && e.Node == null)
            {
                e.TotalValue = (double)CustomSummary[((GridSummaryItem)e.SummaryItem).FieldName];
            }
        }

        private void PrintGrid(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintGrid ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                PrintableControlLink pcl = new PrintableControlLink((TreeListView)obj);

                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TreeListView)obj).Resources["PageHeader"];
                pcl.PageFooterTemplate = (DataTemplate)((TreeListView)obj).Resources["PageFooter"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintGrid() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void ExportLeadsGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLeadsGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "Dashboard2";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                DialogResult = SaveFileDialogService.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        // DXSplashScreen.Show<SplashScreenView>(); 
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

                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;

                    TreeListView treeListView = (TreeListView)obj;
                    treeListView.ShowTotalSummary = false;
                    treeListView.ExpandAllNodes();
                    treeListView.ExportToXlsx(ResultFileName);
                    treeListView.CollapseAllNodes();

                    SpreadsheetControl control = new SpreadsheetControl();
                    control.LoadDocument(ResultFileName);
                    Worksheet worksheet = control.ActiveWorksheet;

                    DevExpress.Spreadsheet.CellRange supplierNamesRange = worksheet.Range["A1:Z1"];
                    supplierNamesRange.Font.Bold = true;

                    // Split all merged cells in the worksheet. 
                    foreach (var item in worksheet.Cells.GetMergedRanges())
                    {
                        item.UnMerge();
                    }

                    control.SaveDocument();

                    IsBusy = false;
                    treeListView.ShowTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportLeadsGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportLeadsGridButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        #endregion // Methods
    }
}
