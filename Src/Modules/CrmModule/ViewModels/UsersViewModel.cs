using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    /// <summary>
    /// This section is only visible to user with Admin permission. idPermission - 9 
    /// </summary>
    public class UsersViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region  public ICommand
        public ICommand AddUserCommand { get; set; }
        public ICommand RefreshUserViewCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand EditUserGridDoubleClickCommand { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }
        #endregion

        #region Declaration
        private string userEditViewTitle;
        private bool isUserViewColumnChooserVisible;
        private ObservableCollection<SalesUser> listUsers;
        private string myFilterString;
        private bool isBusy;
        private DataRowView selectedObject;
        private string salesQuotaViewTitle;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
       
        DataTable dtUsersList;
        DataTable dtUsersListCopy;
        private bool isAddUserEnable;
        public ObservableCollection<Summary> totalSummary;
        #endregion

        #region  public Properties
        public IList<Currency> Currencies { get; set; }
        public Int32 StartYear { get; set; }
        public Int32 EndYear { get; set; }
        public string UserEditViewTitle
        {
            get { return userEditViewTitle; }
            set
            {
                userEditViewTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserEditViewTitle"));
            }
        }
        public string SalesQuotaViewTitle
        {
            get { return salesQuotaViewTitle; }
            set
            {
                salesQuotaViewTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesQuotaViewTitle"));
            }
        }
        public DataRowView SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
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
        public ObservableCollection<Summary> TotalSummary
        {
            get { return totalSummary; }
            set
            {
                totalSummary = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalSummary"));
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
        public bool IsUserViewColumnChooserVisible
        {
            get { return isUserViewColumnChooserVisible; }
            set
            {
                isUserViewColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUserViewColumnChooserVisible"));
            }
        }

        public ObservableCollection<SalesUser> ListUsers
        {
            get { return listUsers; }
            set { listUsers = value; OnPropertyChanged(new PropertyChangedEventArgs("ListUsers")); }
        }

        // Export Excel .xlsx
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        public bool IsAddUserEnable
        {
            get
            {
                return isAddUserEnable;
            }

            set
            {
                isAddUserEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddUserEnable"));
            }
        }
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }
        public DataTable DtUsersList
        {
            get
            {
                return dtUsersList;
            }

            set
            {
                dtUsersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtUsersList"));
            }
        }

        public DataTable DtUsersListCopy
        {
            get
            {
                return dtUsersListCopy;
            }

            set
            {
                dtUsersListCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtUsersListCopy"));
            }
        }
        #endregion

        #region Event

        public event EventHandler RequestClose;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public UsersViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor UsersViewModel ...", category: Category.Info, priority: Priority.Low);
            CustomCellAppearanceCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);
            AddUserCommand = new RelayCommand(new Action<object>(AddUserViewWindowShow));
            RefreshUserViewCommand = new Prism.Commands.DelegateCommand<object>(RefreshUserDetails);
            PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportUserViewGridButtonCommandAction));
            HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
            EditUserGridDoubleClickCommand = new DelegateCommand<RowDoubleClickEventArgs>(UserEditViewWindowShow);
            StartYear = GeosApplication.Instance.SelectedyearStarDate.Year;
            EndYear = GeosApplication.Instance.SelectedyearEndDate.Year;
            AddColumnsToDataTable();
            FillUsers();
            FillCurrenciesExchangeRate();
            IsAddUserEnable = GeosApplication.Instance.IsPermissionAdminOnly;

            GeosApplication.Instance.Logger.Log("Constructor UsersViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        #endregion

        #region Methods

        // This function use to create the dynamic column 
        //[CRM-M045-06] Changes in sales quota column in sales grid

        private void AddColumnsToDataTableOld()
        {
            GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);

            CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
            {
              new Emdep.Geos.UI.Helper.Column() { FieldName="Gender",HeaderText="", Settings = SettingsType.Image, AllowCellMerge=false, Width=40,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="FirstName",HeaderText="First Name", Settings = SettingsType.Default, AllowCellMerge=false, Width=200,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="LastName",HeaderText="Last Name", Settings = SettingsType.Default, AllowCellMerge=false, Width=200,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="Telephone",HeaderText="Telephone", Settings = SettingsType.Default, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="Email",HeaderText="Email", Settings = SettingsType.Email, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="Office",HeaderText="Office", Settings = SettingsType.Default, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="Plant",HeaderText="Plant", Settings = SettingsType.Default, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="IdSalesUser",HeaderText="IdSalesUser", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName="IdCurrency",HeaderText="IdCurrency", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=150,AllowEditing=false,IsVertical= false,FixedWidth=true,AllowBestFit=true},
              new Emdep.Geos.UI.Helper.Column() { FieldName = "MaxDiscountPermitted", HeaderText = "Max Discount Permitted", Settings = SettingsType.MaxDiscount, AllowCellMerge = false, Width = 200, AllowEditing = false, IsVertical = false, FixedWidth = true, AllowBestFit = true }
            };

            DtUsersList = new DataTable();
            DtUsersList.Columns.Add("Gender", typeof(string));
            DtUsersList.Columns.Add("FirstName", typeof(string));
            DtUsersList.Columns.Add("LastName", typeof(string));
            DtUsersList.Columns.Add("Telephone", typeof(string));
            DtUsersList.Columns.Add("Email", typeof(string));
            DtUsersList.Columns.Add("Office", typeof(string));
            DtUsersList.Columns.Add("Plant", typeof(string));
            DtUsersList.Columns.Add("IdSalesUser", typeof(Int32));
            DtUsersList.Columns.Add("IdCurrency", typeof(Int32));
            DtUsersList.Columns.Add("MaxDiscountPermitted", typeof(int));

            TotalSummary = new ObservableCollection<Summary>();
            TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "FirstName", DisplayFormat = "Total: {0}" });

            for (Int64 i = EndYear; i >= StartYear; i--)
            {
                DtUsersList.Columns.Add(i.ToString(), typeof(double));
                for (int j = 0; j < 1; j++)
                {
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = string.Concat(i), HeaderText = string.Concat("Sales Quota(" + i+")"), Settings = SettingsType.Amount, AllowCellMerge = false, Width = 200, AllowEditing = false, IsVertical = false, FixedWidth = true, AllowBestFit = true });
                    TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = i.ToString(), DisplayFormat = " {0:C}" });
                }
            }
            GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        // shubham[skadam] GEOS2-3888 Improvement in  user Sales Quota (better visibility) - 2  02 Sep 2022
        #region GEOS2-3888 Improvement in  user Sales Quota (better visibility) - 2 
        private ObservableCollection<ParentBandItem> bands_FirstLevel = new ObservableCollection<ParentBandItem>();
        public ObservableCollection<ParentBandItem> Bands_FirstLevel
        {
            get { return bands_FirstLevel; }
            set
            {
                bands_FirstLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands_FirstLevel"));
            }
        }
        private void AddColumnsToDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);

                CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
                TotalSummary = new ObservableCollection<Summary>();
                if (Bands_FirstLevel == null)
                {
                    Bands_FirstLevel = new ObservableCollection<ParentBandItem>();
                }
                else
                {
                    Bands_FirstLevel.Clear();
                }
                ParentBandItem ParentBandItem1 = new ParentBandItem()
                {
                    Header = "",
                    Name = "ParentBandItem1",
                    HeaderToolTip = ""
                };
                Bands_FirstLevel.Add(ParentBandItem1);

                BandItem band1 = new BandItem() { BandHeader = "" };
                band1.Columns = new ObservableCollection<ColumnItem>();
                band1.Columns.Add(new ColumnItem() { ColumnFieldName ="Gender",HeaderText="", Settings = SettingsType.Image                 ,Visible = true  ,Width = 50, IsVertical = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName ="FirstName",HeaderText="First Name", Settings = SettingsType.Default  ,Visible = true  ,Width = 200, IsVertical = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName ="LastName",HeaderText="Last Name", Settings = SettingsType.Default    ,Visible = true  ,Width = 200, IsVertical = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName ="Telephone",HeaderText="Telephone", Settings = SettingsType.Default   ,Visible = true  ,Width = 200, IsVertical = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Email", HeaderText = "Email", Settings = SettingsType.Email         ,Visible = true  ,Width = 200, IsVertical = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName ="Office",HeaderText="Office", Settings = SettingsType.Default         ,Visible = true  ,Width = 50, IsVertical = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName ="Plant",HeaderText="Plant", Settings = SettingsType.Default           ,Visible = true  ,Width = 50, IsVertical = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName ="IdSalesUser",HeaderText="IdSalesUser", Settings = SettingsType.Hidden,Visible = false ,Width = 200, IsVertical = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName ="IdCurrency",HeaderText="IdCurrency", Settings = SettingsType.Hidden  ,Visible = false, Width = 200, IsVertical = false });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "MaxDiscountPermitted", HeaderText = "Max Discount Permitted", Settings = SettingsType.MaxDiscount, Visible = true, Width = 200, IsVertical = false });

                Bands_FirstLevel[0].Bands.Add(band1);
                DtUsersList = new DataTable();
                DtUsersList.Columns.Add("Gender", typeof(string));
                DtUsersList.Columns.Add("FirstName", typeof(string));
                DtUsersList.Columns.Add("LastName", typeof(string));
                DtUsersList.Columns.Add("Telephone", typeof(string));
                DtUsersList.Columns.Add("Email", typeof(string));
                DtUsersList.Columns.Add("Office", typeof(string));
                DtUsersList.Columns.Add("Plant", typeof(string));
                DtUsersList.Columns.Add("IdSalesUser", typeof(Int32));
                DtUsersList.Columns.Add("IdCurrency", typeof(Int32));
                DtUsersList.Columns.Add("MaxDiscountPermitted", typeof(int));

                List<int> YearsList = new List<int>();

                for (Int64 i = EndYear; i >= StartYear; i--)
                {
                    if (!YearsList.Contains(Convert.ToInt32(i)) && i != 0)
                    {
                        YearsList.Add(Convert.ToInt32(i));
                    }
                }

                foreach (var item in YearsList)
                {
                    ParentBandItem ParentBandItem = new ParentBandItem()
                    {
                        Header = $"{item.ToString()}",
                        Name = $"Name{item.ToString()}",
                        HeaderToolTip = $"{item.ToString()}"
                    };
                    Bands_FirstLevel.Add(ParentBandItem);
                }

                string WMSCurrencyName = GeosApplication.Instance.Currencies.FirstOrDefault(x => x.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion).Name;
                foreach (var item in YearsList)
                {
                    BandItem bandExchangeRate = new BandItem()
                    {
                        BandName = "Band_ExchangeRate",
                        BandHeader = "Exchange Rate"
                    };

                    BandItem bandTotal = new BandItem()
                    {
                        BandName = "Band_Total",
                        BandHeader = "Sales Quota"
                    };
                    bandTotal.Columns = new ObservableCollection<ColumnItem>();
                    bandExchangeRate.Columns = new ObservableCollection<ColumnItem>();
                    //string totalSummaryField = "Total" + "-" + item.ToString();
                    string totalSummaryField = item.ToString();
                    if (!DtUsersList.Columns.Contains(("Total" + "-" + item).ToString()))
                    {

                        DtUsersList.Columns.Add(totalSummaryField, typeof(double)).AllowDBNull = true;
                        bandTotal.Columns.Add(new ColumnItem() { ColumnFieldName = totalSummaryField, HeaderText = totalSummaryField + "(" + WMSCurrencyName + ")", Width = 120, IsVertical = false, Settings = SettingsType.Amount, Visible = true });

                        string totalSummaryFieldConvertedAmount = "Total" + "-" + item.ToString() + "ConvertedAmount";
                        DtUsersList.Columns.Add(totalSummaryFieldConvertedAmount, typeof(double)).AllowDBNull = true;

                        DtUsersList.Columns.Add(totalSummaryFieldConvertedAmount + "WithSymbol", typeof(string)).AllowDBNull = true;


                        bandTotal.Columns.Add(new ColumnItem()
                        {
                            ColumnFieldName = totalSummaryFieldConvertedAmount + "WithSymbol",
                            HeaderText = totalSummaryField,
                            Width = 120,
                            IsVertical = false,
                            Settings = SettingsType.Amount,
                            Visible = true,
                            Tag = totalSummaryFieldConvertedAmount
                        });

                    }

                    if (!DtUsersList.Columns.Contains(("ExchangeRateDate" + "-" + item).ToString()))
                    {
                        string ExchangeRateDateField = "ExchangeRateDate" + "-" + item.ToString();
                        DtUsersList.Columns.Add(ExchangeRateDateField, typeof(DateTime)).AllowDBNull = true;
                        bandExchangeRate.Columns.Add(new ColumnItem() { ColumnFieldName = ExchangeRateDateField, HeaderText = "Date", Width = 100, IsVertical = false, Settings = SettingsType.CloseDate, Visible = true });
                    }

                    if (!DtUsersList.Columns.Contains(("ExchangeRate4D" + "-" + item).ToString()))
                    {
                        string ExchangeRateField = "ExchangeRate4D" + "-" + item.ToString();
                        DtUsersList.Columns.Add(ExchangeRateField, typeof(double)).AllowDBNull = true;
                        bandExchangeRate.Columns.Add(new ColumnItem() { ColumnFieldName = ExchangeRateField, HeaderText = "Rate", Width = 80, IsVertical = false, Settings = SettingsType.Amount, Visible = true });
                    }

                    if (bandTotal.Columns.Count > 0)
                    {
                        if (item != 0)
                        {
                            var parentBandItem1 = Bands_FirstLevel.FirstOrDefault(
                            x => (x.Header.ToString() == item.ToString()));

                            if (parentBandItem1 != null)
                            {
                                parentBandItem1.Bands.Add(bandTotal);
                            }
                        }
                    }

                    if (bandExchangeRate.Columns.Count > 0)
                    {
                        if (item != 0)
                        {
                            var parentBandItem1 = Bands_FirstLevel.First(
                            x => (x.Header.ToString() == item.ToString()));

                            if (parentBandItem1 != null)
                            {
                                parentBandItem1.Bands.Add(bandExchangeRate);
                            }
                        }
                    }
                    TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = item.ToString(), DisplayFormat = " {0:C}" });
                }
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "FirstName", DisplayFormat = "Total: {0}" });
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        private void FillCurrenciesExchangeRate()
        {
            GeosApplication.Instance.Logger.Log("Method FillCurrenciesExchangeRate...", category: Category.Info, priority: Priority.Low);

            try
            {
                Currencies = crmControl.GetCurrencyByExchangeRate();

                GeosApplication.Instance.Logger.Log("Method FillCurrenciesExchangeRate executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrenciesExchangeRate() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrenciesExchangeRate() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrenciesExchangeRate() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void UserEditViewWindowShow(RowDoubleClickEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UserEditViewWindowShow...", category: Category.Info, priority: Priority.Low);

                TableView detailView = e.HitInfo.Column.View as TableView;
                DataRowView data = (DataRowView)(detailView.DataControl as GridControl).GetRow(e.HitInfo.RowHandle);
                
                int IdsalesUser = Convert.ToInt32(data.Row.ItemArray[7]); 

                //long offerId = Convert.ToInt64(((System.Data.DataRowView)detailView.FocusedRow).Row.ItemArray[1].ToString());
                SalesUser saledata = ListUsers.AsEnumerable().FirstOrDefault(row => row.IdSalesUser == IdsalesUser);

                if (saledata != null)
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

                    EditUserViewModel edituserViewModel = new EditUserViewModel();
                    EditUserView editUserView = new EditUserView();
                    edituserViewModel.InIt(saledata);
                    EventHandler handle = delegate { editUserView.Close(); };
                    edituserViewModel.RequestClose += handle;
                    editUserView.DataContext = edituserViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }

                    //IsUserViewColumnChooserVisible = false;
                    var ownerInfo = (e.OriginalSource as FrameworkElement);
                    editUserView.Owner = Window.GetWindow(ownerInfo);
                    editUserView.ShowDialog();

                    if (edituserViewModel.IsUpdateUser)
                    {
                        saledata.People.IdPerson = edituserViewModel.IdUser;
                        saledata.SalesUserQuotas = edituserViewModel.SalesUserQuotas.ToList();

                        saledata.Company.ShortName = edituserViewModel.ListPlants[edituserViewModel.SelectedIndexCompanyPlant].ShortName;
                        saledata.LookupValue.Value = edituserViewModel.SalesUnitList[edituserViewModel.SelectedIndexSalesUnit].Value;
                        saledata.LookupValue.IdLookupValue = edituserViewModel.SalesUnitList[edituserViewModel.SelectedIndexSalesUnit].IdLookupValue;
                        saledata.MaxDiscountAllowed = edituserViewModel.SalesMaxDiscount;
                        DataRow dataRow = DtUsersList.AsEnumerable().FirstOrDefault(row => Convert.ToInt64(row["IdSalesUser"]) == saledata.IdSalesUser);
                                            
                        dataRow["FirstName"] = saledata.People.Name.ToString();
                        dataRow["LastName"] = saledata.People.Surname.ToString();
                        if (saledata.People.Phone != null)
                            dataRow["Telephone"] = saledata.People.Phone.ToString();
                        if (saledata.People.Email != null)
                            dataRow["Email"] = saledata.People.Email.ToString();
                        if (saledata.People.Company.ShortName != null)
                            dataRow["Office"] = saledata.People.Company.ShortName.ToString();
                        if (saledata.Company.ShortName != null)
                            dataRow["Plant"] = saledata.Company.ShortName.ToString();
                        if(saledata.MaxDiscountAllowed!=null)
                            dataRow["MaxDiscountPermitted"]=saledata.MaxDiscountAllowed;

                        foreach (SalesUserQuota item in edituserViewModel.SalesUserQuotas)
                        {                        
                            if (dataRow != null)
                            {
                                if (dataRow.Table.Columns.Contains(item.Year.ToString()))
                                    dataRow[string.Concat(item.Year)] = item.SalesQuotaAmountWithExchangeRate;
                                dataRow["ExchangeRateDate-" + item.Year.ToString()] = item.ExchangeRateDate;
                                //dataRow["ExchangeRate4D-" + item.Year.ToString()] = item.CurrencyConversionRate.ToString("N4");
                                dataRow["Total-" + item.Year.ToString() + "ConvertedAmount"] = item.SalesQuotaAmount.ToString("N2");
                                dataRow["Total-" + item.Year.ToString() + "ConvertedAmountWithSymbol"] = GeosApplication.Instance.Currencies.FirstOrDefault(curr => curr.IdCurrency == item.IdSalesQuotaCurrency).Symbol
                                                                                                                 + " " +
                                                                                                                  item.SalesQuotaAmount.ToString("N2");
                            }
                        }
                    }

                    ListUsers = new ObservableCollection<SalesUser>(ListUsers);
                 //   SelectedObject = saledata;

                    // code for hide column chooser if empty

                    //int visibleFalseCoulumn = 0;
                    //foreach (GridColumn column in gridControl.Columns)
                    //{
                    //    if (column.Visible == false)
                    //    {
                    //        visibleFalseCoulumn++;
                    //    }
                    //}
                    //if (visibleFalseCoulumn > 0)
                    //{
                    //    IsUserViewColumnChooserVisible = true;
                    //}
                    //else
                    //{
                    //    IsUserViewColumnChooserVisible = false;
                    //}
                    detailView.Focus();
                    DtUsersList.AcceptChanges();
                }
                GeosApplication.Instance.Logger.Log("Method UserEditViewWindowShow executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UserEditViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UserEditViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UserEditViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill all contact list depend on Sales Owner. 
        /// [001][cpatil][16-07-2020][GEOS2-15]Users without sales quota are not displayed in users grid
        /// </summary>
        private void FillUsers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillUsers ...", category: Category.Info, priority: Priority.Low);
                //[001] Changed service method
                // shubham[skadam] GEOS2-3888 Improvement in  user Sales Quota (better visibility) - 2  05 Sep 2022
                ListUsers = new ObservableCollection<SalesUser>(crmControl.GetAllSalesUserQuotaPeopleDetails_V2301(GeosApplication.Instance.IdCurrencyByRegion, StartYear,EndYear));

                if (ListUsers != null && ListUsers.Count > 0)
                {
                    DtUsersList.Rows.Clear();
                    DtUsersListCopy = DtUsersList.Copy();
                    //  OfferTargetForecast = OfferTargetForecast.OrderBy(x => x.Site.Customers[0].CustomerName).ToList();

                    foreach (SalesUser item in ListUsers)
                    {
                        DataRow dr = DtUsersListCopy.NewRow();
                        if (item.People.UserGender != null)
                            dr["Gender"] = item.People.UserGender.ToString();
                        if (item.People.Name != null)
                            dr["FirstName"] = item.People.Name.ToString();
                        if (item.People.Surname != null)
                            dr["LastName"] = item.People.Surname.ToString();
                        if (item.People.Phone != null)
                            dr["Telephone"] = item.People.Phone.ToString();
                        if (item.People.Email != null)
                            dr["Email"] = item.People.Email.ToString();
                        if (item.People.Company != null)
                            dr["Office"] = item.People.Company.ShortName.ToString();
                        if (item.Company.ShortName != null)
                            dr["Plant"] = item.Company.ShortName.ToString();
                        if (item.IdSalesUser != null)
                            dr["IdSalesUser"] = item.IdSalesUser;
                        if (item.MaxDiscountAllowed != null)
                            dr["MaxDiscountPermitted"] = item.MaxDiscountAllowed;
                        //if (item.IdSalesQuotaCurrency != null)
                        //    dr["IdCurrency"] = item.IdSalesQuotaCurrency;

                        //[001]Added
                        //for (Int64 i = EndYear; i >= StartYear; i--)
                        //{
                        //    if(item.SalesUserQuotas != null && item.SalesUserQuotas.Any(suq=>suq.Year==i))
                        //    {
                        //        dr[i.ToString()] = item.SalesUserQuotas.Where(suq => suq.Year == i).FirstOrDefault().SalesQuotaAmountWithExchangeRate;
                        //    }


                        //}
                        for (Int64 i = EndYear; i >= StartYear; i--)
                        {
                            if (item.SalesUserQuotas != null && item.SalesUserQuotas.Any(suq => suq.Year == i))
                            {
                                SalesUserQuota salesUserQuota = item.SalesUserQuotas.Where(suq => suq.Year == i).FirstOrDefault();
                                if (salesUserQuota!=null)
                                {
                                    dr[i.ToString()] = salesUserQuota.SalesQuotaAmountWithExchangeRate;
                                    if(salesUserQuota.CurrencyConversionDate != null)
                                        dr["ExchangeRateDate-" + i.ToString()] = salesUserQuota.CurrencyConversionDate;
                                    if (salesUserQuota.CurrencyConversionRate != null)
                                        dr["ExchangeRate4D-" + i.ToString()] = salesUserQuota.CurrencyConversionRate.ToString("N4");
                                    if (salesUserQuota.SalesQuotaAmount != null)
                                        dr["Total-" + i.ToString() + "ConvertedAmount"] = salesUserQuota.SalesQuotaAmount.ToString("N2");
                                    if (salesUserQuota.SalesQuotaAmount != null)
                                        dr["Total-" + i.ToString() + "ConvertedAmountWithSymbol"] = GeosApplication.Instance.Currencies.FirstOrDefault(curr => curr.IdCurrency == salesUserQuota.IdSalesQuotaCurrency).Symbol
                                                                                                                         + " " +
                                                                                                                          salesUserQuota.SalesQuotaAmount.ToString("N2");

                                }
                            }
                        }
                        DtUsersListCopy.Rows.Add(dr);
                    }
                    DtUsersList = DtUsersListCopy;
                }

                SalesQuotaViewTitle = System.Windows.Application.Current.FindResource("UsersSalesQuota").ToString() + " (" + Convert.ToInt32(GeosApplication.Instance.CrmOfferYear) + ")"; ;

                GeosApplication.Instance.Logger.Log("Method FillUsers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsers() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

     
        
        private void AddUserViewWindowShow(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddUserViewWindowShow...", category: Category.Info, priority: Priority.Low);
                TableView tableView = (TableView)obj;
                DXSplashScreen.Show<SplashScreenView>();
                AddUserViewModel addUserViewModel = new AddUserViewModel();
                AddUserView addUserView = new AddUserView();
                //UserEditViewTitle = System.Windows.Application.Current.FindResource("AddUserViewHeader").ToString();
                EventHandler handle = delegate { addUserView.Close(); };
                addUserViewModel.RequestClose += handle;
                addUserView.DataContext = addUserViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                //IsUserViewColumnChooserVisible = false;
                var ownerInfo = (tableView as FrameworkElement);
                addUserView.Owner = Window.GetWindow(ownerInfo);
                addUserView.ShowDialogWindow();

                if (addUserViewModel.IsSaveUser)
                {
              
                    SalesUserQuota suq = new SalesUserQuota();
                    suq = addUserViewModel.SalesUserQuotas.FirstOrDefault(x => x.Year == Convert.ToInt32(GeosApplication.Instance.CrmOfferYear));
                    addUserViewModel.SaleUserData.SalesQuotaAmount = suq.SalesQuotaAmountWithExchangeRate;

                    ListUsers.Add(addUserViewModel.SaleUserData);
                 
                    // code for hide bind new user data to Gridview using datatable

                    DataRow dr = DtUsersListCopy.NewRow();
                    if (addUserViewModel.SaleUserData.People.UserGender != null)
                        dr["Gender"] = addUserViewModel.SaleUserData.People.UserGender.ToString();
                    if (addUserViewModel.SaleUserData.People.Name != null)
                        dr["FirstName"] = addUserViewModel.SaleUserData.People.Name.ToString();
                    if (addUserViewModel.SaleUserData.People.Surname != null)
                        dr["LastName"] = addUserViewModel.SaleUserData.People.Surname.ToString();
                    if (addUserViewModel.SaleUserData.People.Phone != null)
                        dr["Telephone"] = addUserViewModel.SaleUserData.People.Phone.ToString();
                    if (addUserViewModel.SaleUserData.People.Email != null)
                        dr["Email"] = addUserViewModel.SaleUserData.People.Email.ToString();
                    if (addUserViewModel.SaleUserData.People.Company != null)
                        dr["Office"] = addUserViewModel.SaleUserData.People.Company.ShortName.ToString();
                    if (addUserViewModel.SaleUserData.Company.ShortName != null)
                        dr["Plant"] = addUserViewModel.SaleUserData.Company.ShortName.ToString();
                    if (addUserViewModel.SaleUserData.IdSalesUser != null)
                        dr["IdSalesUser"] = addUserViewModel.SaleUserData.IdSalesUser;
                    if (addUserViewModel.SaleUserData.IdSalesQuotaCurrency != null)
                        dr["IdCurrency"] = addUserViewModel.SaleUserData.IdSalesQuotaCurrency;
                    if(addUserViewModel.SaleUserData.MaxDiscountAllowed!=null)
                        dr["MaxDiscountPermitted"] = addUserViewModel.SaleUserData.MaxDiscountAllowed;

                    if (addUserViewModel.SaleUserData.SalesUserQuotas != null)
                    {
                        if (addUserViewModel.SaleUserData.SalesUserQuotas.Count > 0)
                        {
                            foreach (SalesUserQuota itemSalesUserQuota in addUserViewModel.SaleUserData.SalesUserQuotas)
                            {
                                dr["IdCurrency"] = itemSalesUserQuota.IdSalesQuotaCurrency;
                                dr[itemSalesUserQuota.Year.ToString()] = itemSalesUserQuota.SalesQuotaAmountWithExchangeRate;
                                dr["ExchangeRateDate-" + itemSalesUserQuota.Year.ToString()] = itemSalesUserQuota.ExchangeRateDate;
                                //dataRow["ExchangeRate4D-" + item.Year.ToString()] = item.CurrencyConversionRate.ToString("N4");
                                dr["Total-" + itemSalesUserQuota.Year.ToString() + "ConvertedAmount"] = itemSalesUserQuota.SalesQuotaAmount.ToString("N2");
                                dr["Total-" + itemSalesUserQuota.Year.ToString() + "ConvertedAmountWithSymbol"] = GeosApplication.Instance.Currencies.FirstOrDefault(curr => curr.IdCurrency == itemSalesUserQuota.IdSalesQuotaCurrency).Symbol
                                                                                                                 + " " +
                                                                                                                  itemSalesUserQuota.SalesQuotaAmount.ToString("N2");
                            }
                        }
                    }
                    DtUsersList.Rows.Add(dr);
                    SelectedObject = DtUsersList.DefaultView[(DtUsersList.Rows.Count - 1)];

                }
                // code for hide column chooser if empty
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in gridControl.Columns)
                {
                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }
                if (visibleFalseCoulumn > 0)
                {
                    IsUserViewColumnChooserVisible = true;
                }
                else
                {
                    IsUserViewColumnChooserVisible = false;
                }
                ((GridControl)obj).Focus();
                // = true;
                GeosApplication.Instance.Logger.Log("Method AddUserViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddUserViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshUserDetails(object obj)
        {
            TableView detailView = (TableView)obj;
            GridControl gridControl = (detailView).Grid;

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
            MyFilterString = string.Empty;
            detailView.SearchString = null;
            // code for hide column chooser if empty

            int visibleFalseCoulumn = 0;
            foreach (GridColumn column in gridControl.Columns)
            {
                if (column.Visible == false)
                {
                    visibleFalseCoulumn++;
                }
            }
            if (visibleFalseCoulumn > 0)
            {
                IsUserViewColumnChooserVisible = true;
            }
            else
            {
                IsUserViewColumnChooserVisible = false;
            }
            StartYear = GeosApplication.Instance.SelectedyearStarDate.Year;
            EndYear = GeosApplication.Instance.SelectedyearEndDate.Year;
            AddColumnsToDataTable();
            FillUsers();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        /// <summary>
        /// Method for print User grid.
        /// </summary>
        /// <param name="obj"></param>
        public void PrintAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintAction ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ContactViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ContactViewCustomPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                GeosApplication.Instance.Logger.Log("Method PrintAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Export User grid.
        /// </summary>
        /// <param name="obj"></param>
        private void ExportUserViewGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportUserViewGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Users";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (bool)saveFile.ShowDialog();

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
                    TableView contactTableView = ((TableView)obj);
                    contactTableView.ShowTotalSummary = false;
                    contactTableView.ShowFixedTotalSummary = false;
                    contactTableView.ExportToXlsx(ResultFileName);
                    IsBusy = false;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    Process.Start(ResultFileName);
                    contactTableView.ShowTotalSummary = false;

                    contactTableView.ShowFixedTotalSummary = true;
                }

                GeosApplication.Instance.Logger.Log("Method ExportUserViewGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportUserViewGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for BestFit the grid and save and load Grid as per user.
        /// </summary>
        /// <param name="obj"></param>
        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsUserViewColumnChooserVisible = true;
                }
                else
                {
                    IsUserViewColumnChooserVisible = false;
                }

                //((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(AccountGridSettingFilePath);

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for open MailTo in Outlook for send Email. 
        /// </summary>
        /// <param name="obj"></param>
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();

                IsBusy = false;

                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {

        }

        #endregion


    }
}
