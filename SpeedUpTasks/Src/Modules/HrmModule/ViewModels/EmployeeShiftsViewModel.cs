using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
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
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeShiftsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// [M049-07][20180810][Add option to add and edit shifts][adadibathina]
        /// [M051-10][Wrong weekdays order in shifts section][adadibathina]
        /// [M051-08][Year selection is not saved after change section][adadibathina]
        /// </summary>
        /// 

        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        // private long selectedPeriod;
        private ObservableCollection<CompanyShift> companyShiftsList;
        private CompanyShift selectedCompanyShift;
        private CompanyShift updateCompanyShift;
        private string idsSelectedplants;
        private bool isBusy;

        private string timeEditMask;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;




        #endregion

        #region Properties
        //public long SelectedPeriod
        //{
        //    get { return selectedPeriod; }
        //    set
        //    {
        //        selectedPeriod = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriod"));
        //    }
        // }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public CompanyShift SelectedCompanyShift
        {
            get
            {
                return selectedCompanyShift;
            }

            set
            {
                selectedCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyShift"));
            }
        }


        public CompanyShift UpdateCompanyShift
        {
            get
            {
                return updateCompanyShift;
            }

            set
            {
                updateCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateCompanyShift"));
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
        public ObservableCollection<CompanyShift> CompanyShiftsList
        {
            get
            {
                return companyShiftsList;
            }

            set
            {
                companyShiftsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShiftsList"));
            }
        }

        public string TimeEditMask
        {
            get
            {
                return timeEditMask;
            }

            set
            {
                timeEditMask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeEditMask"));
            }
        }

        // [M051-10]
        private int indexSun;

        public int IndexSun
        {
            get { return indexSun; }
            set { indexSun = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexSun")); }
        }

        private int indexMon;

        public int IndexMon
        {
            get { return indexMon; }
            set { indexMon = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexMon")); }
        }
        private int indexTue;

        public int IndexTue
        {
            get { return indexTue; }
            set { indexTue = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexTue")); }
        }
        private int indexWed;

        public int IndexWed
        {
            get { return indexWed; }
            set { indexWed = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexWed")); }
        }
        private int indexThu;

        public int IndexThu
        {
            get { return indexThu; }
            set { indexThu = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexThu")); }
        }
        private int indexFri;

        public int IndexFri
        {
            get { return indexFri; }
            set { indexFri = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexFri")); }
        }
        private int indexSat;

        public int IndexSat
        {
            get { return indexSat; }
            set { indexSat = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexSat")); }
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


        #endregion

        #region public Events
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Public ICommand
        public ICommand ButtonAddNewShiftCommand { get; set; }
        public ICommand CompanyEditValueChangedCommand { get; set; }
        public ICommand ButtonRefreshCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand EditShiftsDoubleClickCommand { get; set; }

        #endregion

        #region Ctor
        public EmployeeShiftsViewModel()
        {
            SetUserPermission();
            TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            ButtonAddNewShiftCommand = new RelayCommand(new Action<object>(AddNewShift));
            CompanyEditValueChangedCommand = new RelayCommand(new Action<object>(Refresh));
            ButtonRefreshCommand = new RelayCommand(new Action<object>(Refresh));
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportShiftsList));
            PrintButtonCommand = new RelayCommand(new Action<object>(PrintEmployeeShiftsList));
            EditShiftsDoubleClickCommand = new RelayCommand(new Action<object>(EditShift));
        }

        #endregion

        #region Methods
        /// <summary>
        /// Sprint 49-[M049-07][20180810][adadibathina]
        /// Method to initialize EmployeeShiftsViewModel
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.FillFinancialYear();
                //  SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;
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

                idsSelectedplants = string.Empty;

                List<object> Days = GeosApplication.Instance.GetWeekNames();

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().ForEach(x =>
                    {
                        idsSelectedplants += x.IdCompany + ",";
                    });

                    idsSelectedplants = idsSelectedplants.Remove(idsSelectedplants.Length - 1);
                    CompanyShiftsList = new ObservableCollection<CompanyShift>(HrmService.GetAllCompanyShiftsByIdCompany_V2035(idsSelectedplants));
                    if (CompanyShiftsList.Count > 0)
                        SelectedCompanyShift = CompanyShiftsList[0];
                }

                IndexSun = Days.FindIndex(x => x.ToString() == "Sunday") + 3;
                IndexMon = Days.FindIndex(x => x.ToString() == "Monday") + 3;
                IndexTue = Days.FindIndex(x => x.ToString() == "Tuesday") + 3;
                IndexWed = Days.FindIndex(x => x.ToString() == "Wednesday") + 3;
                IndexThu = Days.FindIndex(x => x.ToString() == "Thursday") + 3;
                IndexFri = Days.FindIndex(x => x.ToString() == "Friday") + 3;
                IndexSat = Days.FindIndex(x => x.ToString() == "Saturday") + 3;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }


            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeShiftsViewModel Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeShiftsViewModel Init() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeShiftsViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method to Add New Shift
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewShift(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("EmployeeShiftsViewModel Method AddNewShift()...", category: Category.Info, priority: Priority.Low);
                AddNewShiftView addNewShiftView = new AddNewShiftView();
                AddNewShiftViewModel addNewShiftViewModel = new AddNewShiftViewModel();
                EventHandler handle = delegate { addNewShiftView.Close(); };
                addNewShiftViewModel.RequestClose += handle;
                addNewShiftView.DataContext = addNewShiftViewModel;
                addNewShiftViewModel.Init(CompanyShiftsList);
                addNewShiftViewModel.IsNew = true;
                addNewShiftView.ShowDialog();
                if (addNewShiftViewModel.IsSave == true)
                {
                    CompanyShiftsList.Add(addNewShiftViewModel.NewShift);
                    SelectedCompanyShift = addNewShiftViewModel.NewShift;
                }
                GeosApplication.Instance.Logger.Log(" EmployeeShiftsViewModel Method AddNewShift()....executed successfully", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.Logger.Log("EmployeeShiftsViewModel Method AddNewShift()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeShiftsViewModel  Method AddNewShift()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Refresh Grid
        /// </summary>
        /// <param name="obj"></param>
        private void Refresh(object obj)
        {
            try
            {
                idsSelectedplants = string.Empty;
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                detailView.SearchString = null;
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    idsSelectedplants = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().Select(x => x.IdCompany));
                    //HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().ForEach(x =>
                    //{
                    //    idsSelectedplants += x.IdCompany + ",";
                    //});
                    //idsSelectedplants = idsSelectedplants.Remove(idsSelectedplants.Length - 1);
                    // CompanyShiftsList = new ObservableCollection<CompanyShift>(HrmService.GetAllCompanyShiftsByIdCompany_V2032(idsSelectedplants));
                    CompanyShiftsList = new ObservableCollection<CompanyShift>(HrmService.GetAllCompanyShiftsByIdCompany_V2035(idsSelectedplants));

                }
                else
                {
                    CompanyShiftsList = new ObservableCollection<CompanyShift>();
                }


            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeShiftsViewModel Refresh() Method  " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error  in EmployeeShiftsViewModel Refresh() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeShiftsViewModel  Refresh() Method...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Export Shifts  Details in Excel Sheet. 
        /// </summary>
        private void ExportShiftsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportEmployeeShiftsList()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Shifts";
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

                    ResultFileName = saveFile.FileName;
                    TableView ShiftsTableView = ((TableView)obj);
                    ShiftsTableView.PrintAutoWidth = false;
                    ShiftsTableView.ShowTotalSummary = false;
                    ShiftsTableView.ShowFixedTotalSummary = false;
                    ShiftsTableView.ExportToXlsx(ResultFileName, new DevExpress.XtraPrinting.XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG });
                    IsBusy = false;
                    ShiftsTableView.ShowFixedTotalSummary = true;

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("EmployeeShiftsViewModel in Method ExportShiftsList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error EmployeeShiftsViewModel in Method ExportShiftsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Print Shifts  Details  
        /// </summary>
        private void PrintEmployeeShiftsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("EmployeeShiftsViewModel Method PrintEmployeeShiftsList()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
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

                TableView ShiftsTableView = ((TableView)obj);
                ShiftsTableView.PrintAutoWidth = true;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ShiftsReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ShiftsReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("EmployeeShiftsViewModel Method PrintEmployeeShiftsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeShiftsViewModel Method PrintEmployeeShiftsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for EditShifts  
        /// </summary>
        public void EditShift(object obj)
        {
            try
            {
                //if (HrmCommon.Instance.UserPermission != PermissionManagement.SuperAdmin &&
                //    HrmCommon.Instance.UserPermission != PermissionManagement.Admin && HrmCommon.Instance.UserPermission != PermissionManagement.PlantViewer &&
                //    HrmCommon.Instance.UserPermission != PermissionManagement.GlobalViewer)
                //{
                //    return;
                //}

                GeosApplication.Instance.Logger.Log("EmployeeShiftsViewModel Method EditShift()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                CompanyShift companyShift = (CompanyShift)detailView.DataControl.CurrentItem;
                SelectedCompanyShift = companyShift;
                EditShiftView editShiftView = new EditShiftView();
                EditShiftViewModel editShiftViewModel = new EditShiftViewModel();
                EventHandler handle = delegate { editShiftView.Close(); };

                editShiftViewModel.RequestClose += handle;
                editShiftView.DataContext = editShiftViewModel;
                editShiftViewModel.Init(companyShift, CompanyShiftsList);
                var ownerInfo = (detailView as FrameworkElement);
                editShiftView.Owner = Window.GetWindow(ownerInfo);
                editShiftView.ShowDialog();
                if (editShiftViewModel.IsSave)
                {
                    companyShift.Name = editShiftViewModel.UpdateShift.Name;
                    companyShift.IdCompanyShift = editShiftViewModel.UpdateShift.IdCompanyShift;

                    companyShift.CompanySchedule = editShiftViewModel.UpdateShift.CompanySchedule;
                    companyShift.CompanyAnnualSchedule = editShiftViewModel.UpdateShift.CompanyAnnualSchedule;

                    companyShift.MonStartTime = editShiftViewModel.UpdateShift.MonStartTime;
                    companyShift.MonEndTime = editShiftViewModel.UpdateShift.MonEndTime;
                    companyShift.MonBreakTime = editShiftViewModel.UpdateShift.MonBreakTime;

                    companyShift.TueStartTime = editShiftViewModel.UpdateShift.TueStartTime;
                    companyShift.TueEndTime = editShiftViewModel.UpdateShift.TueEndTime;
                    companyShift.TueBreakTime = editShiftViewModel.UpdateShift.TueBreakTime;

                    companyShift.WedStartTime = editShiftViewModel.UpdateShift.WedStartTime;
                    companyShift.WedEndTime = editShiftViewModel.UpdateShift.WedEndTime;
                    companyShift.WedBreakTime = editShiftViewModel.UpdateShift.WedBreakTime;

                    companyShift.ThuStartTime = editShiftViewModel.UpdateShift.ThuStartTime;
                    companyShift.ThuEndTime = editShiftViewModel.UpdateShift.ThuEndTime;
                    companyShift.ThuBreakTime = editShiftViewModel.UpdateShift.ThuBreakTime;

                    companyShift.FriStartTime = editShiftViewModel.UpdateShift.FriStartTime;
                    companyShift.FriEndTime = editShiftViewModel.UpdateShift.FriEndTime;
                    companyShift.FriBreakTime = editShiftViewModel.UpdateShift.FriBreakTime;

                    companyShift.SatStartTime = editShiftViewModel.UpdateShift.SatStartTime;
                    companyShift.SatEndTime = editShiftViewModel.UpdateShift.SatEndTime;
                    companyShift.SatBreakTime = editShiftViewModel.UpdateShift.SatBreakTime;

                    companyShift.SunStartTime = editShiftViewModel.UpdateShift.SunStartTime;
                    companyShift.SunEndTime = editShiftViewModel.UpdateShift.SunEndTime;
                    companyShift.SunBreakTime = editShiftViewModel.UpdateShift.SunBreakTime;

                    companyShift.IsNightShift = editShiftViewModel.UpdateShift.IsNightShift;

                    SelectedCompanyShift = companyShift;
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeProfile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        #endregion
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
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
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
    }
}
