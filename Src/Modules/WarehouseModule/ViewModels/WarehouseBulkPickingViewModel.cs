using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.WMS;
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
    public class WarehouseBulkPickingViewModel : INotifyPropertyChanged
    {
        #region Service

         IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

       // IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        #endregion

        #region Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Declaration
        private ObservableCollection<WarehouseBulkArticle> bulkPickingList;

        private WarehouseBulkArticle selectedBulkPickingList;

        private WarehouseBulkArticle deletedBulkPickingList;

        private bool IsBusy { get; set; }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private string myFilterString;
        #endregion

        #region Public Properties
        public ObservableCollection<WarehouseBulkArticle> BulkPickingList
        {
            get { return bulkPickingList; }
            set
            {
                bulkPickingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BulkPickingList"));
            }
        }
        public WarehouseBulkArticle SelectedBulkPickingList
        {
            get { return selectedBulkPickingList; }
            set
            {
                selectedBulkPickingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBulkPickingList"));
            }
        }
        public WarehouseBulkArticle DeletedBulkPickingList
        {
            get { return deletedBulkPickingList; }
            set
            {
                deletedBulkPickingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedBulkPickingList"));
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

        #endregion

        #region ICommand
        public ICommand AddNewBulkPickingCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }

        public ICommand RefreshButtonCommand { get; set; }

        public ICommand DeleteButtonCommand { get; set; }

        public ICommand PrintBulkPickingCommand { get; set; }

        public ICommand ExportBulkPickingCommand { get; set; }

        public ICommand CommandWarehouseEditValueChanged { get; set; }
        #endregion

        #region Constructor


        public WarehouseBulkPickingViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WarehouseBulkPickingViewModel....", category: Category.Info, priority: Priority.Low);
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
                AddNewBulkPickingCommand = new DelegateCommand<object>(AddNewBulkPickingCommandAction);
                CommandGridDoubleClick = new DelegateCommand<object>(EditBulkPickingCommandAction);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
                DeleteButtonCommand = new RelayCommand(new Action<object>(DeleteButtonCommandAction));
                PrintBulkPickingCommand = new RelayCommand(new Action<object>(PrintBulkPickingCommandAction));
                ExportBulkPickingCommand = new RelayCommand(new Action<object>(ExportBulkPickingCommandAction));
                CommandWarehouseEditValueChanged = new DelegateCommand<object>(CommandWarehouseEditValueChangedAction);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor  WarehouseBulkPickingViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  WarehouseQuotaViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        private void CommandWarehouseEditValueChangedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);

            //When setting the warehouse from default the data should not be refreshed
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;

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


            FillBulkList();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }
        private void ExportBulkPickingCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportDepartmentList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Bulk Picking List";
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
                    departmentTableView.Grid.Columns[2].Visible = false;
                    departmentTableView.ExportToXlsx(ResultFileName);
                    departmentTableView.Grid.Columns[2].Visible = true;
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

        private void PrintBulkPickingCommandAction(object obj)
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

        private void DeleteButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteBulkPickingDetailsMessageWithoutName"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    DeletedBulkPickingList = new WarehouseBulkArticle();
                    DeletedBulkPickingList.TransactionOperation = ModelBase.TransactionOperations.Delete;
                    DeletedBulkPickingList.IdWarehouseBulkArticle = SelectedBulkPickingList.IdWarehouseBulkArticle;
                    DeletedBulkPickingList = WarehouseService.AddUpdateWarehouseBulkArticle(DeletedBulkPickingList);
                    BulkPickingList.Remove(SelectedBulkPickingList);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("BulkPickingDetailsDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshButtonCommandAction(object obj)
        {
            FillBulkList();
        }
        private void FillBulkList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
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
                // Selectedwarehouse = WarehouseCommon.Instance.Selectedwarehouse;
                BulkPickingList = new ObservableCollection<WarehouseBulkArticle>(WarehouseService.GetWarehouseBulkArticle(WarehouseCommon.Instance.Selectedwarehouse));

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            FillBulkList();
        }
        private void EditBulkPickingCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditBulkPickingCommandAction()...", category: Category.Info, priority: Priority.Low);


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
                GridControl gridControl = (detailView).Grid;
                WarehouseBulkArticle selectedRow = (WarehouseBulkArticle)detailView.FocusedRow;
                long IdWarehouseBulkArticle = selectedRow.IdWarehouseBulkArticle;

                AddWarehouseBulkPickingView addWarehouseBulkPickingView = new AddWarehouseBulkPickingView();
                AddWarehouseBulkPickingViewModel addWarehouseBulkPickingViewModel = new AddWarehouseBulkPickingViewModel();
                EventHandler handle = delegate { addWarehouseBulkPickingView.Close(); };
                addWarehouseBulkPickingViewModel.RequestClose += handle;
                addWarehouseBulkPickingViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditBulkPickingViewTitle").ToString();
                addWarehouseBulkPickingView.DataContext = addWarehouseBulkPickingViewModel;
                addWarehouseBulkPickingViewModel.EditINIT(selectedRow);
                addWarehouseBulkPickingViewModel.IsNew = false;
                addWarehouseBulkPickingView.ShowDialogWindow();

                if (addWarehouseBulkPickingViewModel.IsSave == true)
                {
                    FillBulkList();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditBulkPickingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditBulkPickingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddNewBulkPickingCommandAction(object obj)
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
                AddWarehouseBulkPickingView addWarehouseBulkPickingView = new AddWarehouseBulkPickingView();
                AddWarehouseBulkPickingViewModel addWarehouseBulkPickingViewModel = new AddWarehouseBulkPickingViewModel();
                EventHandler handle = delegate { addWarehouseBulkPickingView.Close(); };
                addWarehouseBulkPickingViewModel.RequestClose += handle;
                addWarehouseBulkPickingViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddBulkPickingViewTitle").ToString();
                addWarehouseBulkPickingView.DataContext = addWarehouseBulkPickingViewModel;
                addWarehouseBulkPickingViewModel.ADDINIT();
                addWarehouseBulkPickingViewModel.IsNew = true;
                addWarehouseBulkPickingView.ShowDialogWindow();

                if (addWarehouseBulkPickingViewModel.IsSave == true)
                {
                    FillBulkList();


                }




                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddNewBulkPickingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewBulkPickingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion



    }
}
