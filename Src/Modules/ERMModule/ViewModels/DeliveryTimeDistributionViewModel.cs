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
    class DeliveryTimeDistributionViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
      //  IERMService ERMService = new ERMServiceController("localhost:6699");
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
        private ObservableCollection<DeliveryTimeDistribution> deliveryTimeDistributionList;
        private DeliveryTimeDistribution selectedDeliveryTimeDistribution;
        private DeliveryTimeDistribution selectedDeliveryTimeDistributionList;
        private bool isViewDeliveryTimeDistributionColumnChooserVisible;
        private bool isDelete;
        private bool isBusy;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        public string DeliveryTimeDistributionGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "DeliveryTimeDistributionGridSettingFilePath.Xml";
        private ObservableCollection<Site> plantList;
        #endregion

        #region Properties
        public ObservableCollection<DeliveryTimeDistribution> DeliveryTimeDistributionList
        {
            get { return deliveryTimeDistributionList; }
            set
            {
                deliveryTimeDistributionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeliveryTimeDistributionList"));
            }
        }

        public DeliveryTimeDistribution SelectedDeliveryTimeDistribution
        {
            get
            {
                return selectedDeliveryTimeDistribution;
            }
            set
            {
                selectedDeliveryTimeDistribution = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDeliveryTimeDistribution"));
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
        public DeliveryTimeDistribution SelectedDeliveryTimeDistributionList
        {
            get
            {
                return selectedDeliveryTimeDistributionList;
            }
            set
            {
                selectedDeliveryTimeDistributionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDeliveryTimeDistributionList"));
            }

        }
        public bool IsViewDeliveryTimeDistributionColumnChooserVisible
        {
            get
            {
                return isViewDeliveryTimeDistributionColumnChooserVisible;
            }

            set
            {
                isViewDeliveryTimeDistributionColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsViewDeliveryTimeDistributionColumnChooserVisible"));
            }
        }
     
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

        public ICommand EditDTDCommand { get; set; }

        public ICommand DeleteDeliveryTimeDistributionCommand { get; set; }
        public ICommand PrintDeliveryTimeDistributionCommand { get; set; }
        public ICommand ExportDeliveryTimeDistributionCommand { get; set; }
        public ICommand RefreshDeliveryTimeDistributionCommand { get; set; }
        public ICommand AddDeliveryTimeDistributionCommand { get; set; }
    
        public ICommand DeliveryTimeDistributionTableViewLoadedCommand { get; set; }

        public ICommand CustomShowFilterPopupCommand { get; set; }


        #endregion

        #region Constructor

        public DeliveryTimeDistributionViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DeliveryTimeDistributionViewModel ...", category: Category.Info, priority: Priority.Low);

                EditDTDCommand = new RelayCommand(new Action<object>(EditDTDAction));
                DeleteDeliveryTimeDistributionCommand = new RelayCommand(new Action<object>(DeleteDeliveryTimeDistributionCommandAction));
                RefreshDeliveryTimeDistributionCommand = new RelayCommand(new Action<object>(RefreshDeliveryTimeDistributionCommandAction));
                PrintDeliveryTimeDistributionCommand = new RelayCommand(new Action<object>(PrintDeliveryTimeDistributionCommandAction));
                ExportDeliveryTimeDistributionCommand = new RelayCommand(new Action<object>(ExportDeliveryTimeDistributionCommandAction));
                DeliveryTimeDistributionTableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(DeliveryTimeDistributionTableViewLoadedCommandAction);
                AddDeliveryTimeDistributionCommand = new RelayCommand(new Action<object>(AddDeliveryTimeDistributionCommandAction));
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupAction);
                IsViewDeliveryTimeDistributionColumnChooserVisible = true;
                GeosApplication.Instance.Logger.Log("Constructor DeliveryTimeDistributionViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor DeliveryTimeDistributionViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        public void Init()
        {
             FillDeliveryTimeDistributionGrid();
        }


        private void EditDTDAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSODAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;

                AddEditDeliveryTimeDistributionView addEditDeliveryTimeDistributionView = new AddEditDeliveryTimeDistributionView();
                AddEditDeliveryTimeDistributionViewModel addEditDeliveryTimeDistributionViewModel = new AddEditDeliveryTimeDistributionViewModel();

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                addEditDeliveryTimeDistributionView.DataContext = addEditDeliveryTimeDistributionViewModel;
                EventHandler handle = delegate { addEditDeliveryTimeDistributionView.Close(); };
                addEditDeliveryTimeDistributionViewModel.RequestClose += handle;

                addEditDeliveryTimeDistributionViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditDeliveryTimeDistribution").ToString();

                addEditDeliveryTimeDistributionViewModel.IsNew = false;
                addEditDeliveryTimeDistributionViewModel.IsFirstTimeLoad = true;
                addEditDeliveryTimeDistributionViewModel.SaveButtonVisibility = Visibility.Visible;
                addEditDeliveryTimeDistributionViewModel.EditInit(System.Windows.Application.Current.FindResource("EditDeliveryTimeDistribution").ToString());
                addEditDeliveryTimeDistributionViewModel.EditInitDTD(SelectedDeliveryTimeDistribution);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                TableView detailView = (TableView)obj;
                var ownerInfo = (detailView as FrameworkElement);
                addEditDeliveryTimeDistributionView.Owner = Window.GetWindow(ownerInfo);
                addEditDeliveryTimeDistributionView.ShowDialog();

                if (addEditDeliveryTimeDistributionViewModel.IsUpdatedDTDSave)
                {
                    var index = DeliveryTimeDistributionList.IndexOf(SelectedDeliveryTimeDistribution);

                    DeliveryTimeDistributionList[index] = addEditDeliveryTimeDistributionViewModel.UpdatedDTD;

                    SelectedDeliveryTimeDistribution = deliveryTimeDistributionList[index];

                    List<string> result = SelectedDeliveryTimeDistribution.Plants.Split('\n').ToList();
                    if (result.Count == PlantList.Count())
                    {
                        SelectedDeliveryTimeDistribution.Plants = "ALL";
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



        public void FillDeliveryTimeDistributionGrid()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

               // IERMService ERMService = new ERMServiceController("localhost:6699");
                DeliveryTimeDistributionList = new ObservableCollection<DeliveryTimeDistribution>(ERMService.GetDeliveryTimeDistributionList_V2480());
            
                PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                foreach (var item in DeliveryTimeDistributionList)
                {
                    List<string> result = item.Plants.Split('\n').ToList();
                    if (result.Count == PlantList.Count())
                    {
                        item.Plants = "ALL";
                    }
                }

                if (DeliveryTimeDistributionList.Count > 0)
                    SelectedDeliveryTimeDistribution = DeliveryTimeDistributionList.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDeliveryTimeDistributionGrid() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDeliveryTimeDistributionGrid() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillDeliveryTimeDistributionGrid()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteDeliveryTimeDistributionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteDeliveryTimeDistributionCommandAction()...", category: Category.Info, priority: Priority.Low);
          
                if (SelectedDeliveryTimeDistributionList.IdStatus == 1535)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveDeliveryTimeDistributionGridValidationForActive").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                else if (SelectedDeliveryTimeDistributionList.IdStatus == 1536)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveDeliveryTimeDistributionGridValidationForInactive").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeliveryTimeDistributionDeleteMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {

                       IsDeleted = ERMService.DeleteOperationFromDeliveryTimeDistribution_V2480(SelectedDeliveryTimeDistributionList.Iddeliverytimedistribution, (uint)GeosApplication.Instance.ActiveUser.IdUser);

                        if (IsDeleted)
                        {
                            DeliveryTimeDistributionList.Remove(SelectedDeliveryTimeDistributionList);
                            DeliveryTimeDistributionList = new ObservableCollection<DeliveryTimeDistribution>(DeliveryTimeDistributionList);
                            SelectedDeliveryTimeDistributionList = DeliveryTimeDistributionList.FirstOrDefault();
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeliveryTimeDistributionDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteDeliveryTimeDistributionCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDeliveryTimeDistributionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDeliveryTimeDistributionCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteDeliveryTimeDistributionCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddDeliveryTimeDistributionCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddDeliveryTimeDistributionCommandAction()", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                AddEditDeliveryTimeDistributionView addEditDeliveryTimeDistributionView = new AddEditDeliveryTimeDistributionView();
                AddEditDeliveryTimeDistributionViewModel addEditDeliveryTimeDistributionViewModel = new AddEditDeliveryTimeDistributionViewModel();

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                addEditDeliveryTimeDistributionView.DataContext = addEditDeliveryTimeDistributionViewModel;
                EventHandler handle = delegate { addEditDeliveryTimeDistributionView.Close(); };
                addEditDeliveryTimeDistributionViewModel.RequestClose += handle;

                addEditDeliveryTimeDistributionViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddDeliveryTimeDistribution").ToString();

                addEditDeliveryTimeDistributionViewModel.IsNew = true;
                addEditDeliveryTimeDistributionViewModel.IsFirstTimeLoad = true;
                addEditDeliveryTimeDistributionViewModel.SaveButtonVisibility = Visibility.Visible;
                addEditDeliveryTimeDistributionViewModel.Init(System.Windows.Application.Current.FindResource("AddDeliveryTimeDistribution").ToString());
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                TableView detailView = (TableView)obj;
                var ownerInfo = (detailView as FrameworkElement);
                addEditDeliveryTimeDistributionView.Owner = Window.GetWindow(ownerInfo);
                addEditDeliveryTimeDistributionView.ShowDialog();

                if (addEditDeliveryTimeDistributionViewModel.IsNewDTDSave)
                {
                    DeliveryTimeDistribution newSODClone = (DeliveryTimeDistribution)addEditDeliveryTimeDistributionViewModel.NewDTD.Clone();
                    DeliveryTimeDistributionList.Add(newSODClone);
                    DeliveryTimeDistributionList = new ObservableCollection<DeliveryTimeDistribution>(
                        DeliveryTimeDistributionList.OrderByDescending(
                        a => a.Code));
                    SelectedDeliveryTimeDistribution = DeliveryTimeDistributionList.FirstOrDefault();
                    List<string> result = newSODClone.Plants.Split('\n').ToList();
                    if (result.Count == PlantList.Count())
                    {
                        newSODClone.Plants = "ALL";
                    }
                }


                GeosApplication.Instance.Logger.Log("Method AddDeliveryTimeDistributionCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddDeliveryTimeDistributionCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        private void RefreshDeliveryTimeDistributionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDeliveryTimeDistributionCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillDeliveryTimeDistributionGrid();

                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in gridControl.Columns)
                {
                    if (column.Visible == false)
                        visibleFalseCoulumn++;
                }

                if (visibleFalseCoulumn > 0)
                    IsViewDeliveryTimeDistributionColumnChooserVisible = true;
                else
                    IsViewDeliveryTimeDistributionColumnChooserVisible = false;

                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshDeliveryTimeDistributionCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDeliveryTimeDistributionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDeliveryTimeDistributionCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshDeliveryTimeDistributionCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintDeliveryTimeDistributionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintDeliveryTimeDistributionCommandAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["DeliveryTimeDistributionListPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["DeliveryTimeDistributionListPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintDeliveryTimeDistributionCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintDeliveryTimeDistributionCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportDeliveryTimeDistributionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportDeliveryTimeDistributionCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Delivery Time Distribution";
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
                    //  if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsEditDeliveryTimeDistributionERM) { DXSplashScreen.Close(); }
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportDeliveryTimeDistributionCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsEditDeliveryTimeDistributionERM) { DXSplashScreen.Close(); }
                if (DXSplashScreen.IsActive ) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportDeliveryTimeDistributionCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

    
        private void DeliveryTimeDistributionTableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeliveryTimeDistributionTableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                if (File.Exists(DeliveryTimeDistributionGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(DeliveryTimeDistributionGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)GridControlSTDetails.View;

                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(DeliveryTimeDistributionGridSettingFilePath);

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
                    IsViewDeliveryTimeDistributionColumnChooserVisible = true;
                }
                else
                {
                    IsViewDeliveryTimeDistributionColumnChooserVisible = false;
                }

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);

                // datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method DeliveryTimeDistributionTableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeliveryTimeDistributionTableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(DeliveryTimeDistributionGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    isViewDeliveryTimeDistributionColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(DeliveryTimeDistributionGridSettingFilePath);

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

                    foreach (var dataObject in DeliveryTimeDistributionList)
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
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DeliveryTimeDistributionList.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = DeliveryTimeDistributionList.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plants Like '%{0}%'", DeliveryTimeDistributionList.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim()));
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


        #endregion

    }
}
