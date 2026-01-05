using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.Windows.Input;
using DevExpress.Export;
using DevExpress.Export.Xl;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using System.Collections.ObjectModel;
using System.IO;
using Emdep.Geos.Data.Common.SCM;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.Modules.SCM.Common_Classes;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.UI.Helper;
using System.Collections.Generic;
using DevExpress.Data.Filtering;
using DevExpress.Mvvm.Native;
using System.Linq;
using Emdep.Geos.Utility;
using System.ServiceModel;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class FamiliesManagerViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
        #endregion

        #region Declaration
        string myFilterString;
        private ObservableCollection<ConnectorFamily> listConnectorFamilies;
        private ConnectorFamily selectedConnectorFamilies;
        public string FamiliesManagerSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "SCM_FamiliesManagerSetting.Xml";
        private ImageSource attachmentImage;
        private ConnectorFamily selectConnectorFamilies;
        private List<string> name;
        private bool isSCMEditFamiliesManager;//[pramod.misal][GEOS2-5482][24.05.2024]
        #endregion

        #region Properties

        //[pramod.misal][GEOS2-5482][24.05.2024]
        public bool IsSCMEditFamiliesManager
        {
            get { return isSCMEditFamiliesManager; }
            set
            {

                isSCMEditFamiliesManager = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditFamiliesManager"));

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
        public ObservableCollection<ConnectorFamily> ListConnectorFamilies
        {
            get { return listConnectorFamilies; }
            set { listConnectorFamilies = value; OnPropertyChanged(new PropertyChangedEventArgs("ListConnectorFamilies")); }
        }
        public ConnectorFamily SelectedConnectorFamilies
        {
            get { return selectedConnectorFamilies; }
            set
            { selectedConnectorFamilies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorFamilies")); }
        }
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public ImageSource AttachmentImage
        {
            get
            {
                return attachmentImage;
            }

            set
            {
                attachmentImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentImage"));
            }
        }
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
        public List<string> Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        public ConnectorFamily SelectConnectorFamilies
        {
            get { return selectConnectorFamilies; }
            set
            {
                selectConnectorFamilies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectConnectorFamilies"));
            }
        }
        public void Dispose()
        {

        }
        #endregion

        #region Public ICommand
        public ICommand FamilyManagerViewCancelButtonCommand { get; set; }
        public ICommand AddFamilyManagerButtonCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand CurrentItemChangedCommand { get; set; }
        public ICommand RefreshFamilyCommand { get; set; }
        public ICommand EditFamilymanagerHyperlinkClickCommand { get; set; }
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]

        #endregion

        #region Constructor
        public FamiliesManagerViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FamiliesManagerViewModel ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddFamilyManagerButtonCommand= new RelayCommand(new Action<object>(AddButtonCommandAction));
                FamilyManagerViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                PrintCommand = new RelayCommand(new Action<object>(PrintCommandAction));
                ExportCommand = new RelayCommand(new Action<object>(ExportCommandAction));
                CurrentItemChangedCommand = new DelegateCommand<object>(CurrentItemChangedCommandAction);
                RefreshFamilyCommand = new RelayCommand(new Action<object>(RefreshFamilyCommandAction));
                EditFamilymanagerHyperlinkClickCommand= new RelayCommand(new Action<object>(HyperlinkClickCommandAction));
                MyFilterString = string.Empty;
                //[pramod.misal][GEOS2-5482][23.05.2024]
                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditFamiliesManager)
                {
                    IsSCMEditFamiliesManager = true;
                }
                else
                {
                    IsSCMEditFamiliesManager = false;
                }
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor FamiliesManagerViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FamiliesManagerViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

        #region Method
        public void Init()
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
                AttachmentImage = null;
                //Service GetConnectorFamilies updated with GetConnectorFamilies_V2430 [rdixit][01.09.2023][GEOS2-4565]
                //Service GetConnectorFamilies_V2430 updated with GetConnectorFamilies_V2450 [rdixit][19.10.2023][GEOS2-4958]
                //[rdixit][05.03.2025][GEOS2-7026] 
                List<ConnectorFamily> tmpList = SCMService.GetConnectorFamilies_V2620();
                ListConnectorFamilies = new ObservableCollection<ConnectorFamily>(tmpList);
                foreach (var item in ListConnectorFamilies)
                {
                    if(item.IdSubFamilyConnector != 0)
                    {
                        item.Familyname = tmpList.FirstOrDefault(i => i.IdFamily == item.IdFamily && i.IdSubFamilyConnector == 0).Name_es;
                    }
                }
                SelectedConnectorFamilies = ListConnectorFamilies.FirstOrDefault();
                MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CurrentItemChangedCommandAction(object obj)
        {
            try
            {
                CurrentItemChangedEventArgs tempObj =(CurrentItemChangedEventArgs)obj;
                //[rdixit][03.03.2025][GEOS2-6989]
                if (tempObj.NewItem != null)
                {
                    ConnectorFamily NewData = (ConnectorFamily)tempObj.NewItem;
                    if ((NewData.ConnectorFamilyImageInBytes != null || NewData.ConnectorFamilyImageInBytes != null) && NewData.Position == 1)
                    {
                        AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(NewData.ConnectorFamilyImageInBytes);
                    }
                    else
                    {
                        AttachmentImage = null;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CurrentItemChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintCommandAction()...", category: Category.Info, priority: Priority.Low);
                // IsBusy = true;
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
                PrintableControlLink pcl = new PrintableControlLink((TreeListView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                //pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["WorkOrderListReportPrintHeaderTemplate"];
                //pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["WorkOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;

                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                // IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                //  IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportCommandAction()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Families Report";
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
                    // IsBusy = true;
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
                    TreeListView activityTableView = ((TreeListView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    //  IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                // IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Options_CustomizeCell()...", category: Category.Info, priority: Priority.Low);
                if (e.ColumnFieldName == "CostDeviation")
                {
                    e.Formatting.Alignment = new XlCellAlignment() { HorizontalAlignment = XlHorizontalAlignment.Right };
                    e.Formatting.FormatType = DevExpress.Utils.FormatType.Numeric;
                    e.Formatting.NumberFormat = "0%";
                }

                e.Handled = true;
                GeosApplication.Instance.Logger.Log("Method Options_CustomizeCell()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Options_CustomizeCell()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        private void AddButtonCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView().", category: Category.Info, priority: Priority.Low);
              
                Name = ListConnectorFamilies.Select(i => i.Name).ToList();
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddFamilyPropertyView addFamilyPropertyView = new AddFamilyPropertyView();
                AddFamilyPropertyViewModel addFamilyPropertyViewModel = new AddFamilyPropertyViewModel();
                addFamilyPropertyViewModel.WindowHeader = System.Windows.Application.Current.FindResource("SCMAddFamilyPropertyTitle").ToString();
                EventHandler handle = delegate { addFamilyPropertyView.Close(); };
                addFamilyPropertyViewModel.RequestClose += handle;
                addFamilyPropertyView.DataContext = addFamilyPropertyViewModel;
                addFamilyPropertyViewModel.Init(Name);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addFamilyPropertyView.ShowDialogWindow();
                //[GEOS2-4563][rupali sarode][31-07-2023]
                if (addFamilyPropertyViewModel.IsSave)
                {
                    Init();             
                }

                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
           
        }
        private void RefreshFamilyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView().", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                Init();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }               
                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method RefreshFamilyCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void HyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView().", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                TreeListView detailView = (TreeListView)obj;
                ConnectorFamily SelectedRow = (ConnectorFamily)detailView.DataControl.SelectedItem;
                Name = ListConnectorFamilies.Select(i => i.Name).ToList();
          
                List<ConnectorFamily> ConnectorFamiliesList = new List<ConnectorFamily>();
                ConnectorFamiliesList = ListConnectorFamilies.Where(a => a.IdFamily == SelectedRow.IdFamily && a.Key!= "Parent_"+ SelectedRow.IdFamily).ToList();

                #region subfamily
                if (SelectedRow.IdSubFamilyConnector!=0)
                {
                    AddSubFamilyView addSubFamilyView = new AddSubFamilyView();
                    AddSubFamilyViewModel addSubFamilyViewModel = new AddSubFamilyViewModel();
                    addSubFamilyViewModel.WindowHeader = Application.Current.FindResource("SCMEditSubFamilyPropertyTitle").ToString();
                    EventHandler handle = delegate { addSubFamilyView.Close(); };
                    addSubFamilyViewModel.RequestClose += handle;
                    addSubFamilyViewModel.IsNew = false;
                    addSubFamilyView.DataContext = addSubFamilyViewModel;
                    addSubFamilyViewModel.EditInit(Convert.ToInt32(SelectedRow.IdSubFamilyConnector), SelectedRow.Familyname);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    var ownerInfo = (obj as FrameworkElement);
                    addSubFamilyView.Owner = Window.GetWindow(ownerInfo);
                    addSubFamilyView.ShowDialogWindow();
                    if (addSubFamilyViewModel.IsUpdate)
                    {
                        Init();                       
                    }
                }
                else
                {
                    byte[] AttachmentImage = SelectedRow.ConnectorFamilyImageInBytes;
                    AddFamilyPropertyView addFamilyPropertyView = new AddFamilyPropertyView();
                    AddFamilyPropertyViewModel addFamilyPropertyViewModel = new AddFamilyPropertyViewModel();
                    addFamilyPropertyViewModel.WindowHeader = System.Windows.Application.Current.FindResource("SCMEditFamilyPropertyTitle").ToString();
                    EventHandler handle = delegate { addFamilyPropertyView.Close(); };
                    addFamilyPropertyViewModel.RequestClose += handle;
                    addFamilyPropertyViewModel.IsNew = false;
                    addFamilyPropertyView.DataContext = addFamilyPropertyViewModel;
                    //[rdixit][03.03.2025][GEOS2-6989]
                    Name = ListConnectorFamilies.Where(i=>i.Name.ToUpper() != SelectedRow.Name.ToUpper()).Select(i => i.Name).ToList();
                    addFamilyPropertyViewModel.EditInit(SelectedRow, Name);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    var ownerInfo = (obj as FrameworkElement);
                    addFamilyPropertyView.Owner = Window.GetWindow(ownerInfo);
                    addFamilyPropertyView.ShowDialogWindow();
                    //[GEOS2-4563][rupali sarode][31-07-2023]
                    if (addFamilyPropertyViewModel.IsUpdate)
                    {
                        Init();                     
                    }
                }
                #endregion
                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                //if (SCMShortcuts.Instance.IsActive)
                //{
                //    RequestClose(null, null);
                //}
                //GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
