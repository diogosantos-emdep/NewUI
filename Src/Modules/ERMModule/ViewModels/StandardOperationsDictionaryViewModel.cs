using DevExpress.Data.Filtering;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Utils.CommonDialogs.Internal;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.Views;
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
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    class StandardOperationsDictionaryViewModel: ViewModelBase, INotifyPropertyChanged
    {
        #region Service
         private INavigationService Service { get { return this.GetService<INavigationService>(); } }
		// shubham[skadam] GEOS2-3243 Add Standard Operations Dictionary (Information) (#ERM07) [1/2]
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IERMService ERMService = new ERMServiceController("localhost:6699");
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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
        private ObservableCollection<StandardOperationsDictionary> standardOperationsDictionaryList;
        private StandardOperationsDictionary selectedStandardOperation;
        private StandardOperationsDictionary selectedStandardOperationsDictionaryList;
        private bool isViewStandardOperationsDictionaryColumnChooserVisible;
        private bool isDelete;
        private bool isBusy;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        public string StandardOperationsDictionaryGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "StandardOperationsDictionaryGridSettingFilePath.Xml";
        private ObservableCollection<Site> plantList;
        #endregion

        #region Properties
        public ObservableCollection<StandardOperationsDictionary> StandardOperationsDictionaryList
        {
            get { return standardOperationsDictionaryList; }
            set
            {
                standardOperationsDictionaryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StandardOperationsDictionaryList"));
            }
        }

        public StandardOperationsDictionary SelectedStandardOperation
        {
            get
            {
                return selectedStandardOperation;
            }
            set
            {
                selectedStandardOperation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStandardOperation"));
            }
        }

        public bool IsDeleted
        {
            get
            {
                return isDelete;
            }

            set
            {
                isDelete = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
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
        public StandardOperationsDictionary SelectedStandardOperationsDictionaryList
        {
            get {
                return selectedStandardOperationsDictionaryList;
            }
            set
            {
                selectedStandardOperationsDictionaryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStandardOperationsDictionaryList"));
            }

        }
        public bool IsViewStandardOperationsDictionaryColumnChooserVisible
        {
            get
            {
                return isViewStandardOperationsDictionaryColumnChooserVisible;
            }

            set
            {
                isViewStandardOperationsDictionaryColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsViewStandardOperationsDictionaryColumnChooserVisible"));
            }
        }
		// shubham[skadam] GEOS2-3243 Add Standard Operations Dictionary (Information) (#ERM07) [1/2]
        public ObservableCollection<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
            }
        }
        #endregion

        #region ICommands

        public ICommand EditSODCommand { get; set; }

        public ICommand DeleteStandardOperationsDictionaryCommand { get; set; }
        public ICommand PrintStandardOperationsDictionaryListCommand { get; set; }
        public ICommand ExportStandardOperationsDictionaryListCommand { get; set; }
        public ICommand RefreshStandardOperationsDictionaryListCommand { get; set; }
        public ICommand AddStandardOperationsDictionaryListCommand { get; set; }
        public ICommand EditStandardOperationsDictionaryCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }

        public ICommand STLGridControlLoadedCommand { get; set; }
        public ICommand StandardOperationsDictionaryTableViewLoadedCommand { get; set; }
        public ICommand STLGridControlUnloadedCommand { get; set; }
        public ICommand StandardOperationsDictionaryListTableViewLoadedCommand { get; set; }

        #endregion

        #region Constructor

        public StandardOperationsDictionaryViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor StandardOperationsDictionaryViewModel ...", category: Category.Info, priority: Priority.Low);

                EditSODCommand = new RelayCommand(new Action<object>(EditSODAction));
                DeleteStandardOperationsDictionaryCommand = new RelayCommand(new Action<object>(DeleteStandardOperationsDictionaryAction));
                RefreshStandardOperationsDictionaryListCommand = new RelayCommand(new Action<object>(RefreshStandardOperationsDictionaryListCommandAction));
                  PrintStandardOperationsDictionaryListCommand = new RelayCommand(new Action<object>(PrintStandardOperationsDictionaryListCommandAction));
                  ExportStandardOperationsDictionaryListCommand = new RelayCommand(new Action<object>(ExportStandardOperationsDictionaryListCommandAction));
                  CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupAction);
                  StandardOperationsDictionaryListTableViewLoadedCommand=new DelegateCommand<RoutedEventArgs>(StandardOperationsDictionaryListTableViewLoadedCommandAction);
                AddStandardOperationsDictionaryListCommand = new RelayCommand(new Action<object>(AddStandardOperationsDictionaryListCommandAction));

                IsViewStandardOperationsDictionaryColumnChooserVisible = true;
                GeosApplication.Instance.Logger.Log("Constructor StandardOperationsDictionaryViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor StandardOperationsDictionaryViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        public void Init()
        {
            FillStandardOperationsDictionaryGrid();
        }
        

        private void EditSODAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSODAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;
                
                AddEditStandardOperationsDictionaryView addEditStandardOperationsDictionaryView = new AddEditStandardOperationsDictionaryView();
                AddEditStandardOperationsDictionaryViewModel addEditStandardOperationsDictionaryViewModel = new AddEditStandardOperationsDictionaryViewModel();

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                addEditStandardOperationsDictionaryView.DataContext = addEditStandardOperationsDictionaryViewModel;
                EventHandler handle = delegate { addEditStandardOperationsDictionaryView.Close(); };
                addEditStandardOperationsDictionaryViewModel.RequestClose += handle;

                addEditStandardOperationsDictionaryViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditStandardOperationsDictionary").ToString();

                addEditStandardOperationsDictionaryViewModel.IsNew = false;
                addEditStandardOperationsDictionaryViewModel.IsFirstTimeLoad = true;
                addEditStandardOperationsDictionaryViewModel.SaveButtonVisibility = Visibility.Visible;
                addEditStandardOperationsDictionaryViewModel.EditInit(System.Windows.Application.Current.FindResource("EditStandardOperationsDictionary").ToString());
                addEditStandardOperationsDictionaryViewModel.EditInitSOD(SelectedStandardOperation);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                TableView detailView = (TableView)obj;
                var ownerInfo = (detailView as FrameworkElement);
                addEditStandardOperationsDictionaryView.Owner = Window.GetWindow(ownerInfo);
                addEditStandardOperationsDictionaryView.ShowDialog();

                if (addEditStandardOperationsDictionaryViewModel.IsUpdatedSODSave)
                {
                    var index = standardOperationsDictionaryList.IndexOf(SelectedStandardOperation);

                    standardOperationsDictionaryList[index] = addEditStandardOperationsDictionaryViewModel.UpdatedSOD;
                    SelectedStandardOperation = standardOperationsDictionaryList[index];
					// shubham[skadam] GEOS2-3243 Add Standard Operations Dictionary (Information) (#ERM07) [1/2]
                    //PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                    List<string> result = SelectedStandardOperation.Plants.Split('\n').ToList();
                    if (result.Count == PlantList.Count())
                    {
                        SelectedStandardOperation.Plants = "ALL";
                    }
                }

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Method EditSODAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditSODAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        public void FillStandardOperationsDictionaryGrid()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                //StandardOperationsDictionaryList = new ObservableCollection<StandardTime>(ERMService.GetStandardTimeList());
                StandardOperationsDictionaryList = new ObservableCollection<StandardOperationsDictionary>(ERMService.GetStandardOperationsDictionaryList_V2260());
                // shubham[skadam] GEOS2-3243 Add Standard Operations Dictionary (Information) (#ERM07) [1/2]
				PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                foreach (var item in StandardOperationsDictionaryList)
                {
                    List<string> result = item.Plants.Split('\n').ToList();
                    if (result.Count== PlantList.Count())
                    {
                        item.Plants = "ALL";
                    }
                }
               
                if (StandardOperationsDictionaryList.Count > 0)
                    SelectedStandardOperation = StandardOperationsDictionaryList.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStandardOperationsDictionaryGrid() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStandardOperationsDictionaryGrid() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillStandardOperationsDictionaryGrid()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteStandardOperationsDictionaryAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteStandardOperationsDictionaryAction()...", category: Category.Info, priority: Priority.Low);
              	// shubham[skadam] GEOS2-3243 Add Standard Operations Dictionary (Information) (#ERM07) [1/2]
                if (SelectedStandardOperationsDictionaryList.IdStatus == 1535)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveStandardOperationGridValidationForActive").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                else if (SelectedStandardOperationsDictionaryList.IdStatus == 1536)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveStandardOperationGridValidationForInactive").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["StandardOperationDeleteMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                      //  IsDeleted = ERMService.IsDeletedStandardTimeList(SelectedStandardOperationsDictionaryList.IdStandardOperationsDictionary, (uint)GeosApplication.Instance.ActiveUser.IdUser);

                        IsDeleted = ERMService.DeleteOperationFromStandardOperationsDictionary_V2260(SelectedStandardOperationsDictionaryList.IdStandardOperationsDictionary, (uint)GeosApplication.Instance.ActiveUser.IdUser);

                        if (IsDeleted)
                        {
                            StandardOperationsDictionaryList.Remove(SelectedStandardOperationsDictionaryList);
                            StandardOperationsDictionaryList = new ObservableCollection<StandardOperationsDictionary>(StandardOperationsDictionaryList);
                            SelectedStandardOperationsDictionaryList = StandardOperationsDictionaryList.FirstOrDefault();
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("StandardOperationDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteStandardOperationsDictionaryAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteStandardOperationsDictionaryAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteStandardOperationsDictionaryAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteStandardOperationsDictionaryAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddStandardOperationsDictionaryListCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddStandardOperationsDictionaryListCommandAction()", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                AddEditStandardOperationsDictionaryView addEditStandardOperationsDictionaryView = new AddEditStandardOperationsDictionaryView();
                AddEditStandardOperationsDictionaryViewModel addEditStandardOperationsDictionaryViewModel = new AddEditStandardOperationsDictionaryViewModel();

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                addEditStandardOperationsDictionaryView.DataContext = addEditStandardOperationsDictionaryViewModel;
                EventHandler handle = delegate { addEditStandardOperationsDictionaryView.Close(); };
                addEditStandardOperationsDictionaryViewModel.RequestClose += handle;

                addEditStandardOperationsDictionaryViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddStandardOperationsDictionary").ToString();
                
                addEditStandardOperationsDictionaryViewModel.IsNew = true;
                addEditStandardOperationsDictionaryViewModel.IsFirstTimeLoad = true;
                addEditStandardOperationsDictionaryViewModel.SaveButtonVisibility = Visibility.Visible;
                addEditStandardOperationsDictionaryViewModel.Init(System.Windows.Application.Current.FindResource("AddStandardOperationsDictionary").ToString());
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                TableView detailView = (TableView)obj;
                var ownerInfo = (detailView as FrameworkElement);
                addEditStandardOperationsDictionaryView.Owner = Window.GetWindow(ownerInfo);
                addEditStandardOperationsDictionaryView.ShowDialog();

                if(addEditStandardOperationsDictionaryViewModel.IsNewSODSave)
                {
                    StandardOperationsDictionary newSODClone = (StandardOperationsDictionary)addEditStandardOperationsDictionaryViewModel.NewSOD.Clone();
                    StandardOperationsDictionaryList.Add(newSODClone);
                    StandardOperationsDictionaryList = new ObservableCollection<StandardOperationsDictionary>(
                        StandardOperationsDictionaryList.OrderByDescending(
                        a => a.Code));
                    SelectedStandardOperation = StandardOperationsDictionaryList.FirstOrDefault();
					// shubham[skadam] GEOS2-3243 Add Standard Operations Dictionary (Information) (#ERM07) [1/2]
                    List<string> result = newSODClone.Plants.Split('\n').ToList();
                    if (result.Count == PlantList.Count())
                    {
                        //SelectedStandardOperation.Plants = "ALL";
                        newSODClone.Plants = "ALL";
                    }
                }
                
                GeosApplication.Instance.Logger.Log("Method AddStandardOperationsDictionaryListCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddStandardOperationsDictionaryListCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void RefreshStandardOperationsDictionaryListCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshStandardOperationsDictionaryListCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillStandardOperationsDictionaryGrid();

                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in gridControl.Columns)
                {
                    if (column.Visible == false)
                        visibleFalseCoulumn++;
                }

                if (visibleFalseCoulumn > 0)
                    IsViewStandardOperationsDictionaryColumnChooserVisible = true;
                else
                    IsViewStandardOperationsDictionaryColumnChooserVisible = false;

                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshStandardOperationsDictionaryListCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshStandardOperationsDictionaryListCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshStandardOperationsDictionaryListCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshStandardOperationsDictionaryListCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintStandardOperationsDictionaryListCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintStandardOperationsDictionaryListCommandAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["StandardOperationsDictionaryListPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["StandardOperationsDictionaryListPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintStandardOperationsDictionaryListCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintStandardOperationsDictionaryListCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportStandardOperationsDictionaryListCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportStandardOperationsDictionaryListCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Standard Operations Dictionary";
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
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportStandardOperationsDictionaryListCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportStandardOperationsDictionaryListCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        private void CustomShowFilterPopupAction(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "Plants")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();
               if (e.Column.FieldName == "Plants")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Plants])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Plants])")
                    });

                    foreach (var dataObject in standardOperationsDictionaryList)
                    {
                        if (dataObject.Plants == null)
                        {
                            continue;
                        }
                        else if (dataObject.Plants != null)
                        {
                            if (dataObject.Plants.Contains("\n"))
                            {
                                string tempPlants = dataObject.Plants;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plants Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == standardOperationsDictionaryList.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = standardOperationsDictionaryList.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plants Like '%{0}%'", standardOperationsDictionaryList.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }



                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void StandardOperationsDictionaryListTableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method StandardOperationsDictionaryListTableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                if (File.Exists(StandardOperationsDictionaryGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(StandardOperationsDictionaryGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)GridControlSTDetails.View;

                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(StandardOperationsDictionaryGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsViewStandardOperationsDictionaryColumnChooserVisible = true;
                }
                else
                {
                    IsViewStandardOperationsDictionaryColumnChooserVisible = false;
                }

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);

               // datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method StandardOperationsDictionaryListTableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in StandardOperationsDictionaryListTableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(StandardOperationsDictionaryGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    isViewStandardOperationsDictionaryColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(StandardOperationsDictionaryGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.DependencyProperty == TreeListControl.FilterStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

    }
}
