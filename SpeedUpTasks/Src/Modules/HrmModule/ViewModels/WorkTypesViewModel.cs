using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common.Epc;
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
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class WorkTypesViewModel: INotifyPropertyChanged
    {

        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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

        #region Declaration
        private ObservableCollection<LookupValue> workTypesList;
        private LookupValue selectedWorkType;
        #endregion

        #region Properties
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public ObservableCollection<LookupValue> WorkTypesList
        {
            get
            {
                return workTypesList;
            }

            set
            {
                workTypesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkTypesList"));
            }
        }

        public LookupValue SelectedWorkType
        {
            get
            {
                return selectedWorkType;
            }

            set
            {
                selectedWorkType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkType"));
            }
        }

        #endregion

        #region Public ICommand
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        //public ICommand AddWorkTypeCommand { get; set; }
        //public ICommand EditWorkTypeDoubleClickCommand { get; set; }
        #endregion // Public Commands

        #region Constructor
        public WorkTypesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor LeavesViewModel()...", category: Category.Info, priority: Priority.Low);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintWorkTypesList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportWorkTypesList));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshWorkTypesList));
                //AddWorkTypeCommand = new RelayCommand(new Action<object>(AddWorkTypeCommandAction));
                //EditWorkTypeDoubleClickCommand = new DelegateCommand<object>(EditWorkTypeInformation);
                GeosApplication.Instance.Logger.Log("Constructor LeavesViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor LeavesViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
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

                FillWorkType();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to print Work Types List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintWorkTypesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorkTypesList()...", category: Category.Info, priority: Priority.Low);
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["WorkTypesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["WorkTypesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintWorkTypesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkTypesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method to Export Work Types List
        /// </summary>
        /// <param name="obj"></param>
        private void ExportWorkTypesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkTypesList()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Work Types";
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
                    TableView WorkTypesTableView = ((TableView)obj);
                    WorkTypesTableView.ShowTotalSummary = false;
                    WorkTypesTableView.ShowFixedTotalSummary = false;
                    WorkTypesTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    WorkTypesTableView.ShowTotalSummary = false;
                    WorkTypesTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportWorkTypesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWorkTypesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void RefreshWorkTypesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWorkTypesList()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
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

                detailView.SearchString = null;
                FillWorkType();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshWorkTypesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorkTypesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to get work types list
        /// </summary>
        public void FillWorkType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkType()...", category: Category.Info, priority: Priority.Low);
                WorkTypesList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(33).ToList());
                if (WorkTypesList.Count > 0)
                    SelectedWorkType = WorkTypesList[0];
                GeosApplication.Instance.Logger.Log("Method FillWorkType()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkType() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillWorkType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void AddWorkTypeCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method AddWorkTypeCommandAction()...", category: Category.Info, priority: Priority.Low);
        //        AddWorkTypeView addWorkTypeView = new AddWorkTypeView();
        //        AddWorkTypeViewModel addWorkTypeViewModel = new AddWorkTypeViewModel();
        //        EventHandler handle = delegate { addWorkTypeView.Close(); };
        //        addWorkTypeViewModel.RequestClose += handle;
        //        addWorkTypeView.DataContext = addWorkTypeViewModel;
        //        addWorkTypeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddWorkType").ToString();
        //        addWorkTypeViewModel.IsNew = true;
        //        addWorkTypeView.ShowDialog();
        //        GeosApplication.Instance.Logger.Log("Method AddWorkTypeCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method AddWorkTypeCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        /// <summary>
        /// Method to Edit Work Type Information
        /// </summary>
        /// <param name="obj"></param>
        //private void EditWorkTypeInformation(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method EditWorkTypeInformation()...", category: Category.Info, priority: Priority.Low);
        //        TableView detailView = (TableView)obj;

        //        GeosApplication.Instance.Logger.Log("Method EditWorkTypeInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method EditWorkTypeInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        #endregion
    }
}
