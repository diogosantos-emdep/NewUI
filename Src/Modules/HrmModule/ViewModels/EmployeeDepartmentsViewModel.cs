using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeDepartmentsViewModel : INotifyPropertyChanged
    {

        // This View Model Created By Amit
        //[HRM-M040-05] New configuration section Departments
        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public ICommand
        public ICommand AddNewDepartmentCommand { get; set; }
        public ICommand EditDepartmentDoubleClickCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }

        #endregion

        #region Declaration
        private ObservableCollection<Department> departmentList;
        private Department selectedDepartment;
        private bool isBusy;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        #endregion

        #region Public Properties
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public ObservableCollection<Department> DepartmentList
        {
            get
            {
                return departmentList;
            }

            set
            {
                departmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentList"));
            }
        }

        public Department SelectedDepartment
        {
            get
            {
                return selectedDepartment;
            }

            set
            {
                selectedDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDepartment"));
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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
        public EmployeeDepartmentsViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor EmployeeDepartmentsViewModel()...", category: Category.Info, priority: Priority.Low);

                RefreshButtonCommand = new DelegateCommand(RefreshDepartmentList);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintDepartmentList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportDepartmentList));
                AddNewDepartmentCommand = new DelegateCommand<object>(AddNewDepartmentInformation);
                EditDepartmentDoubleClickCommand = new DelegateCommand<object>(EditDepartmentInformation);

                GeosApplication.Instance.Logger.Log("Constructor EmployeeDepartmentsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeDepartmentsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Method for Initialization . 
        /// </summary>
        /// 
        public void Init()
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

                DepartmentList = new ObservableCollection<Department>(HrmService.GetAllDepartmentDetails());
                if (DepartmentList.Count > 0)
                {
                    SelectedDepartment = DepartmentList[0];
                }


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Refresh Department List. 
        /// </summary>

        public void RefreshDepartmentList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDepartmentList()...", category: Category.Info, priority: Priority.Low);

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

                DepartmentList = new ObservableCollection<Department>(HrmService.GetAllDepartmentDetails());

                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshDepartmentList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshDepartmentList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Print Department Details List. 
        /// </summary>
        private void PrintDepartmentList(object obj)
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["DepartmentReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["DepartmentReportPrintFooterTemplate"];
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

        /// <summary>
        /// Method for Export Department Details in Excel Sheet. 
        /// </summary>
        private void ExportDepartmentList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportDepartmentList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Department List";
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

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        /// <summary>
        /// Method for Add New Department Information . 
        /// </summary>
        private void AddNewDepartmentInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewDepartmentInformation()...", category: Category.Info, priority: Priority.Low);

                AddDepartmentView addDepartmentView = new AddDepartmentView();
                AddDepartmentViewModel addDepartmentViewModel = new AddDepartmentViewModel();
                EventHandler handle = delegate { addDepartmentView.Close(); };
                addDepartmentViewModel.RequestClose += handle;
                addDepartmentView.DataContext = addDepartmentViewModel;
                addDepartmentViewModel.Init(DepartmentList);
                addDepartmentViewModel.IsNew = true;
                addDepartmentView.ShowDialog();

                if (addDepartmentViewModel.IsSave == true)
                {
                    DepartmentList.Add(addDepartmentViewModel.NewDepartment);
                    SelectedDepartment = addDepartmentViewModel.NewDepartment;
                }
                GeosApplication.Instance.Logger.Log("Method AddNewDepartmentInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewDepartmentInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Department Information. 
        /// [001][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// </summary>
        private void EditDepartmentInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditDepartmentInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                Department department = (Department)detailView.FocusedRow;
                SelectedDepartment = department;
                if (department != null)
                {
                    AddDepartmentView addDepartmentView = new AddDepartmentView();
                    AddDepartmentViewModel addDepartmentViewModel = new AddDepartmentViewModel();
                    EventHandler handle = delegate { addDepartmentView.Close(); };
                    addDepartmentViewModel.RequestClose += handle;
                    addDepartmentView.DataContext = addDepartmentViewModel;
                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addDepartmentViewModel.InitReadOnly(DepartmentList, department);
                    else
                        addDepartmentViewModel.EditInit(DepartmentList, department);

                    addDepartmentViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addDepartmentView.Owner = Window.GetWindow(ownerInfo);
                    addDepartmentView.ShowDialog();
                    if (addDepartmentViewModel.IsSave == true)
                    {
                        department.IdDepartment = addDepartmentViewModel.EditDepartment.IdDepartment;
                        department.DepartmentName = addDepartmentViewModel.EditDepartment.DepartmentName;
                        department.IdDepartmentParent = addDepartmentViewModel.EditDepartment.ParentDepartment.IdDepartment;
                        department.Abbreviation = addDepartmentViewModel.EditDepartment.Abbreviation;
                        department.ParentDepartment = addDepartmentViewModel.EditDepartment.ParentDepartment;
                        department.IdDepartmentArea = (uint)addDepartmentViewModel.EditDepartment.DepartmentArea.IdLookupValue;
                        department.DepartmentArea = addDepartmentViewModel.EditDepartment.DepartmentArea;
                        department.DepartmentHtmlColor = addDepartmentViewModel.EditDepartment.DepartmentHtmlColor;
                        //department.DepartmentIsIsolated = addDepartmentViewModel.EditDepartment.DepartmentIsIsolated; //[001] Removed
                        department.DepartmentInUse = addDepartmentViewModel.EditDepartment.DepartmentInUse;

                        SelectedDepartment = department;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditDepartmentInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditDepartmentInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    IsAcceptEnabled = false;
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
