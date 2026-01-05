using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Hrm;
using System.ComponentModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.UI.Commands;
using Microsoft.Win32;
using DevExpress.Export.Xl;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using System.ServiceModel;
using DevExpress.Data.Filtering;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.UI.Helper;
using System.Text.RegularExpressions;
using NodaTime;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeRecruitmentViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region TaskLog
        // [nsatpute][28-04-2025][GEOS2-6502]
        #endregion

        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // IHrmService HrmService = new HrmServiceController("localhost:6699");
        private INavigationService Service { get { return GetService<INavigationService>(); } }

        #endregion // End Services

        #region Public ICommand
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand NavigateCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand AddNewJobOfferCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand SelectedYearChangedCommand { get; private set; }

        #endregion

        #region Declaration

        private bool isInIt = false;
        private bool isBusy;
        private ObservableCollection<Recruitment> recruitmentList;
        #endregion

        #region Properties


        public ObservableCollection<Recruitment> RecruitmentList
        {
            get { return recruitmentList; }
            set { recruitmentList = value; OnPropertyChanged(new PropertyChangedEventArgs(nameof(RecruitmentList))); }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsBusy))); }
        }
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
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

        #region IService
        //IServiceContainer serviceContainer = null;
        //protected IServiceContainer ServiceContainer
        //{
        //    get
        //    {
        //        if (serviceContainer == null)
        //            serviceContainer = new ServiceContainer(this);
        //        return serviceContainer;
        //    }
        //}     
        #endregion

        #region Constructor
		// [nsatpute][12-05-2025][GEOS2-5707]
        public EmployeeRecruitmentViewModel()
        {
            try
            {
                GeosApplication.Instance.IsButtonRowVisible = true;
                AddNewJobOfferCommand = new RelayCommand(new Action<object>(AddNewJobOfferCommandAction));
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SelectedYearChangedCommandAction);
                SelectedYearChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SelectedYearChangedCommandAction);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintRecruitmentListCommandAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportRecruitmentList));
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
                isInIt = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor EmployeeProfileViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeProfileViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
		// [nsatpute][12-05-2025][GEOS2-5707]
        private void AddNewJobOfferCommandAction(object obj)
        {
            AddEditJobOfferViewModel addEditJobOfferViewModel = new AddEditJobOfferViewModel();
            AddEditJobOfferView addEditJobOfferView =new AddEditJobOfferView();
            addEditJobOfferViewModel.IsNew = true;
            addEditJobOfferViewModel.WindowHeader = System.Windows.Application.Current.FindResource("Employeeprofiledetailview_Addjoboffer").ToString();
            addEditJobOfferView.DataContext = addEditJobOfferViewModel;
            EventHandler handle = delegate { addEditJobOfferView.Close(); };
            addEditJobOfferViewModel.RequestClose += handle;
            addEditJobOfferView.ShowDialog();
        }
		// [nsatpute][12-05-2025][GEOS2-5707]
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

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

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    FillRecruitments(plantOwnersIds, HrmCommon.Instance.SelectedPeriod);
                }
                else
                {
                    RecruitmentList = new ObservableCollection<Recruitment>();
                }


                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // [nsatpute][12-05-2025][GEOS2-5707]
        private void FillRecruitments(string idCompany, long selectedPeriod)
        {
            try
            {
                //HrmService = new HrmServiceController("localhost:6699");
                RecruitmentList = new ObservableCollection<Recruitment>(HrmService.GetEmployeeRecruitmentByIdSite(idCompany, selectedPeriod));
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRecruitments() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRecruitments() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillRecruitments()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		// [nsatpute][12-05-2025][GEOS2-5707]
        private void SelectedYearChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    FillRecruitments(plantOwnersIds, HrmCommon.Instance.SelectedPeriod);
                }
                else
                {
                    RecruitmentList = new ObservableCollection<Recruitment>();
                }


                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedYearChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
		// [nsatpute][12-05-2025][GEOS2-5707]
        private void PrintRecruitmentListCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintRecruitmentListCommandAction()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["RecruitmentReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["RecruitmentReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintRecruitmentListCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintRecruitmentListCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		// [nsatpute][12-05-2025][GEOS2-5707]
        private void ExportRecruitmentList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportRecruitmentList()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Recruitment List";
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
                    TableView recruitmentTableView = ((TableView)obj);
                    recruitmentTableView.ShowTotalSummary = false;
                    recruitmentTableView.ShowFixedTotalSummary = false;
                    recruitmentTableView.ExportToXlsx(ResultFileName, new DevExpress.XtraPrinting.XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG });

                    IsBusy = false;
                    recruitmentTableView.ShowTotalSummary = true;
                    recruitmentTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportRecruitmentList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportRecruitmentList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }

}
