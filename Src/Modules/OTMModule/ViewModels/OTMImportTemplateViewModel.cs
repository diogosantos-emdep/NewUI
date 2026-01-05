using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
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
using System.Windows.Media;
using System.Windows;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.OTM.Views;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.OTM;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Microsoft.Win32;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.Data;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.Modules.OTM.CommonClass;
using System.IO;
using System.Xml.Linq;
using Emdep.Geos.UI.Helper;



namespace Emdep.Geos.Modules.OTM.ViewModels
{
    // <!--[Rahul.Gadhave][GEOS2-6829][Date:06-01-2025]-->
    public class OTMImportTemplateViewModel : NavigationViewModelBase, ITabViewModel, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
       IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");
        #endregion
        #region Declaration      
        private ObservableCollection<OtRequestTemplates> OtRequestTemplateslist;
        private OtRequestTemplates selectedOtRequestTemplates;
        private bool isDeleted;
        private bool isAddTemplatesButtunEnable=false;
        private bool isEditTemplatesButtunEnable = false;

        
        public string OTM_OTRequestTemplateGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "OTRequestTemplateGridSetting.Xml";
        private bool isPOrequestsColumnChooserVisible;
        public ObservableCollection<GridSummaryItem> TotalSummary { get; private set; }
        private string myFilterString;
        private ObservableCollection<TileBarFilters> _filterTypeListOfTile;
        private string userSettingsKey_OTRequestTemplate = "OTM_OTRequestTemplate_";
        private int selectedSearchIndex;
        #endregion
        #region Properties
        public virtual object ParentViewModel { get; set; }
        public virtual int Position { get; set; }
        public virtual string TabName { get; set; }
        public virtual object TabContent { get; protected set; }

        //[Rahul.Gadhave][For tab control][Date:24-02-2025]
        private ObservableCollection<POTemplate> potemplatelist;
        public ObservableCollection<POTemplate> POTemplateList
        {
            get { return potemplatelist; }
            set
            {
                potemplatelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("potemplatelist"));
            }
        }

        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == UI.Helper.TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public int SelectedSearchIndex
        {
            get { return selectedSearchIndex; }
            set
            {
                selectedSearchIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSearchIndex"));
            }
        }
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
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
        public ObservableCollection<TileBarFilters> FilterTypeListOfTiles
        {
            get { return _filterTypeListOfTile; }
            set
            {
                _filterTypeListOfTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterTypeListOfTiles"));
            }
        }
        public bool IsPOrequestsColumnChooserVisible
        {
            get { return isPOrequestsColumnChooserVisible; }
            set
            {
                isPOrequestsColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOrequestsColumnChooserVisible"));
            }
        }
        public bool IsAddTemplatesButtunEnable
        {
            get { return isAddTemplatesButtunEnable; }
            set
            {
                isAddTemplatesButtunEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddTemplatesButtunEnable"));
            }
        }
       
        public bool IsEditTemplatesButtunEnable
        {
            get { return isEditTemplatesButtunEnable; }
            set
            {
                isEditTemplatesButtunEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditTemplatesButtunEnable"));
            }
        }
        public ObservableCollection<OtRequestTemplates> OtRequestTemplatesList
        {
            get { return OtRequestTemplateslist; }
            set
            {
                OtRequestTemplateslist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtRequestTemplatesList"));
            }
        }
        public OtRequestTemplates SelectedOtRequestTemplates
        {
            get { return selectedOtRequestTemplates; }
            set
            {
                selectedOtRequestTemplates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOtRequestTemplates"));
            }
        }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        #endregion
        #region Public Events
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
        #region Public ICommand
        public ICommand AddNewOtRequestTemplates { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand DocumentViewCommand { get; set; }
        public ICommand DeleteTaskCommand { get; set; }
        public ICommand TableViewLoadedOTRequestTemplateCommand { get; set; }
        public ICommand CommandImportTemplateDoubleClick { get; set; }
        //public ICommand CloseButtonCommand { get; set; }
        #endregion
        #region Constructor
        public OTMImportTemplateViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor OTMImportTemplateViewModel()...", category: Category.Info, priority: Priority.Low);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintOtRequestTemplatesList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportOtRequestTemplatesList));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshOtRequestTemplatesList));
                DocumentViewCommand = new RelayCommand(new Action<object>(BrowseOtRequestTemplatesFile));
                TableViewLoadedOTRequestTemplateCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedOTRequestTemplateCommandAction);
                //[pramod.misal][GEOS2-6734][06-01-2025] 
                AddNewOtRequestTemplates = new RelayCommand(new Action<object>(AddNewOtRequestTemplatesAction));
                DeleteTaskCommand = new RelayCommand(new Action<object>(DeleteTaskCommandAction));
                //CloseButtonCommand = new DelegateCommand<object>(CloseButtonCommandAction);
                CommandImportTemplateDoubleClick = new DelegateCommand<object>(EditImportTemplate);
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 138))
                {
                    IsAddTemplatesButtunEnable = true;
                    IsEditTemplatesButtunEnable = true;
                }
                else
                {
                    IsEditTemplatesButtunEnable = false;
                }
                Init();
                GeosApplication.Instance.Logger.Log("Constructor OTMImportTemplateViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor OTMImportTemplateViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

                 //OTMService = new OTMServiceController("localhost:6699");
                //OtRequestTemplatesList = new ObservableCollection<OtRequestTemplates>(OTMService.GetAllOtImportTemplate_V2600());
                OtRequestTemplatesList = new ObservableCollection<OtRequestTemplates>(OTMService.GetAllOtImportTemplate_V2610());
                // [Rahul.Gadhave][GEOS2-6962][Date:25-02-2025]
                if (POTemplateList == null || !POTemplateList.Any())
                {
                    POTemplateList = new ObservableCollection<POTemplate>
                      {
                          new POTemplate()
                      };
                }
                string header = Application.Current.FindResource("PoTemplate").ToString();
                foreach (var item in POTemplateList)
                {
                    item.OtRequestTemplatesList = OtRequestTemplatesList;
                    item.Header = header;
                    item.SelectedOtRequestTemplates = item.OtRequestTemplatesList.FirstOrDefault();
                    SelectedOtRequestTemplates = item.SelectedOtRequestTemplates;
                }
                
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        /// <summary>
        /// Method to Open PDF in another window
        /// </summary>
        /// <param name="obj"></param>
        private void BrowseOtRequestTemplatesFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method BrowseOtRequestTemplatesFile()...", category: Category.Info, priority: Priority.Low);
                //  OtRequestTemplates Temp = (OtRequestTemplates)obj;
                DocumentView employeeDocumentView = new DocumentView();
                DocumentViewModel employeeDocumentViewModel = new DocumentViewModel();
                // string EmployeeCode = string.Empty;
                if (POTemplateList[0].SelectedOtRequestTemplates.FileDocInBytes!=null)
                {
                    employeeDocumentViewModel.OpenPdfByTemplateCode(POTemplateList[0].SelectedOtRequestTemplates, obj);
                    if (employeeDocumentViewModel.IsPresent)
                    {
                        employeeDocumentView.DataContext = employeeDocumentViewModel;
                        employeeDocumentView.Show();
                    }

                    GeosApplication.Instance.Logger.Log("Method BrowseOtRequestTemplatesFile()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    CustomMessageBox.Show(string.Format("Could not find file {0}", SelectedOtRequestTemplates.File), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //throw;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in BrowseOtRequestTemplatesFile() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in BrowseOtRequestTemplatesFile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method BrowseOtRequestTemplatesFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Print Ot Request Templates List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintOtRequestTemplatesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintOtRequestTemplatesList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["JobDescriptionReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["JobDescriptionReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.BPlus;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintOtRequestTemplatesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintOtRequestTemplatesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Export Ot Request Templates List
        /// </summary>
        /// <param name="obj"></param>
        private void ExportOtRequestTemplatesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportOtRequestTemplatesList ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "PORequestTemplates";
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
                    TableView JobDescriptionsTableView = ((TableView)obj);
                    JobDescriptionsTableView.ShowTotalSummary = false;
                    JobDescriptionsTableView.ShowFixedTotalSummary = false;
                    JobDescriptionsTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    JobDescriptionsTableView.ShowTotalSummary = false;
                    JobDescriptionsTableView.ShowFixedTotalSummary = true;
                }

                GeosApplication.Instance.Logger.Log("Method ExportOtRequestTemplatesList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportOtRequestTemplatesList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method Refresh Ot Request Templates List
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshOtRequestTemplatesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshOtRequestTemplatesList()...", category: Category.Info, priority: Priority.Low);
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
            //   OTMService = new OTMServiceController("localhost:6699");
                //OtRequestTemplatesList = new ObservableCollection<OtRequestTemplates>(OTMService.GetAllOtImportTemplate_V2600());
                OtRequestTemplatesList = new ObservableCollection<OtRequestTemplates>(OTMService.GetAllOtImportTemplate_V2610());
                if (POTemplateList == null || !POTemplateList.Any())
                {
                    POTemplateList = new ObservableCollection<POTemplate>
                      {
                          new POTemplate()
                      };
                }
                string header = Application.Current.FindResource("PoTemplate").ToString();
                foreach (var item in POTemplateList)
                {
                    item.OtRequestTemplatesList = OtRequestTemplatesList;
                    item.Header = header;
                    item.SelectedOtRequestTemplates = item.OtRequestTemplatesList.FirstOrDefault();
                    SelectedOtRequestTemplates = item.SelectedOtRequestTemplates;
                }

                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshOtRequestTemplatesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllOtImportTemplate_V2600() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllOtImportTemplate_V2600() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshOtRequestTemplatesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Add New Ot RequestTemplatesAction
        /// //[pramod.misal][GEOS2-6734][06-01-2025] 
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewOtRequestTemplatesAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewOtRequestTemplatesAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                TableView detailView = (TableView)obj;
                AddEditNewOtTemplateView addEditNewOtTemplateView = new AddEditNewOtTemplateView();
                AddEditNewOtTemplateViewModel addEditNewOtTemplateViewModel = new AddEditNewOtTemplateViewModel();
                EventHandler handle = delegate { addEditNewOtTemplateView.Close(); };
                addEditNewOtTemplateViewModel.RequestClose += handle;
                addEditNewOtTemplateView.DataContext = addEditNewOtTemplateViewModel;
                addEditNewOtTemplateViewModel.Init();
                addEditNewOtTemplateViewModel.WindowHeader = System.Windows.Application.Current.FindResource("NewOtTemplateTitle").ToString();
                addEditNewOtTemplateViewModel.IsNew = true;
                var ownerInfo = (detailView as FrameworkElement);
                addEditNewOtTemplateView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditNewOtTemplateView.ShowDialog();
                //[pooja.jadhav][23-01-2025][GEOS2-6734]
                if (addEditNewOtTemplateViewModel.Template != null)
                {
                    if (addEditNewOtTemplateViewModel.Template.InUse == 1)
                    {
                        addEditNewOtTemplateViewModel.Template.Action = "Yes";
                    }
                    // [Rahul.Gadhave][GEOS2-6962][Date:25-02-2025]
                    POTemplateList[0].OtRequestTemplatesList.Add(addEditNewOtTemplateViewModel.Template);
                    POTemplateList[0].SelectedOtRequestTemplates = addEditNewOtTemplateViewModel.Template;

                    //OtRequestTemplatesList.Add(addEditNewOtTemplateViewModel.Template);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewOtRequestTemplatesAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewOtRequestTemplatesAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void DeleteTaskCommandAction(object obj)
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteTaskDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    OtRequestTemplates Temp = (OtRequestTemplates)obj;
                    IsDeleted = OTMService.DeletetImportTemplate_V2600(Temp.IdOTRequestTemplate);

                    var parentItem = OtRequestTemplatesList.FirstOrDefault(j => j.IdOTRequestTemplate == Temp.IdOTRequestTemplate);
                    if (parentItem != null && IsDeleted==true)
                    {

                        parentItem.InUse = 0;
                        parentItem.Action = "No";
                        SelectedOtRequestTemplates = null;
                        if (IsDeleted)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OTRequestTemplateDeletedsuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                             OtRequestTemplatesList = new ObservableCollection<OtRequestTemplates>(OtRequestTemplatesList);
                        }
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DeleteTaskCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteTaskCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteTaskCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteTaskCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void TableViewLoadedOTRequestTemplateCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedPORegisteredCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(OTM_OTRequestTemplateGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(OTM_OTRequestTemplateGridSettingFilePath);
                    try
                    {
                        // Get the TableView and GridControl from the event source
                        TableView gridTableView = obj.OriginalSource as TableView;
                        GridControl tempgridControl = gridTableView.DataControl as GridControl;
                        // Manually parse the XML to enforce visibility settings
                        ApplyPORegisteredVisibilityFromXml(tempgridControl);
                    }
                    catch (Exception ex)
                    {

                    }

                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(OTM_OTRequestTemplateGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChangedPORegistered);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexPORegisteredChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsPOrequestsColumnChooserVisible = true;
                }
                else
                {
                    IsPOrequestsColumnChooserVisible = false;
                }

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.ShowTotalSummary = true;
                //Total Summary
                TotalSummary = new ObservableCollection<GridSummaryItem>();
                TotalSummary.Add(new GridSummaryItem() { SummaryType = SummaryItemType.Sum, FieldName = "Amount", DisplayFormat = " {0:C}" });
                gridControl.TotalSummary.Clear();

                gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                new GridSummaryItem()
                {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}",
                }
                });
                if (TotalSummary.Count > 0)
                {
                    foreach (var sum in TotalSummary)
                    {
                        gridControl.TotalSummary.Add(sum);
                    }
                }
                //if (OtRequestTemplatesList[SelectedSearchIndex].Header == OTMCommon.Instance.PORequestsTitle)
                //{
                //    OtRequestTemplatesList[SelectedSearchIndex].ISVisiblePOReq = Visibility.Visible;
                //}
                //else
                //{
                //    OtRequestTemplatesList[SelectedSearchIndex].ISVisiblePOReq = Visibility.Collapsed;
                //}
                //AddPORegisteredCustomSettingCount(gridControl);
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedPORegisteredCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedPORegisteredCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void ApplyPORegisteredVisibilityFromXml(GridControl gridControl)
        {
            try
            {
                XDocument layoutXml = XDocument.Load(OTM_OTRequestTemplateGridSettingFilePath);

                foreach (GridColumn column in gridControl.Columns)
                {
                    // Find the column entry in the XML based on its FieldName
                    var columnElement = layoutXml.Descendants("property")
                        .Where(x => (string)x.Attribute("name") == "FieldName" && x.Value == column.FieldName)
                        .FirstOrDefault();

                    if (columnElement != null)
                    {
                        // Get the visibility setting from the XML
                        var visibleElement = columnElement.Parent.Descendants("property")
                            .Where(x => (string)x.Attribute("name") == "Visible")
                            .FirstOrDefault();

                        if (visibleElement != null)
                        {
                            bool isVisible = bool.Parse(visibleElement.Value);
                            column.Visible = isVisible;
                            GeosApplication.Instance.Logger.Log($"Column '{column.FieldName}' visibility set to: {isVisible}.", category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ApplyVisibilityFromXml: " + ex.Message, category: Category.Exception, priority: Priority.High);
            }
        }
        void VisibleChangedPORegistered(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChangedPORegistered ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    var view = column.View as TableView;
                    GridControl gridControl = view.DataControl as GridControl;
                    gridControl.SaveLayoutToXml(OTM_OTRequestTemplateGridSettingFilePath);
                    ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_OTRequestTemplateGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    //  IsWorkOrderColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChangedPORegistered() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChangedPORegistered() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibleIndexPORegisteredChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexPORegisteredChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                var view = column.View as TableView;
                GridControl gridControl = view.DataControl as GridControl;
                gridControl.SaveLayoutToXml(OTM_OTRequestTemplateGridSettingFilePath);
                ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_OTRequestTemplateGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexPORegisteredChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexPORegisteredChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditImportTemplate(object obj)
        {
            try
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 138))
                {

                    GeosApplication.Instance.Logger.Log("Method EditImportTemplate...", category: Category.Info, priority: Priority.Low);

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

                    if ((OtRequestTemplates)detailView.DataControl.CurrentItem != null)
                    {
                        int IdOtRequestTemplate = Convert.ToInt32(((OtRequestTemplates)detailView.DataControl.CurrentItem).IdOtRequestTemplate);
                        AddEditNewOtTemplateViewModel addEditNewOtTemplateViewModel = new AddEditNewOtTemplateViewModel();
                        AddEditNewOtTemplateView addEditNewOtTemplateView = new AddEditNewOtTemplateView();

                        addEditNewOtTemplateViewModel.EditInit((OtRequestTemplates)detailView.DataControl.CurrentItem);
                        EventHandler handle = delegate { addEditNewOtTemplateView.Close(); };
                        addEditNewOtTemplateViewModel.RequestClose += handle;
                        addEditNewOtTemplateView.DataContext = addEditNewOtTemplateViewModel;

                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        var ownerInfo = (detailView as FrameworkElement);
                        addEditNewOtTemplateViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditOtTemplateTitle").ToString();
                        addEditNewOtTemplateViewModel.IsNew = false;
                        addEditNewOtTemplateViewModel.TempReadOnly = true;
                        addEditNewOtTemplateViewModel.GroupIsEnabled = false;
                        addEditNewOtTemplateView.ShowDialogWindow();
                        if (addEditNewOtTemplateViewModel.Template != null)
                        {
                            // [Rahul.Gadhave][GEOS2-6962][Date:25-02-2025]
                            var selectedTemplate = POTemplateList[0].SelectedOtRequestTemplates;
                            if (selectedTemplate != null)
                            {
                                selectedTemplate.Code = addEditNewOtTemplateViewModel.Template.Code;
                                selectedTemplate.TemplateName = addEditNewOtTemplateViewModel.Template.TemplateName;
                                selectedTemplate.Group = addEditNewOtTemplateViewModel.Template.Group;
                                selectedTemplate.Region = addEditNewOtTemplateViewModel.Template.Region;
                                selectedTemplate.Country = addEditNewOtTemplateViewModel.Template.Country;
                                selectedTemplate.Plant = addEditNewOtTemplateViewModel.Template.Plant;
                                selectedTemplate.UpdatedBy = addEditNewOtTemplateViewModel.Template.UpdatedBy;
                                selectedTemplate.UpdatedAt = addEditNewOtTemplateViewModel.Template.UpdatedAt;
                                selectedTemplate.InUse = addEditNewOtTemplateViewModel.Template.InUse;
                                selectedTemplate.File = addEditNewOtTemplateViewModel.Template.File;
                                selectedTemplate.Action = addEditNewOtTemplateViewModel.Template.Action;
                                selectedTemplate.fileExtension = addEditNewOtTemplateViewModel.FileExtension;
                                selectedTemplate.FileDocInBytes = addEditNewOtTemplateViewModel.Template.FileDocInBytes;

                                if (addEditNewOtTemplateViewModel.Template.InUse == 1)
                                {
                                    selectedTemplate.Action = "Yes";
                                }
                            }
                        }

                        GeosApplication.Instance.Logger.Log("Method EditImportTemplate() executed successfully...", category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditImportTemplate() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region Validation
        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }
        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                return null;
            }
        }
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                return null;
            }
              
        }
        public void Dispose()
        {

        }
        #endregion
    }
}
