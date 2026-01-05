using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Modules.PLM.Views;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Prism.Logging;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.UI.Helper;
using System.Data;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Data;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.Utility;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Filtering;
using Emdep.Geos.Data.Common.PLM;

namespace Emdep.Geos.Modules.PLM.ViewModels
{
    class BasePriceListGridViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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

        private ObservableCollection<BasePrice> basePricelist;
        private BasePrice selectedBasePrice;

        private bool isDeleted;
        private bool isBusy;

        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        #endregion

        #region Properties

        public ObservableCollection<BasePrice> BasePricelist
        {
            get { return basePricelist; }
            set
            {
                basePricelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BasePricelist"));
            }
        }

        public BasePrice SelectedBasePrice
        {
            get
            {
                return selectedBasePrice;
            }
            set
            {
                selectedBasePrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBasePrice"));
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

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        #endregion

        #region ICommands


        public ICommand AddBasePriceListCommand { get; set; }
        public ICommand RefreshBasePriceListCommand { get; set; }
        public ICommand PrintBasePriceListCommand { get; set; }
        public ICommand ExportBasePriceListCommand { get; set; }
        public ICommand DeleteBasePriceCommand { get; set; }


        #endregion

        #region Constructor

        public BasePriceListGridViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor BasePriceListGridViewModel ...", category: Category.Info, priority: Priority.Low);

                //AddBasePriceListCommand = new RelayCommand(new Action<object>(AddBasePriceAction));
                RefreshBasePriceListCommand = new RelayCommand(new Action<object>(RefreshBasePriceAction));
                PrintBasePriceListCommand = new RelayCommand(new Action<object>(PrintBasePriceAction));
                ExportBasePriceListCommand = new RelayCommand(new Action<object>(ExportBasePriceAction));
                DeleteBasePriceCommand = new RelayCommand(new Action<object>(DeleteBasePriceAction));


                GeosApplication.Instance.Logger.Log("Constructor BasePriceListGridViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor BasePriceListGridViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

               BasePricelist = new ObservableCollection<BasePrice>(PLMService.GetBasePricesByYear());
                if (BasePricelist.Count > 0)
                    SelectedBasePrice = BasePricelist.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddBasePriceAction()", category: Category.Info, priority: Priority.Low);

                AddBasePriceView addBasePriceView = new AddBasePriceView();
                AddBasePriceViewModel addBasePriceViewModel = new AddBasePriceViewModel();
                EventHandler handle = delegate { addBasePriceView.Close(); };
                addBasePriceViewModel.RequestClose += handle;
                addBasePriceView.ShowDialog();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddBasePriceAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddBasePriceAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshBasePriceAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                BasePricelist = new ObservableCollection<BasePrice>(PLMService.GetBasePricesByYear());
                SelectedBasePrice = BasePricelist.FirstOrDefault();
                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshBasePriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshBasePriceAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshBasePriceAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshBasePriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintBasePriceAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["BasePriceListPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["BasePriceListPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintBasePriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintBasePriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportBasePriceAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Base Price List";
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

                    GeosApplication.Instance.Logger.Log("Method ExportBasePriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportBasePriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }


        private void DeleteBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteBasePriceAction()...", category: Category.Info, priority: Priority.Low);

                if (SelectedBasePrice.Name == null)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["BasePriceMessageWithoutName"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsDeleted = PLMService.IsDeletedBasePriceList(SelectedBasePrice.IdBasePriceList);

                        if (IsDeleted)
                        {
                            BasePricelist.Remove(SelectedBasePrice);
                            BasePricelist = new ObservableCollection<BasePrice>(BasePricelist);
                            SelectedBasePrice = BasePricelist.FirstOrDefault();
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionDetailsDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["BasePriceDetailsMessage"].ToString(), "[" + SelectedBasePrice.Name.ToString() + "]"), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsDeleted = PLMService.IsDeletedBasePriceList(SelectedBasePrice.IdBasePriceList);

                        if (IsDeleted)
                        {
                            BasePricelist.Remove(SelectedBasePrice);
                            BasePricelist = new ObservableCollection<BasePrice>(BasePricelist);
                            SelectedBasePrice = BasePricelist.FirstOrDefault();
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("BasePriceDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteBasePriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteBasePriceAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDetectionItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteDetectionItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion
    }
}
