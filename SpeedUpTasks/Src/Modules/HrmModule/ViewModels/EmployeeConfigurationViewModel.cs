using DevExpress.Export.Xl;
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
using Microsoft.Win32;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    /// <summary>
    /// M040-10	Changes in company configurations-sdesai
    /// </summary>
    public class EmployeeConfigurationViewModel : INotifyPropertyChanged
    {
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public ICommands
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand HyperlinkForWebsite { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        #endregion

        #region Declaration
        private ObservableCollection<Company> companyList;
        private long selectedPeriod;
        private Company selectedCompany;
        private bool isBusy;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        #endregion

        #region Properties  
        public ObservableCollection<Company> CompanyList
        {
            get
            {
                return companyList;
            }

            set
            {
                companyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyList"));
            }
        }
        public long SelectedPeriod
        {
            get { return selectedPeriod; }
            set
            {
                selectedPeriod = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriod"));
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
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        public Company SelectedCompany
        {
            get
            {
                return selectedCompany;
            }

            set
            {
                selectedCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompany"));
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

        #region Constructor
        public EmployeeConfigurationViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor EmployeeConfigurationViewModel()...", category: Category.Info, priority: Priority.Low);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshCompanyConfiguration));
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                HyperlinkForWebsite = new DelegateCommand<object>(new Action<object>((obj) => { OpenWebsite(obj); }));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintCompaniesList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportCompaniesList));
                CommandGridDoubleClick = new DelegateCommand<object>(CommandGridDoubleClickAction);
                //if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                //{
                //    DXSplashScreen.Show(x =>
                //    {
                //        Window win = new Window()
                //        {
                //            ShowActivated = false,
                //            WindowStyle = WindowStyle.None,
                //            ResizeMode = ResizeMode.NoResize,
                //            AllowsTransparency = true,
                //            Background = new SolidColorBrush(Colors.Transparent),
                //            ShowInTaskbar = false,
                //            Topmost = true,
                //            SizeToContent = SizeToContent.WidthAndHeight,
                //            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //        };
                //        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                //        win.Topmost = false;
                //        return win;
                //    }, x =>
                //    {
                //        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                //    }, null, null);
                //}



                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor EmployeeConfigurationViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeConfigurationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                CompanyList = new ObservableCollection<Company>(WorkbenchStartUp.GetAllCompanyList());
                SelectedCompany = CompanyList[0];
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method added for refresh the company grid
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshCompanyConfiguration(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshCompanyConfiguration()...", category: Category.Info, priority: Priority.Low);

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

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                CompanyList = new ObservableCollection<Company>(WorkbenchStartUp.GetAllCompanyList());
                SelectedCompany = CompanyList[0];
                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshCompanyConfiguration()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshHolidaysList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshHolidaysList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshHolidaysList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for open Website
        /// </summary>
        /// <param name="obj"></param>
        public void OpenWebsite(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWebsite ...", category: Category.Info, priority: Priority.Low);
                string URLAddess = Convert.ToString(obj);
                string command = URLAddess;
                System.Diagnostics.Process.Start(URLAddess);

                GeosApplication.Instance.Logger.Log("Method OpenWebsite() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenWebsite() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for print Companies List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintCompaniesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintCompaniesList()...", category: Category.Info, priority: Priority.Low);

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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["CompaniesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["CompaniesReportPrintHeaderTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintCompaniesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintCompaniesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Export Companies List
        /// </summary>
        /// <param name="obj"></param>
        private void ExportCompaniesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportCompaniesList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Companies List";
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

                    ResultFileName = (saveFile.FileName);
                    TableView companiesTableView = ((TableView)obj);
                    companiesTableView.ShowTotalSummary = false;
                    companiesTableView.ShowFixedTotalSummary = false;
                    companiesTableView.ExportToXlsx(ResultFileName);
                    IsBusy = false;
                    companiesTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportCompaniesList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportCompaniesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        private void CommandGridDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandGridDoubleClickAction()...", category: Category.Info, priority: Priority.Low);

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

                TableView detailView = (TableView)obj;
                Company SelectedCompanyRow = (Company)detailView.DataControl.CurrentItem;
                int index = detailView.FocusedRowHandle;
                Company company = HrmService.GetCompanyDetailsByCompanyIdSelectedPeriod(SelectedCompanyRow.IdCompany, HrmCommon.Instance.SelectedPeriod);

                if (company != null)
                {
                    EditCompanyView editCompanyView = new EditCompanyView();
                    EditCompanyViewModel editCompanyViewModel = new EditCompanyViewModel();
                    EventHandler handle = delegate { editCompanyView.Close(); };
                    editCompanyViewModel.RequestClose += handle;
                    editCompanyView.DataContext = editCompanyViewModel;
                    editCompanyViewModel.Init(company);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    var ownerInfo = (detailView as FrameworkElement);
                    editCompanyView.Owner = Window.GetWindow(ownerInfo);
                    editCompanyView.ShowDialog();

                    if (editCompanyViewModel.IsSave)
                    {
                        company = CompanyList.Where(i => i.IdCompany == editCompanyViewModel.UpdateComapny.IdCompany).FirstOrDefault();
                        company.Alias = editCompanyViewModel.UpdateComapny.Alias;
                        company.Name = editCompanyViewModel.UpdateComapny.Name;
                        company.RegisteredName = editCompanyViewModel.UpdateComapny.RegisteredName;
                        company.Address = editCompanyViewModel.UpdateComapny.Address;
                        company.City = editCompanyViewModel.UpdateComapny.City;
                        company.Region = editCompanyViewModel.UpdateComapny.Region;
                        company.ZipCode = editCompanyViewModel.UpdateComapny.ZipCode;
                        company.IdCountry = editCompanyViewModel.UpdateComapny.IdCountry;
                        company.Country = editCompanyViewModel.UpdateComapny.Country;
                        company.Telephone = editCompanyViewModel.UpdateComapny.Telephone;
                        company.CIF = editCompanyViewModel.UpdateComapny.CIF;
                        company.Fax = editCompanyViewModel.UpdateComapny.Fax;
                        company.Email = editCompanyViewModel.UpdateComapny.Email;
                        company.Website = editCompanyViewModel.UpdateComapny.Website;
                        company.Latitude = editCompanyViewModel.UpdateComapny.Latitude;
                        company.Longitude = editCompanyViewModel.UpdateComapny.Longitude;
                        company.Coordinates = editCompanyViewModel.UpdateComapny.Coordinates;
                        //TableView detailView = (TableView)obj;
                        GridControl gridControl = (detailView).Grid;
                        gridControl.RefreshData();

                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method CommandGridDoubleClickAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandGridDoubleClickAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
