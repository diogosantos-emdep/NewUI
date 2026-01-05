
using DevExpress.Data;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.PCM.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class HardLockLicensesViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // IPCMService PCMService = new PCMServiceController("localhost:6699");
        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler RequestClose;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Declarations
        private string title;
        private DataTable localDataTable;
        private DataTable dataTable;
        private BandItem bandPlugin;
        private ObservableCollection<HardLockLicenses> hardLockLicensesList;
        private ObservableCollection<BandItem> bands;
        private ObservableCollection<Summary> totalSummary;
        private string myFilterString;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private HardLockLicenses selectedArticle;
        #endregion

        #region Properties
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }
        public BandItem BandPlugin
        {
            get { return bandPlugin; }
            set
            {
                bandPlugin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandPlugin"));
            }
        }
        public ObservableCollection<HardLockLicenses> HardLockLicensesList
        {
            get { return hardLockLicensesList; }
            set
            {
                hardLockLicensesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HardLockLicensesList"));
            }
        }
        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
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
        public DataTable DataTable
        {
            get { return dataTable; }
            set
            {
                dataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTable"));
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

        public HardLockLicenses SelectedArticle
        {
            get { return selectedArticle; }
            set
            {
                selectedArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticle"));
            }
        }
        #endregion

        #region ICommands
        public ICommand EditArticleViewCommand { get; set; }
        public ICommand RefreshHardLockLicensesCommand { get; set; }//[Sudhir.Jangra][GEOS2-4900]

        public ICommand PrintHardLockLicensesCommand { get; set; }//[Sudhir.Jangra][GEOS2-4900]

        public ICommand ExportHardLockLicensesCommand { get; set; }//[Sudhir.Jangra][GEOS2-4900]

        public ICommand AddHardLockLicenseButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-4900]

        public ICommand EditHardLockLicenseCommand { get; set; }//[Sudhir.Jangra][GEOS2-4901]
        #endregion

        #region Constructor
        public HardLockLicensesViewModel()
        {
            EditArticleViewCommand = new RelayCommand(new Action<object>(EditArticleViewCommandAction));
            RefreshHardLockLicensesCommand = new RelayCommand(new Action<object>(RefreshHardLockLicensesCommandAction));//[Sudhir.jangra][GEOS2-4900]
            PrintHardLockLicensesCommand = new RelayCommand(new Action<object>(PrintHardLockLicensesCommandAction));//[Sudhir.Jangra][GEOS2-4900]
            ExportHardLockLicensesCommand = new RelayCommand(new Action<object>(ExportHardLockLicensesCommandAction));//[Sudhir.Jangra][GEOS2-4900]
            AddHardLockLicenseButtonCommand = new RelayCommand(new Action<object>(AddHardLockLicenseButtonCommandAction));//[Sudhir.Jangra][GEOS2-4901]
            if (GeosApplication.Instance.IsPCMAddEditPermissionForHardLockLicense)
            {
                EditHardLockLicenseCommand = new RelayCommand(new Action<object>(EditHardLockLicenseCommandAction));//[Sudhir.jangra][GEOS2-4901]
            }

            BandPlugin = new BandItem() { BandName = "Supported Plugins", BandHeader = "Supported Plugins" };
            MyFilterString = string.Empty;
            SelectedArticle = new HardLockLicenses();
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                // HardLockLicensesList = new ObservableCollection<HardLockLicenses>(PCMService.GetAllHardLockLicenses_V2440());

                 var temp = new ObservableCollection<HardLockLicenses>(PCMService.GetAllHardLockLicenses_V2440());
                HardLockLicensesList = new ObservableCollection<HardLockLicenses>(temp.GroupBy(h=>h.IdArticle).Select(g=>g.First()));
                AddColumnsToDataTable();
                FillDataTable();
                MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddColumnsToDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);
                List<BandItem> bandsLocal = new List<BandItem>();
                BandItem bandAll = new BandItem() { FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandAll.Columns = new ObservableCollection<ColumnItem>();
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Reference", HeaderText = "Reference", Width = 200, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Reference, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Description", HeaderText = "Description", Width = 250, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Description, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "IdArticle", HeaderText = "IdArticle", Width = 250, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Hidden, Visible = false });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "IdPCMArticle", HeaderText = "IdPCMArticle", Width = 250, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Hidden, Visible = false });

                bandsLocal.Add(bandAll);

                localDataTable = new DataTable();
                localDataTable.Columns.Add("Reference", typeof(string));
                localDataTable.Columns.Add("Description", typeof(string));
                localDataTable.Columns.Add("IdArticle", typeof(uint));
                localDataTable.Columns.Add("IdPCMArticle", typeof(uint));

                bandsLocal.Add(BandPlugin);
                BandPlugin.Columns = new ObservableCollection<ColumnItem>();

                foreach (HardLockLicenses hardLockLicenses in HardLockLicensesList)
                {
                    if (hardLockLicenses.HardLockPluginList != null && hardLockLicenses.HardLockPluginList.Count > 0)
                    {
                        foreach (HardLockPlugins item in hardLockLicenses.HardLockPluginList)
                        {
                            if (!string.IsNullOrEmpty(item.Name) && !BandPlugin.Columns.Any(x => x.ColumnFieldName == item.IdPlugin.ToString()))
                            {
                                string fieldName = item.IdPlugin.ToString();
                                localDataTable.Columns.Add(fieldName, typeof(string));
                                BandPlugin.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = item.Name, Width = 45, Visible = true, IsVertical = true, ProductTypeSettings = ProductTypeSettingsType.Description });
                            }
                        }
                    }
                }

                BandPlugin.Columns = new ObservableCollection<ColumnItem>(BandPlugin.Columns.OrderBy(x => x.HeaderText));

                Bands = new ObservableCollection<BandItem>(bandsLocal);

                TotalSummary = new ObservableCollection<Summary>() { new Summary() { Type = SummaryItemType.Count, FieldName = "Reference", DisplayFormat = "Total : {0}" } };
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTable() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddDataTableColumns() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);
                localDataTable.Rows.Clear();

                foreach (HardLockLicenses item in HardLockLicensesList)
                {
                    DataRow dr = localDataTable.NewRow();
                    dr["IdArticle"] = item.IdArticle;
                    dr["IdPCMArticle"] = item.IdPCMArticle;
                    dr["Reference"] = item.Reference;
                    dr["Description"] = item.Description;

                    if (item.HardLockPluginList != null)
                    {
                        foreach (HardLockPlugins plugin in item.HardLockPluginList)
                        {
                            if (!string.IsNullOrEmpty(plugin.Name))
                            {
                                dr[plugin.IdPlugin.ToString()] = "X";
                            }
                        }
                    }
                    localDataTable.Rows.Add(dr);
                }
                DataTable = localDataTable;
                GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillDataTable() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditArticleViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleViewCommandAction()..."), category: Category.Info, priority: Priority.Low);
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
                if (obj == null)
                {
                    return;
                }
                Articles articles = null;
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                if ((System.Data.DataRowView)detailView.DataControl.CurrentItem != null)
                {
                    DataRowView dr = (System.Data.DataRowView)detailView.DataControl.CurrentItem;
                    Articles selectedRow = new Articles();
                    selectedRow.IdArticle = Convert.ToUInt32(dr.Row["IdArticle"]);
                    selectedRow.IdPCMArticle = Convert.ToUInt32(dr.Row["IdPCMArticle"]);
                    selectedRow.IsHardLockPluginEditView = true;
                    //selectedRow.ArticleImageInBytes = Convert.ToByte(dr.Row["ArticleImageInBytes"]);
                    if (selectedRow.IdPCMArticle != 0)
                    {
                        EditPCMArticleView editPCMArticleView = new EditPCMArticleView();
                        EditPCMArticleViewModel editPCMArticleViewModel = new EditPCMArticleViewModel();
                        EventHandler handle = delegate { editPCMArticleView.Close(); };
                        editPCMArticleViewModel.RequestClose += handle;
                        editPCMArticleViewModel.IsNew = false;


                        editPCMArticleViewModel.EditInit(selectedRow);
                        editPCMArticleView.DataContext = editPCMArticleViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        var ownerInfo = (detailView as FrameworkElement);

                        editPCMArticleView.Owner = Window.GetWindow(ownerInfo);
                        editPCMArticleViewModel.IsEnabledCancelButton = false;
                        editPCMArticleView.ShowDialog();

                        if (editPCMArticleViewModel.IsSave)
                        {

                        }
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(Application.Current.Resources["PCM_NotPCMArticle"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                    }

                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleViewCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditArticleViewCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-4900]
        private void RefreshHardLockLicensesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshHardLockLicensesCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                Init();
                detailView.SearchString = null;
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RefreshHardLockLicensesCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-4900]
        private void PrintHardLockLicensesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintHardLockLicensesCommandAction()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;

                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["HardLockReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["HardLockReportPrintFooterTemplate"];

                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;

                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintHardLockLicensesCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method PrintHardLockLicensesCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[Sudhir.Jangra][GEOS2-4900]
        private void ExportHardLockLicensesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportHardLockLicensesCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";

                saveFile.FileName = "HardLock Licenses List";
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
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    activityTableView.ShowTotalSummary = true;
                  //  activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportHardLockLicensesCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in Method ExportHardLockLicensesCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.jangra][GEOS-4900]
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        //[Sudhir.Jangra][GEOS2-4901]
        private void AddHardLockLicenseButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddHardLockLicenseButtonCommandAction().", category: Category.Info, priority: Priority.Low);
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
                AddEditHardLockLicenseView addEditHardLockLicenseView = new AddEditHardLockLicenseView();
                AddEditHardLockLicensesViewModel addEditHardLockLicensesViewModel = new AddEditHardLockLicensesViewModel();
                EventHandler handle = delegate { addEditHardLockLicenseView.Close(); };
                addEditHardLockLicensesViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddHardLockPluginHeader").ToString();
                addEditHardLockLicensesViewModel.IsNew = true;
                addEditHardLockLicensesViewModel.HardLockGridList = HardLockLicensesList.ToList();
                addEditHardLockLicensesViewModel.Init();
                addEditHardLockLicensesViewModel.RequestClose += handle;
                addEditHardLockLicenseView.DataContext = addEditHardLockLicensesViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditHardLockLicenseView.Owner = Window.GetWindow(ownerInfo);
                addEditHardLockLicenseView.ShowDialog();

                if (addEditHardLockLicensesViewModel.IsSave)
                {
                    DataRow dataRow = DataTable.NewRow();
                    dataRow["IdArticle"] = addEditHardLockLicensesViewModel.SelectedReference.IdArticle;
                    dataRow["IdPCMArticle"] = addEditHardLockLicensesViewModel.SelectedReference.IdPCMArticle;
                    dataRow["Reference"] = addEditHardLockLicensesViewModel.SelectedReference.Reference;
                    dataRow["Description"] = addEditHardLockLicensesViewModel.SelectedReference.Description;
                    if (addEditHardLockLicensesViewModel.SupportedPluginList!=null)
                    {
                        foreach (HardLockPlugins item in addEditHardLockLicensesViewModel.SupportedPluginList)
                        {
                            if (DataTable.Columns.Contains(item.IdPlugin.ToString()))
                            {
                                dataRow[item.IdPlugin.ToString()] = "X";
                            }
                            else
                            {
                                DataTable.Columns.Add(item.IdPlugin.ToString(), typeof(string));
                                BandPlugin.Columns.Add(new ColumnItem() { ColumnFieldName = item.IdPlugin.ToString(), HeaderText = item.Name, Width = 45, Visible = true, IsVertical = true });
                            }
                        }
                    }
                    DataTable.Rows.InsertAt(dataRow, 0);
                    HardLockLicensesList.Add(addEditHardLockLicensesViewModel.SelectedReference);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddHardLockLicenseButtonCommandAction(). executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddHardLockLicenseButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-4901]
        private void EditHardLockLicenseCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditHardLockLicenseCommandAction()...", category: Category.Info, priority: Priority.Low);
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
                GridControl gridControl = (detailView).Grid;
                if ((System.Data.DataRowView)detailView.DataControl.CurrentItem!=null)
                {
                    DataRowView dr = (System.Data.DataRowView)detailView.DataControl.CurrentItem;
                    Articles selectedRow = new Articles();
                    selectedRow.IdArticle = Convert.ToUInt32(dr.Row["IdArticle"]);
                    selectedRow.IdPCMArticle = Convert.ToUInt32(dr.Row["IdPCMArticle"]);
                    selectedRow.Reference = Convert.ToString(dr.Row["Reference"]);
                    selectedRow.Description = Convert.ToString(dr.Row["Description"]);
                    // HardLockLicenses selectedRow = (HardLockLicenses)detailView.FocusedRow;


                    AddEditHardLockLicenseView addEditHardLockLicenseView = new AddEditHardLockLicenseView();
                AddEditHardLockLicensesViewModel addEditHardLockLicensesViewModel = new AddEditHardLockLicensesViewModel();
                EventHandler handle = delegate { addEditHardLockLicenseView.Close(); };
                addEditHardLockLicensesViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditHardLockPluginHeader").ToString();
                addEditHardLockLicensesViewModel.IsNew = false;
                // addEditHardLockLicensesViewModel.HardLockGridList = HardLockLicensesList.ToList();
                addEditHardLockLicensesViewModel.EditInit(selectedRow);
                addEditHardLockLicensesViewModel.RequestClose += handle;
                addEditHardLockLicenseView.DataContext = addEditHardLockLicensesViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditHardLockLicenseView.Owner = Window.GetWindow(ownerInfo);
                addEditHardLockLicenseView.ShowDialog();

                if (addEditHardLockLicensesViewModel.IsSave|| addEditHardLockLicensesViewModel.IsDeleted)
                {
                        dr["IdArticle"] = addEditHardLockLicensesViewModel.SelectedReference.IdArticle;
                        dr["IdPCMArticle"]= addEditHardLockLicensesViewModel.SelectedReference.IdPCMArticle;
                        dr["Reference"] = addEditHardLockLicensesViewModel.SelectedReference.Reference;
                        dr["Description"] = addEditHardLockLicensesViewModel.SelectedReference.Description;

                        if (addEditHardLockLicensesViewModel.SupportedPluginList!=null)
                        {
                            foreach (HardLockPlugins item in addEditHardLockLicensesViewModel.SupportedPluginList)
                            {
                                if (item.TransactionOperation==ModelBase.TransactionOperations.Add)
                                {
                                    if (!string.IsNullOrEmpty(item.Name))
                                    {
                                        if (!BandPlugin.Columns.Any(x=>x.ColumnFieldName==item.IdPlugin.ToString()))
                                        {
                                            string fieldName = item.IdPlugin.ToString();
                                            DataTable.Columns.Add(fieldName, typeof(string));
                                            BandPlugin.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = item.Name, Width = 45, Visible = true, IsVertical = true });
                                            dr[item.IdPlugin.ToString()] = "X";
                                        }
                                        else if (BandPlugin.Columns.Any(x=>x.ColumnFieldName==item.IdPlugin.ToString()))
                                        {
                                            dr[item.IdPlugin.ToString()] = "X";
                                        }
                                    }
                                    else if (item.TransactionOperation==ModelBase.TransactionOperations.Delete)
                                    {
                                        dr[item.IdPlugin.ToString()] = "X";
                                    }
                                }
                            }
                        }
                        if (addEditHardLockLicensesViewModel.DeletedPluginList != null)
                        {
                            foreach (var item in addEditHardLockLicensesViewModel.DeletedPluginList)
                            {
                                if (!string.IsNullOrEmpty(item.Name))
                                {
                                    dr[item.IdPlugin.ToString()] = "";
                                }
                            }
                        }
                        gridControl.RefreshData();
                        gridControl.UpdateLayout();
                        Init();
                    }
                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditHardLockLicenseCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditHardLockLicenseCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion
    }
}
