using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Modules.Hrm.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class LeavesViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        #endregion

        #region Declaration
        ObservableCollection<Column> leavesColumns;
        DataTable dttable;
        DataTable dttableCopy;
        private ObservableCollection<Summary> totalSummary;
        ObservableCollection<LookupValue> holidaysSettingList;
        private LookupValue selectedLeave;
        private ObservableCollection<LookupValue> leaveList;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private MaximizedElementPosition maximizedElementPosition;
        private ObservableCollection<CompanyHolidaySetting> companyHolidaySettingList;
        private ObservableCollection<CompanyServiceLength> serviceLengthList;
        private ObservableCollection<CompanyServiceLength> serviceLengthListClone;
        private DataTable localDataTable;
        private DataTable dataTable;
        private BandItem bandYearsOfService;
        private BandItem bandSitesHolidays;
        private ObservableCollection<BandItem> bands;
        private List<CompanyServiceLength> updatedServiceLengthList;
        ObservableCollection<PlantLeave> plantLeaveList;
        #endregion

        #region Properties

        public ObservableCollection<PlantLeave> PlantLeaveList
        {
            get { return plantLeaveList; }
            set
            {
                plantLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantLeaveList"));
            }
        }
        public DataTable DttableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DttableCopy"));
            }
        }
        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }
        public ObservableCollection<Column> LeavesColumns
        {
            get { return leavesColumns; }
            set
            {
                leavesColumns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeavesColumns"));
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
        public bool IsPlantChange { get; set; }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        ObservableCollection<CompanyHolidaySetting> compHolidaySettingList;
        public ObservableCollection<CompanyHolidaySetting> CompHolidaySettingList
        {
            get
            {
                return compHolidaySettingList;
            }

            set
            {
                compHolidaySettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompHolidaySettingList"));
            }
        }
        public ObservableCollection<CompanyHolidaySetting> CompanyHolidaySettingList
        {
            get
            {
                return companyHolidaySettingList;
            }

            set
            {
                companyHolidaySettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyHolidaySettingList"));
            }
        }
        CompanyHolidaySetting selectedCompanyHolidaySetting;
        public CompanyHolidaySetting SelectedCompanyHolidaySetting
        {
            get
            {
                return selectedCompanyHolidaySetting;
            }

            set
            {
                selectedCompanyHolidaySetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyHolidaySetting"));
            }
        }
        public LookupValue SelectedLeave
        {
            get
            {
                return selectedLeave;
            }

            set
            {
                selectedLeave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLeave"));
            }
        }
        public ObservableCollection<LookupValue> LeaveList
        {
            get
            {
                return leaveList;
            }

            set
            {
                leaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeaveList"));
            }
        }

        public ObservableCollection<LookupValue> HolidaysSettingList
        {
            get
            {
                return holidaysSettingList;
            }

            set
            {
                holidaysSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HolidaysSettingList"));
            }
        }
        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }

        public bool IsAcceptEnabled
        {
            get { return isAcceptEnabled; }
            set
            {
                isAcceptEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnabled"));
            }
        }

        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }
        //[nsatpute][04-07-2024][GEOS2-5681]
        public ObservableCollection<CompanyServiceLength> ServiceLengthList
        {
            get { return serviceLengthList; }
            set
            {
                serviceLengthList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ServiceLengthList"));
            }
        }
        //[nsatpute][04-07-2024][GEOS2-5681]
        public ObservableCollection<CompanyServiceLength> ServiceLengthListClone
        {
            get { return serviceLengthListClone; }
            set
            {
                serviceLengthListClone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ServiceLengthListClone"));
            }
        }
        //[nsatpute][04-07-2024][GEOS2-5681]
        public DataTable DataTable
        {
            get { return dataTable; }
            set
            {
                dataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTable"));
            }
        }
        //[nsatpute][04-07-2024][GEOS2-5681]
        public BandItem BandYearsOfService
        {
            get { return bandYearsOfService; }
            set
            {
                bandYearsOfService = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandYearsOfService"));
            }
        }
        public BandItem BandSitesHolidays
        {
            get { return bandSitesHolidays; }
            set
            {
                bandSitesHolidays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandSitesHolidays"));
            }
        }
        //[nsatpute][04-07-2024][GEOS2-5681]
        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }

        ObservableCollection<LogEntriesByCompanyLeaves> changeLogList;
        public ObservableCollection<LogEntriesByCompanyLeaves> ChangeLogList
        {
            get { return changeLogList; }
            set
            {
                changeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChangeLogList"));
            }
        }
        ObservableCollection<LookupValue> holidaysSettingListClone;
        public ObservableCollection<LookupValue> HolidaysSettingListClone
        {
            get
            {
                return holidaysSettingListClone;
            }

            set
            {
                holidaysSettingListClone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HolidaysSettingListClone"));
            }
        }

        ObservableCollection<CompanyHolidaySetting> companyHolidaySettingListClone;
        public ObservableCollection<CompanyHolidaySetting> CompanyHolidaySettingListClone
        {
            get
            {
                return companyHolidaySettingListClone;
            }

            set
            {
                companyHolidaySettingListClone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyHolidaySettingListClone"));
            }
        }

        LogEntriesByCompanyLeaves selectedChangeLog;
        public LogEntriesByCompanyLeaves SelectedChangeLog
        {
            get
            {
                return selectedChangeLog;
            }

            set
            {
                selectedChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedChangeLog"));
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

        #region Public ICommand
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand SiteValueChangedCommand { get; set; }
        public ICommand PlantPrintButtonCommand { get; set; }
        public ICommand PlantExportButtonCommand { get; set; }
        public ICommand PlantRefreshButtonCommand { get; set; }

        public ICommand LengthOfServiocePrintButtonCommand { get; set; }
        public ICommand LengthOfServioceExportButtonCommand { get; set; }
        public ICommand LengthOfServioceRefreshButtonCommand { get; set; }

        public ICommand HolidaySettingChangeCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }

        public ICommand ChangeLogPrintButtonCommand { get; set; }
        public ICommand ChangeLogExportButtonCommand { get; set; }
        public ICommand ChangeLogRefreshButtonCommand { get; set; }
        #endregion // Public Commands

        #region Constructor
        public LeavesViewModel()
        {
            try
            {
                IsPlantChange = false;
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor LeavesViewModel()...", category: Category.Info, priority: Priority.Low);
                BandYearsOfService = new BandItem() { BandName = "Years of service", BandHeader = "Years of service" };
                FillCompanyServiceList();
                AddColumnsAndDataToDataTable();
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintLeavesList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportLeavesList));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshLeavesList));

                PlantPrintButtonCommand = new RelayCommand(new Action<object>(PlantPrintLeavesList));
                PlantExportButtonCommand = new RelayCommand(new Action<object>(PlantExportLeavesList));
                PlantRefreshButtonCommand = new RelayCommand(new Action<object>(PlantRefreshLeavesList));

                LengthOfServiocePrintButtonCommand = new RelayCommand(new Action<object>(LengthOfServiocePrintButtonCommandAction));
                LengthOfServioceExportButtonCommand = new RelayCommand(new Action<object>(LengthOfServioceExportButtonCommandAction));
                LengthOfServioceRefreshButtonCommand = new RelayCommand(new Action<object>(LengthOfServioceRefreshButtonCommandAction));

                HolidaySettingChangeCommand = new DelegateCommand<object>(HolidaySettingChangedAction);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonAction);
                SelectedItemChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SelectedItemChangedCommandAction);
                SiteValueChangedCommand = new DelegateCommand<CellValueChangedEventArgs>(SiteValueChangedCommandAction);

                ChangeLogPrintButtonCommand = new RelayCommand(new Action<object>(ChangeLogPrintButtonCommandAction));
                ChangeLogExportButtonCommand = new RelayCommand(new Action<object>(ChangeLogExportButtonCommandAction));
                ChangeLogRefreshButtonCommand = new RelayCommand(new Action<object>(ChangeLogRefreshButtonCommandAction));
                FillChangeLogList();
                GeosApplication.Instance.Logger.Log("Constructor LeavesViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor LeavesViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion //Constructor

        #region Methods
        public void SiteValueChangedCommandAction(CellValueChangedEventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SiteValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                IsPlantChange = true;
                #region Region
                //CellValue cell = args.Cell;
                //int rowIndex = args.RowHandle;
                //var row1 = DataTable.AsEnumerable().ElementAt(rowIndex);
                //var row2 = localDataTable.AsEnumerable().ElementAt(rowIndex);
                //var eaesValue1 = row1[cell.Property];
                //var eaesValue2 = row2[cell.Property];
                //IsSiteValueChange = (!eaesValue1.Equals(eaesValue2));

                //for (int i = 0; i < localDataTable?.Rows?.Count; i++)
                //{
                //    for (int j = 0; j < localDataTable?.Columns?.Count; j++)
                //    {
                //        var value1 = localDataTable?.Rows[i][j];
                //        var value2 = dataTable?.Rows[i][j];
                //        if (value2 != null && value1 != null)
                //        {
                //            if (value1 == DBNull.Value && value2 == DBNull.Value)
                //            {
                //                continue;
                //            }
                //            if (!value1.Equals(value2))
                //            {
                //                IsSiteValueChange = true;
                //            }
                //        }
                //    }
                //}
                #endregion
                GeosApplication.Instance.Logger.Log("Method SiteValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SiteValueChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-5680][rdixit][22.07.2024]
        public void SelectedItemChangedCommandAction(EditValueChangedEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (CompanyHolidaySettingList != null && CompHolidaySettingList != null && SelectedCompanyHolidaySetting != null)
                {
                    var original = CompHolidaySettingList.FirstOrDefault(i => i.IdHolidaySetting == SelectedCompanyHolidaySetting.IdHolidaySetting);
                    var current = CompanyHolidaySettingList.FirstOrDefault(i => i.IdHolidaySetting == SelectedCompanyHolidaySetting.IdHolidaySetting);
                    if (original != null && current !=null)
                    {
                        if (original.HolidaySetting.IdLookupValue != current.HolidaySetting.IdLookupValue)
                            current.TransactionOperation = Data.Common.ModelBase.TransactionOperations.Update;
                        else
                            current.TransactionOperation = Data.Common.ModelBase.TransactionOperations.Add;
                    }
                    if (CompanyHolidaySettingList.Any(i => i.TransactionOperation == Data.Common.ModelBase.TransactionOperations.Update))
                        IsPlantChange = true;
                    else
                        IsPlantChange = false;
                }
                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedItemChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[HRM][GEOS2-6571][14.12.2024]
        public void GetLeavesByPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetLeavesByPlants()...", category: Category.Info, priority: Priority.Low);
                PlantLeaveList = new ObservableCollection<PlantLeave>(HrmService.GetLeaveTypesWithLocations_V2590());
                GeosApplication.Instance.Logger.Log("Method GetLeavesByPlants()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetLeavesByPlants()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
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

                MaximizedElementPosition = MaximizedElementPosition.Right;
                HoliDaySettingLookupValuesList();
                FillHoliDaySettingList();
                FillLeaveList();
                FillChangeLogList();
                GetLeavesByPlants();
                FillCompanyLocationcolumns();
                FillDataTable();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-6571][14.12.2024]
        void FillCompanyLocationcolumns()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyLocationcolumns ...", category: Category.Info, priority: Priority.Low);

                LeavesColumns = new ObservableCollection<Column>()
                {
                new Column() { FieldName="Name",HeaderText="Name", Settings = SettingsType.Name, AllowCellMerge=false, Width=130,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true  },
                new Column() { FieldName="Abbrevation",HeaderText="Abbrevation", Settings = SettingsType.Name, AllowCellMerge=false,Width=120,AllowEditing=true,Visible=true,IsVertical=false,FixedWidth=true },
                new Column() { FieldName="Position",HeaderText="Position", Settings = SettingsType.Name, AllowCellMerge=false, Width=80,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true  },
                new Column() { FieldName="HtmlColor",HeaderText="HtmlColor", Settings = SettingsType.HexColor, AllowCellMerge=false, Width=110,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true  },
                new Column() { FieldName="InUse",HeaderText="InUse", Settings = SettingsType.Name, AllowCellMerge=false, Width=65,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true  },
                };
                Dttable = new DataTable();

                Dttable.Columns.Add("Name", typeof(string));
                Dttable.Columns.Add("Abbrevation", typeof(string));
                Dttable.Columns.Add("Position", typeof(string));
                Dttable.Columns.Add("HtmlColor", typeof(string));
                Dttable.Columns.Add("InUse", typeof(string));

                if (PlantLeaveList?.Count > 0)
                {
                    foreach (var item in PlantLeaveList.GroupBy(i => i.Company?.Alias))
                    {
                        try
                        {
                            LeavesColumns.Add(new Column() { FieldName = item.Key, HeaderText = item.Key, Settings = SettingsType.Name, AllowCellMerge = false, Width = 60, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true });
                            Dttable.Columns.Add(item.Key, typeof(string));
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanyLocationcolumns()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanyLocationcolumns()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-6571][14.12.2024]
        void FillDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);

                Dttable.Rows.Clear();
                DttableCopy = Dttable.Copy();

                for (int i = 0; i < LeaveList.Count; i++)
                {
                    try
                    {
                        DataRow dr = DttableCopy.NewRow();
                        dr["Name"] = LeaveList[i].Value;
                        dr["Abbrevation"] = LeaveList[i].Abbreviation;
                        dr["Position"] = LeaveList[i].Position;
                        dr["HtmlColor"] = LeaveList[i].HtmlColor;
                        dr["InUse"] = LeaveList[i].InUse ? "Yes" : "No";
                        foreach (var item in PlantLeaveList.GroupBy(j => j.Company?.Alias))
                        {
                            foreach (var item1 in item.ToList())
                            {
                                if (item1.LeaveType.Value == LeaveList[i].Value)
                                    dr[item.Key] = "X";
                            }
                        }
                        DttableCopy.Rows.Add(dr);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method FillDataTable()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }

                Dttable = DttableCopy;

                GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillDataTable()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void AcceptButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonAction()...", category: Category.Info, priority: Priority.Low);
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
                    List<CompanyHolidaySetting> updatedList = new List<CompanyHolidaySetting>();
                    bool serviceLengthSave = false; bool holidaySettingSave = false;
                    #region Plant [GEOS2-5680][rdixit][22.07.2024]
                    if (CompHolidaySettingList != null && CompanyHolidaySettingList != null)
                    {
                        CompanyHolidaySettingList.ToList().ForEach(x => {
                            if (!CompHolidaySettingList.Any(y => y.Company.IdCompany == x.Company.IdCompany && y.HolidaySetting?.IdLookupValue == x.HolidaySetting?.IdLookupValue))
                            { x.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;updatedList.Add(x); };});
                    }
                    #endregion
                    #region Company service list
                    List<CompanyServiceLength> updatedServiceLength = GetUpdatedCompanyServiceLengthList();
                    if (updatedServiceLength.Count > 0)
                    {
                        try
                        {
                            foreach (var item in updatedServiceLength)
                            {
                                if (item.ChangeLog==null)
                                {
                                    item.ChangeLog = new List<LogEntriesByCompanyLeaves>();
                                }
                                LogEntriesByCompanyLeaves LogEntriesByCompanyLeaves = new LogEntriesByCompanyLeaves();
                                CompanyServiceLength companyServiceLength= ServiceLengthListClone.Where(w=>w.IdAnnualHoliday== item.IdAnnualHoliday).FirstOrDefault();
                                LogEntriesByCompanyLeaves = new LogEntriesByCompanyLeaves()
                                {
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    IdCompany= item.IdCompany,
                                    ChangeLogDatetime = DateTime.Now,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("LengthOfServiceChangeLog").ToString(),
                                    companyServiceLength.Holidays, item.Holidays, item.YearOfService, item.SiteShortName)
                                };
                                item.ChangeLog.Add(LogEntriesByCompanyLeaves);
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        //Shubham[skadam] GEOS2-5682 HRM - Holidays (3 of 4) 12 09 2024
                        //serviceLengthSave = HrmService.UpdateCompanyWiseLengthOfService(GeosApplication.Instance.ActiveUser.Login, updatedServiceLength);
                        //HrmService = new HrmServiceController("localhost:6699");
                        serviceLengthSave = HrmService.UpdateCompanyWiseLengthOfService_V2560(GeosApplication.Instance.ActiveUser.Login, updatedServiceLength);
                        if (serviceLengthSave)
                            MergeUpdatedServiceLengthRecods();
                    }
                    #endregion

                    if (updatedList?.Count > 0)
                    {
                        try
                        {
                            foreach (var item in updatedList)
                            {
                                if (item.ChangeLog == null)
                                {
                                    item.ChangeLog = new List<LogEntriesByCompanyLeaves>();
                                }
                                LogEntriesByCompanyLeaves LogEntriesByCompanyLeaves = new LogEntriesByCompanyLeaves();
                                CompanyHolidaySetting colidaysSettingListClone = CompanyHolidaySettingListClone.Where(w => w.Company.IdCompany == item.Company.IdCompany).FirstOrDefault();
                                LogEntriesByCompanyLeaves = new LogEntriesByCompanyLeaves()
                                {
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    IdCompany = item.Company.IdCompany,
                                    ChangeLogDatetime = DateTime.Now,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyHolidayChangeLog").ToString(),
                                    colidaysSettingListClone.HolidaySetting?.ToString() ?? "None" , item.HolidaySetting?.ToString() ?? "None" , item.Company.Alias
                                    )
                                };
                                item.ChangeLog.Add(LogEntriesByCompanyLeaves);
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        //Shubham[skadam] GEOS2-5682 HRM - Holidays (3 of 4) 12 09 2024
                        //holidaySettingSave = HrmService.UpdateCompanyHolidaySetting(updatedList);
                        holidaySettingSave = HrmService.UpdateCompanyHolidaySetting_V2560(updatedList);
                    }
                    if (serviceLengthSave || holidaySettingSave)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CompHolidaySettingList = new ObservableCollection<CompanyHolidaySetting>(CompanyHolidaySettingList?.Select(i => (CompanyHolidaySetting)i.Clone()));
                        CompanyHolidaySettingListClone = new ObservableCollection<CompanyHolidaySetting>(CompanyHolidaySettingList?.Select(i => (CompanyHolidaySetting)i.Clone()));
                        IsPlantChange = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("HRMLeaveDetailsSavedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }
                //FillChangeLogList();
                Refresh();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AcceptButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		//[GEOS2-5681][25-07-2024][nsatpute]
        private void MergeUpdatedServiceLengthRecods()
        {
            foreach(CompanyServiceLength serviceLength in updatedServiceLengthList)
            {
                if (ServiceLengthList.Any(x => x.IdCompany == serviceLength.IdCompany && x.YearOfService == serviceLength.YearOfService))
                    ServiceLengthList.FirstOrDefault(x => x.IdCompany == serviceLength.IdCompany && x.YearOfService == serviceLength.YearOfService).Holidays = serviceLength.Holidays;
                else
                    ServiceLengthList.Add(serviceLength);
            }
        }
        #region Leave Refresh,Export and Print
        private void RefreshLeavesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshLeavesList()...", category: Category.Info, priority: Priority.Low);
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

                FillLeaveList();
                GetLeavesByPlants();
                FillCompanyLocationcolumns();
                FillDataTable();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshLeavesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshLeavesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportLeavesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLeavesList()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Leaves";
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
                    ResultFileName = (saveFile.FileName);
                    TableView LeavesTableView = ((TableView)obj);
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = false;
                    LeavesTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportLeavesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportLeavesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void PrintLeavesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintLeavesList()...", category: Category.Info, priority: Priority.Low);
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintLeavesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintLeavesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Plants Refresh,Export and Print [GEOS2-5680][rdixit][22.07.2024]
        private void PlantRefreshLeavesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshLeavesList()...", category: Category.Info, priority: Priority.Low);
                if (IsPlantChange)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeaveGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        AcceptButtonAction(null);
                    }
                    else
                    {
                        IsPlantChange = false;
                    }
                }

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

                FillHoliDaySettingList();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshLeavesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshLeavesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-5681][25-07-2024][nsatpute]
        private void LengthOfServioceRefreshButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LengthOfServioceRefreshButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
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

                FillCompanyServiceList();
                AddColumnsAndDataToDataTable();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method LengthOfServioceRefreshButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LengthOfServioceRefreshButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PlantExportLeavesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLeavesList()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Plants";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Plant Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {

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
                    ResultFileName = (saveFile.FileName);
                    TableView LeavesTableView = ((TableView)obj);
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = false;
                    LeavesTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportLeavesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportLeavesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
		//[GEOS2-5681][25-07-2024][nsatpute]
        private void LengthOfServioceExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LengthOfServioceExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Company Length Service Report";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Company Length Service Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {

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
                    ResultFileName = (saveFile.FileName);
                    TableView LeavesTableView = ((TableView)obj);
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = false;
                    LeavesTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method LengthOfServioceExportButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method LengthOfServioceExportButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void PlantPrintLeavesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintLeavesList()...", category: Category.Info, priority: Priority.Low);
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PlantReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PlantReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintLeavesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintLeavesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
		//[GEOS2-5681][25-07-2024][nsatpute]
        private void LengthOfServiocePrintButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LengthOfServiocePrintButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
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
       
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ServiceOfLengthPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ServiceOfLengthPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method LengthOfServiocePrintButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LengthOfServiocePrintButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public void FillLeaveList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeaveList()...", category: Category.Info, priority: Priority.Low);
                LeaveList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(32).OrderByDescending(x => x.Position != null).ToList());
                if (LeaveList.Count > 0)
                    SelectedLeave = LeaveList[0];
                GeosApplication.Instance.Logger.Log("Method FillLeaveList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeaveList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeaveList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLeaveList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region [GEOS2-5680][rdixit][22.07.2024]
        public void HoliDaySettingLookupValuesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeaveList()...", category: Category.Info, priority: Priority.Low);
                HolidaysSettingList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(151));
                GeosApplication.Instance.Logger.Log("Method FillLeaveList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeaveList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeaveList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLeaveList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][04-07-2024][GEOS2-5681]
        public void FillCompanyServiceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyServiceList()...", category: Category.Info, priority: Priority.Low);

                ServiceLengthList = new ObservableCollection<CompanyServiceLength>(HrmService.GetCompanyWiseLengthOfService());
                ServiceLengthListClone = new ObservableCollection<CompanyServiceLength>(ServiceLengthList.Select(i => (CompanyServiceLength)i.Clone()));
                GeosApplication.Instance.Logger.Log("Method FillCompanyServiceList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyServiceList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyServiceList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanyServiceList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][04-07-2024][GEOS2-5681]
        private void AddColumnsAndDataToDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);
                Bands = new ObservableCollection<BandItem>(); Bands.Clear();
                BandYearsOfService = new BandItem() { BandName = "YearsOfServices", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                BandSitesHolidays = new BandItem() { BandName = "Sites",  AllowBandMove = true, FixedStyle = FixedStyle.None, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                BandYearsOfService.Columns = new ObservableCollection<ColumnItem>();
                BandSitesHolidays.Columns = new ObservableCollection<ColumnItem>();
                BandYearsOfService.Columns.Add(new ColumnItem() { ColumnFieldName = "YearsOfService", HeaderText = "Years of service", Width = 100, IsVertical = false, SiteTypeSettings = SiteTypeSettingsType.YearsOfService, Visible = true });
                Bands.Add(BandYearsOfService);
                localDataTable = new DataTable();
                localDataTable.Columns.Add("YearsOfService", typeof(int));
                BandYearsOfService.Width = 100;
                var siteNames = ServiceLengthList.Select(d => d.SiteShortName).OrderBy(y => y).Distinct().ToList();
                foreach (var siteName in siteNames)
                {
                    localDataTable.Columns.Add(siteName, typeof(float));
                    BandSitesHolidays.Columns.Add(new ColumnItem() { ColumnFieldName = siteName, Readonly = IsReadOnlyField, HeaderText = siteName, IsVertical = false, SiteTypeSettings = SiteTypeSettingsType.Sites, Visible = true });
                }
                BandSitesHolidays.Columns.Add(new ColumnItem() { ColumnFieldName = "IsReadOnly", HeaderText = "IsReadOnly", Width = 0, IsVertical = false, SiteTypeSettings = SiteTypeSettingsType.Hidden, Visible = false });
                localDataTable.Columns.Add("IsReadOnly", typeof(bool));
                Bands.Add(BandSitesHolidays);
                var groupedData = ServiceLengthList.GroupBy(d => d.YearOfService);
                foreach (var group in groupedData)
                {
                    var row = localDataTable.NewRow();
                    row["YearsOfService"] = group.Key;
                    row["IsReadOnly"] = IsAcceptEnabled;
                    foreach (var siteName in siteNames)
                    {
                        row[siteName] = group.FirstOrDefault(d => d.SiteShortName == siteName)?.Holidays ?? 0;
                    }
                    localDataTable.Rows.Add(row);
                }
                DataTable = localDataTable;
                //CloneTable
                //DataTable = localDataTable.Clone();
                //if (localDataTable != null)
                //{
                //    foreach (DataRow row in localDataTable.Rows)
                //    {
                //        DataTable.ImportRow(row);
                //    }
                //}
                Bands = new ObservableCollection<BandItem>(Bands);
                TotalSummary = new ObservableCollection<Summary>() { new Summary() { Type = SummaryItemType.Count, FieldName = "YearsOfService", DisplayFormat = "Total : {0}" } };
                GeosApplication.Instance.Logger.Log("Method AddColumnsAndDataToDataTable executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsAndDataToDataTable() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsAndDataToDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsAndDataToDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		//[GEOS2-5681][25-07-2024][nsatpute]
        private void RefreshDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDataTable ...", category: Category.Info, priority: Priority.Low);
               
                localDataTable = new DataTable();
                localDataTable.Columns.Add("YearsOfService", typeof(int));
                BandYearsOfService.Width = 100;
                var siteNames = ServiceLengthList.Select(d => d.SiteShortName).OrderBy(y => y).Distinct().ToList();
                foreach (var siteName in siteNames)
                {
                    localDataTable.Columns.Add(siteName, typeof(float));
                }
                var groupedData = ServiceLengthList.GroupBy(d => d.YearOfService);
                foreach (var group in groupedData)
                {
                    var row = localDataTable.NewRow();
                    row["YearsOfService"] = group.Key;
                    row["IsReadOnly"] = IsAcceptEnabled;
                    foreach (var siteName in siteNames)
                    {
                        row[siteName] = group.FirstOrDefault(d => d.SiteShortName == siteName)?.Holidays ?? 0;
                    }
                    localDataTable.Rows.Add(row);
                }
                DataTable = localDataTable;
                Bands = new ObservableCollection<BandItem>(Bands);
                TotalSummary = new ObservableCollection<Summary>() { new Summary() { Type = SummaryItemType.Count, FieldName = "YearsOfService", DisplayFormat = "Total : {0}" } };
                GeosApplication.Instance.Logger.Log("Method RefreshDataTable executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in RefreshDataTable() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in RefreshDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsAndDataToDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][04-07-2024][GEOS2-5681]
        private List<CompanyServiceLength> GetUpdatedCompanyServiceLengthList()
        {

            updatedServiceLengthList = new List<CompanyServiceLength>();
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);

                var siteNames = ServiceLengthList.Select(d => d.SiteShortName).Distinct().ToList();
                foreach (DataRow row in DataTable.Rows)
                {
                    int yearOfService = (int)row["YearsOfService"];

                    foreach (var siteName in siteNames)
                    {
                        float holidays = Convert.ToSingle(row[siteName]);
                        var itemToUpdate = ServiceLengthList.FirstOrDefault(x => x.YearOfService == yearOfService && x.SiteShortName == siteName && x.Holidays != holidays);
                        if (itemToUpdate != null)
                        {
                            itemToUpdate.Holidays = (float)row[siteName];
                            updatedServiceLengthList.Add(itemToUpdate);
                        }
                        else if (ServiceLengthList.FirstOrDefault(x => x.YearOfService == yearOfService && x.SiteShortName == siteName) == null)
                        {
                            updatedServiceLengthList.Add(new CompanyServiceLength() { IdAnnualHoliday = 0, IdCompany = ServiceLengthList.FirstOrDefault(x => x.SiteShortName == siteName).IdCompany, YearOfService = yearOfService, Holidays = holidays });
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in GetUpdatedHolidayList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in GetUpdatedHolidayList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in GetUpdatedHolidayList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return updatedServiceLengthList;
        }
        public void FillHoliDaySettingList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeaveList()...", category: Category.Info, priority: Priority.Low);
                CompanyHolidaySettingList = new ObservableCollection<CompanyHolidaySetting>(HrmService.GetAllCompanyHolidaySetting());
                if (CompanyHolidaySettingList?.Count > 0 && HolidaysSettingList?.Count > 0)
                {
                    CompanyHolidaySettingList.ToList().ForEach(i =>
                    {
                        i.HolidaySetting = HolidaysSettingList.FirstOrDefault(j => j.IdLookupValue == i.IdValue);
                        i.HolidaySettingList = HolidaysSettingList.ToList();
                    });
                    CompanyHolidaySettingList = new ObservableCollection<CompanyHolidaySetting>(CompanyHolidaySettingList);
                    CompHolidaySettingList = new ObservableCollection<CompanyHolidaySetting>(CompanyHolidaySettingList.Select(i => (CompanyHolidaySetting)i.Clone()));
                    SelectedCompanyHolidaySetting = CompanyHolidaySettingList.FirstOrDefault();
                }
                CompanyHolidaySettingListClone = new ObservableCollection<CompanyHolidaySetting>(CompanyHolidaySettingList.Select(i => (CompanyHolidaySetting)i.Clone()));
                GeosApplication.Instance.Logger.Log("Method FillLeaveList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeaveList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeaveList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLeaveList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void HolidaySettingChangedAction(object obj)
        {
            //RoutedEventArgs eventarg = (RoutedEventArgs)obj;
            //DevExpress.Xpf.Editors.ComboBoxEdit combo = (DevExpress.Xpf.Editors.ComboBoxEdit)eventarg.OriginalSource;
            //CompanyHolidaySetting newVal = CompanyHolidaySettingList.FirstOrDefault(i => i.IdValue == ((LookupValue)combo.SelectedItem)?.IdLookupValue);
            SelectedCompanyHolidaySetting.TransactionOperation = Data.Common.ModelBase.TransactionOperations.Update;
        }
        #endregion

        #endregion //Methods

        private void SetUserPermission()
        {
            //HrmCommon.Instance.UserPermission = PermissionManagement.PlantViewer;

            switch (HrmCommon.Instance.UserPermission)
            {
                case PermissionManagement.SuperAdmin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.Admin:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.PlantViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.GlobalViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                default:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void FillChangeLogList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyServiceList()...", category: Category.Info, priority: Priority.Low);
                //HrmService = new HrmServiceController("localhost:6699");
                ChangeLogList = new ObservableCollection<LogEntriesByCompanyLeaves>();
                ChangeLogList = new ObservableCollection<LogEntriesByCompanyLeaves>(HrmService.GetCompanyLeavesChangeLog_V2560());
                if (ChangeLogList.Count > 0)
                    SelectedChangeLog = ChangeLogList[0];
                GeosApplication.Instance.Logger.Log("Method FillCompanyServiceList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyServiceList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyServiceList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanyServiceList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeLogRefreshButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeLogRefreshButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
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

                FillChangeLogList();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeLogRefreshButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ChangeLogRefreshButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeLogExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeLogExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Change Log Report";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Change Log Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {

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
                    ResultFileName = (saveFile.FileName);
                    TableView LeavesTableView = ((TableView)obj);
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = false;
                    LeavesTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ChangeLogExportButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ChangeLogExportButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void ChangeLogPrintButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeLogPrintButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ChangeLogPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ChangeLogPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeLogPrintButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ChangeLogPrintButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void Refresh()
        {
            try
            {
                FillHoliDaySettingList();
                FillCompanyServiceList();
                FillChangeLogList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Refresh()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }

        }
        #endregion
    }
}
