using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm.POCO;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Warehouse.Reports;
using DevExpress.Xpf.Printing;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraReports.UI;
using Emdep.Geos.UI.Commands;
using Microsoft.Win32;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using DevExpress.Xpf.LayoutControl;
//DevExpress.XtraGrid.Views.Base;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class WorkOrderItemDetailsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Task log
        // [000][skale][14-08-2019][GEOS2-1659]Add work time log in Work Order details
        // [001][avpawar][22-08-2019][GEOS2-1648] Wizzard should not be displayed if there are NO items
        // [002][avpawar][28/08/2019][GEOS2-1655] Add "Stock History" section in Work Order
        // [003][avpawar][11/09/2019][GEOS2-1655] Add Loaction and Delivery Note column in "Stock History" section in Work Order
        // [004][avpawar][09/01/2020][GEOS2-1848] Add Producer Info in Work Order Label On Print button
        #endregion

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services

        #region Declaration

        private Ots oT;
        private Int64 downloadRemainingQuantity;
        long idOtTemp;
        Warehouses objWarehouseTemp;
        private double dialogHeight;
        private double dialogWidth;

        //[000] added
        private List<OTWorkingTime> workLogItemList;

        private string worklogTotalTime;

        private List<UserShortDetail> userImageList;

        bool isBusy; // [002] added       

        MaximizedElementPosition maximizedElementPosition;

        #endregion // End Of Declaration

        #region Public Properties

        public Bitmap ReportHeaderImage { get; set; }
        public Warehouses WarehouseDetails { get; set; }

        public Ots OT
        {
            get { return oT; }
            set
            {
                oT = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
            }
        }

        public long DownloadRemainingQuantity
        {
            get { return downloadRemainingQuantity; }
            set
            {
                downloadRemainingQuantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadRemainingQuantity"));
            }
        }

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }

        //[000] added
        public List<OTWorkingTime> WorkLogItemList
        {
            get { return workLogItemList; }
            set { workLogItemList = value; }
        }
        public List<UserShortDetail> UserImageList
        {
            get { return userImageList; }
            set { userImageList = value; }
        }
        public string WorklogTotalTime
        {
            get { return worklogTotalTime; }
            set
            {
                worklogTotalTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorklogTotalTime"));
            }
        }
        // [002] added
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        // [002] End

        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }

        #endregion

        #region ICommands
        public ICommand CommandPendingWorkOrderItemShowCancelButton { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand ScanMaterialsCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand WorkOrderItemDetailsViewHyperlinkClickCommand { get; set; }
        public ICommand PrintWorkOrderItemCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand DNHyperlinkClickCommand { get; set; } //[003] added
        public ICommand ExportWorkLogCommand { get; set; }


        #endregion // End Of ICommands

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

        #endregion // End Of Events

        #region Constructor
        public WorkOrderItemDetailsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsViewModel()...", category: Category.Info, priority: Priority.Low);
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 95;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                PopupMenuShowingCommand = new DelegateCommand<OpenPopupEventArgs>(OpenPopupDateEditAction);
                CommandPendingWorkOrderItemShowCancelButton = new DelegateCommand<object>(CommandPendingWorkOrderItemShowCancelAction);
                WorkOrderItemDetailsViewHyperlinkClickCommand = new DelegateCommand<object>(WorkOrderItemDetailsViewHyperlinkClickCommandAction);
                ScanMaterialsCommand = new DelegateCommand<object>(ScanAction);
                PrintCommand = new DelegateCommand<object>(PrintCommandAction);
                ReportHeaderImage = ResizeImage(new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.EmdepMonoBlue));
                PrintWorkOrderItemCommand = new DelegateCommand<object>(PrintWorkOrderItem);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportStockLogButtonCommandAction));   // [002] added
                DNHyperlinkClickCommand = new DelegateCommand<object>(DNHyperlinkClickCommandAction);           //[003] Added
                ExportWorkLogCommand = new RelayCommand(new Action<object>(ExportWorkLogCommandAction));

                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor WorkOrderItemDetailsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // End Of Constructor

        #region Methods

        /// <summary>
        /// method for resize image.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private Bitmap ResizeImage(Bitmap bitmap)
        {
            GeosApplication.Instance.Logger.Log("Method ResizeImage ...", category: Category.Info, priority: Priority.Low);

            Bitmap resized = new Bitmap(300, 100);

            try
            {
                Graphics g = Graphics.FromImage(resized);

                g.DrawImage(bitmap, new Rectangle(0, 0, resized.Width, resized.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
                g.Dispose();

                //Save picture in users temp folder.
                string myTempFile = Path.Combine(Path.GetTempPath(), "EmdepLogo.jpg");

                //delete if already image exist there.
                if (File.Exists(myTempFile))
                {
                    File.Delete(myTempFile);
                }

                resized.Save(myTempFile, ImageFormat.Jpeg);
                return resized;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ResizeImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return resized;
        }

        /// <summary>
        /// Method for Print work order 
        /// [001][skale][23/01/2020][GEOS2-1937]- Add OT comments in OT label
        /// </summary>
        /// <param name="idOt"></param>
        /// <param name="objWarehouse"></param>
        public void PrintCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PrintCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
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

                WorkorderReport report = new WorkorderReport();
                report.imgLogo.Image = ReportHeaderImage;
                report.lblCarriageType.BackColor = System.Drawing.Color.Black;
                report.lblCarriageType.ForeColor = System.Drawing.Color.White;
                report.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                report.lblTradeGroup.ForeColor = System.Drawing.Color.Black;
                report.lblCustomer.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrLblCustomer.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 40, System.Drawing.FontStyle.Bold);
                report.lblWO.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrLblWO.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 50, System.Drawing.FontStyle.Bold);
                report.lblCarriageMethod.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.lblCarriageType.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 100, System.Drawing.FontStyle.Bold);
                report.lblDeliveryDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrLblDeliveyDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 40, System.Drawing.FontStyle.Bold);
                report.lblTradeGroup.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 75, System.Drawing.FontStyle.Bold);
                report.xrLblWarehouse.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                 
                 //[004] Added
                report.lblProducerOrigin.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrLblProducerName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 50, System.Drawing.FontStyle.Bold);

                //[001] added
                report.lblComment.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrlblComment.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 50, System.Drawing.FontStyle.Bold);

                List<WorkorderItemPrintDetails> tempList = new List<WorkorderItemPrintDetails>();
                WorkorderItemPrintDetails workOrderItemPrintDetails = new WorkorderItemPrintDetails();
                workOrderItemPrintDetails.Customer = OT.Quotation.Site.Name;
                workOrderItemPrintDetails.Workorder = OT.Code;

                if (OT.Quotation.Site.CountryGroup != null)
                {
                    if (OT.Quotation.Site.CountryGroup.IsFreeTrade == 1)
                    {
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.Black;
                        report.lblTradeGroup.Text = OT.Quotation.Site.CountryGroup.Name;
                        workOrderItemPrintDetails.TradeGroup = OT.Quotation.Site.CountryGroup.Name;
                    }
                    else
                    {
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                        report.lblTradeGroup.Text = OT.Quotation.Site.Country.Iso;
                        workOrderItemPrintDetails.TradeGroup = OT.Quotation.Site.Country.Iso;
                    }
                }
                else
                {
                    report.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                    report.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                    report.lblTradeGroup.Text = OT.Quotation.Site.Country.Iso;
                    workOrderItemPrintDetails.TradeGroup = OT.Quotation.Site.Country.Iso;
                }

                if (OT.DeliveryDate != null)
                    workOrderItemPrintDetails.DeliveryDateString = OT.DeliveryDate.Value.ToShortDateString();

                if (OT.Quotation.Offer.CarriageMethod != null && OT.Quotation.Offer.IdCarriageMethod != null)
                {
                    string typeText = string.Empty;
                    typeText = OT.Quotation.Offer.CarriageMethod.Value.Substring(0, 1);
                    workOrderItemPrintDetails.CarriageText = typeText;
                    if (OT.Quotation.Offer.IdCarriageMethod == 50)
                    {
                        workOrderItemPrintDetails.CarriageType = new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.Ground);
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                    }
                    else if (OT.Quotation.Offer.IdCarriageMethod == 51)
                    {
                        workOrderItemPrintDetails.CarriageType = new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.Sea);
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                    }
                    else if (OT.Quotation.Offer.IdCarriageMethod == 52)
                    {
                        workOrderItemPrintDetails.CarriageType = new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.Air);
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.Black;
                    }
                }
                else
                {
                    report.lblCarriageType.BackColor = System.Drawing.Color.Transparent;
                }

                report.xrLblWarehouse.Text = WarehouseCommon.Instance.Selectedwarehouse.Name;           // [004] Added

                if (OT.CountryGroup != null)                                                            // [004] Added
                {
                    report.xrLblProducerName.Text = OT.CountryGroup.Name;
                    report.xrLine5.LocationF = new DevExpress.Utils.PointFloat(24F, 321.7083F);
                    report.lblWO.LocationF = new DevExpress.Utils.PointFloat(27F, 330.2099F);
                }
                else
                {
                    report.xrProducerPanel.Visible = false;
                    report.xrLine9.Visible = false;
                    report.xrLblCustomer.SizeF = new System.Drawing.SizeF(575.25F, 188.2049F);
                    report.lblTradeGroup.SizeF = new System.Drawing.SizeF(187.75F, 250.7917F);
                    report.xrLine1.SizeF = new System.Drawing.SizeF(2.083313F, 250.7917F);
                    report.xrLine5.LocationF = new DevExpress.Utils.PointFloat(24F, 248.7917F);
                    report.lblWO.LocationF = new DevExpress.Utils.PointFloat(27F, 255.2099F);
                    report.xrLblWO.LocationF = new DevExpress.Utils.PointFloat(27F, 300.2883F);
                }
                //[001] added
                if (string.IsNullOrEmpty(OT.Comments))
                    report.xrlblComment.Text = string.Empty;
                else
                    report.xrlblComment.Text = OT.Comments;
               
                //report.xrlblComment.Text = OT.Comments.Replace("\r\n","").Trim();


                tempList.Add(workOrderItemPrintDetails);
                report.DataSource = tempList;

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = report;
                report.CreateDocument();
                window.Show();

                ReportPrintTool printTool = new ReportPrintTool(report);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][14-08-2019][GEOS2-1659] Add work time log in Work Order details
        /// </summary>
        /// <param name="idOt"></param>
        /// <param name="objWarehouse"></param>
        public void Init(long idOt, Warehouses objWarehouse)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                idOtTemp = idOt;
                objWarehouseTemp = objWarehouse;
                WarehouseDetails = objWarehouse;
                OT = WarehouseService.GetWorkOrderByIdOt_V2038(idOt, objWarehouse);

                DownloadRemainingQuantity = OT.OtItems.Sum(oti => oti.RevisionItem.RemainingQuantity);

                //[001] Added
                WorkLogItemList = new List<OTWorkingTime>();
                UserImageList = new List<UserShortDetail>();
               
                    //WorkLogItemList = WarehouseService.GetOTWorkingTimeDetails(idOtTemp, objWarehouse);
                    WorkLogItemList = WarehouseService.GetOTWorkingTimeDetails_V2036(idOtTemp, objWarehouse);
                    List<UserShortDetail> tempList = WorkLogItemList.Select(x => x.UserShortDetail).ToList();
                    UserImageList = tempList.GroupBy(x => x.IdUser).Select(x => x.First()).ToList();
              
                TimeSpan worklogTotalTime = new TimeSpan(WorkLogItemList.Sum(r => r.TotalTime.Ticks));

                WorklogTotalTime = string.Format("{0}H {1}M", worklogTotalTime.Hours, worklogTotalTime.Minutes);
                //End
                SetMaximizedElementPosition();


                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to restrict dateEdit Popup.
        /// </summary>
        /// <param name="obj"></param>
        private void OpenPopupDateEditAction(OpenPopupEventArgs obj)
        {
            obj.Cancel = true;
            obj.Handled = true;
        }

        //decimal qty = 0; decimal weight = 0;
        //public void UnboundData(object obj)
        //{
        //    if (obj == null) return;
        //    TreeListCustomColumnDisplayTextEventArgs e = obj as TreeListCustomColumnDisplayTextEventArgs;
        //    if (e != null)
        //    {
        //        if (e.Column.FieldName == "RevisionItem.Quantity")
        //        {
        //            qty = 0;
        //            qty = Convert.ToDecimal(e.DisplayText);
        //        }
        //        if (e.Column.FieldName == "RevisionItem.WarehouseProduct.Article.Weight")
        //        {
        //            weight = 0;
        //            weight = Convert.ToDecimal(e.Value);
        //            e.DisplayText = Convert.ToString((qty * weight));
        //            weight = qty * System.Convert.ToDecimal(e.Value);
        //            if (weight < 1)
        //            {
        //                if (Math.Round(weight * 1000, 0) == 1000)
        //                    e.DisplayText = string.Format("{0} Kg", 1);
        //                e.DisplayText = string.Format("{0} gr", Math.Round(weight * 1000, 0));
        //            }
        //            else
        //            {
        //                e.DisplayText = string.Format("{0} Kg", Math.Round(weight, 3));
        //            }
        //        }
        //    }
        //}

        private void PrintWorkOrderItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderItem....", category: Category.Info, priority: Priority.Low);

                TreeListView detailView = (TreeListView)((object[])obj)[0];
               // OtItem otItem = (OtItem)obj;
               OtItem otItem =(OtItem)((object[])obj)[1];
                WorkOrderPrintView workOrderPrintView = new WorkOrderPrintView();
                WorkOrderPrintViewModel workOrderPrintViewModel = new WorkOrderPrintViewModel();
                EventHandler handler = delegate { workOrderPrintView.Close(); };
                workOrderPrintViewModel.RequestClose += handler; //392235
                workOrderPrintView.DataContext = workOrderPrintViewModel;
                workOrderPrintViewModel.Init(otItem, OT);
                var ownerInfo = (detailView as FrameworkElement);
                workOrderPrintView.Owner = Window.GetWindow(ownerInfo);
                workOrderPrintView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderItem....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkOrderItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for open scan picking material window.
        /// [001] [GEOS2-1648][avpawar] Wizzard should not be displayed if there are NO items
        /// </summary>
        /// <param name="obj"></param>
        private void ScanAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);
                bool FollowFIFO;
                string FIFOGotoItem;
                bool isTimer;
                bool isBatchLabel;
                PickingMaterialsViewModel pickingMaterialsViewModel = new PickingMaterialsViewModel();
                isTimer = WarehouseCommon.Instance.IsPickingTimer;
                pickingMaterialsViewModel.PreviousTimerValue = isTimer;
                bool isItemsAvailable = false; //[001] Added

                do
                {
                    PickingMaterialsView pickingMaterialsView = new PickingMaterialsView();

                    FollowFIFO = pickingMaterialsViewModel.FollowFIFO;
                    FIFOGotoItem = pickingMaterialsViewModel.FIFOGotoItem;
                    isTimer = pickingMaterialsViewModel.PreviousTimerValue;
                    isBatchLabel = pickingMaterialsViewModel.PrintBatchLabelAfterScanCompleted;
                    pickingMaterialsViewModel = new PickingMaterialsViewModel();
                    pickingMaterialsViewModel.PreviousTimerValue = isTimer;
                    pickingMaterialsViewModel.PrintBatchLabelAfterScanCompleted = isBatchLabel;
                    EventHandler handle = delegate { pickingMaterialsView.Close(); };
                    pickingMaterialsViewModel.RequestClose += handle;
                    pickingMaterialsViewModel.SetFollowFIFO(FollowFIFO, FIFOGotoItem);
                    pickingMaterialsViewModel.InIt(OT, WarehouseDetails);

                    // [001] Added
                    if (pickingMaterialsViewModel.materialSoredList != null && pickingMaterialsViewModel.materialSoredList.Count > 0)
                    {
                        isItemsAvailable = true;

                        //foreach (OtItem item in pickingMaterialsViewModel.OtItemList)
                        //{
                        //    if (item.PickingMaterialsList != null && item.PickingMaterialsList.Count > 0)
                        //    {
                        //        isItemsAvailable = true;
                        //        break;
                        //    }
                        //}
                    }

                    if (isItemsAvailable)
                    {
                        pickingMaterialsView.DataContext = pickingMaterialsViewModel;
                        pickingMaterialsView.ShowDialog();
                    }
                    else
                    {
                        break;
                    }

                } while (pickingMaterialsViewModel.IsCanceled == false);

                //[001] Added
                if (!isItemsAvailable)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PickingNoItemsAvailable").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                //[001] End

                //OT = WarehouseService.GetWorkOrderByIdOt(idOtTemp, Convert.ToInt32(objWarehouseTemp.IdWarehouse), objWarehouseTemp);
                OT = WarehouseService.GetWorkOrderByIdOt_V2035(idOtTemp, objWarehouseTemp);

                DownloadRemainingQuantity = OT.OtItems.Sum(oti => oti.RevisionItem.RemainingQuantity);

                GeosApplication.Instance.Logger.Log("Method ScanAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for cloase window.
        /// </summary>
        /// <param name="obj"></param>
        private void CommandPendingWorkOrderItemShowCancelAction(object obj)
        {
            RequestClose(null, null);
        }
        public void Dispose()
        {

        }

        /// <summary>
        /// Method to Open Article View on hyperlink click
        /// [001][skale][2019-03-06][GEOS2-1515][SP-64]Add new column "Stock" in Work Order
        /// </summary>
        /// <param name="obj"></param>
        public void WorkOrderItemDetailsViewHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorkOrderItemDetailsViewHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
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

                TreeListView detailView = (TreeListView)obj;
                OtItem otItem = (OtItem)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.Init(otItem.RevisionItem.WarehouseProduct.Article.Reference, warehouseId);
                articleDetailsView.DataContext = articleDetailsViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                //[001] added
                if (articleDetailsViewModel.IsResult)
                {
                    if (articleDetailsViewModel.UpdateArticle.MyWarehouse != null)
                        otItem.ArticleMinimumStock = articleDetailsViewModel.UpdateArticle.MyWarehouse.MinimumStock;
                }
                //end
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method WorkOrderItemDetailsViewHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WorkOrderItemDetailsViewHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [002][avpawar][28/08/2019][GEOS2-1655][Add "Stock History" section in Work Order]
        /// Method to export Stock Log on Excel
        /// </summary>
        /// <param name="obj"></param>
        private void ExportStockLogButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportStockLogButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Stock_Log";
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

                    TableView tblStockLog = ((TableView)obj);
                    tblStockLog.ShowTotalSummary = false;
                    tblStockLog.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    tblStockLog.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    tblStockLog.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportStockLogButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportStockLogButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method is for [GEOS2-1655][Add "Stock History" section in Work Order]
        /// </summary>
        /// <param name="e"></param>
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }


        /// <summary>
        /// [003][avpawar][11/09/2019][GEOS2-1655][Add Location and Delivery Note in "Stock History" section in Work Order]
        /// Method to Open Delivery Note on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
        private void DNHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DNHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

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

                TableView tblView = (TableView)obj;
                ArticlesStock ac = (ArticlesStock)tblView.DataControl.CurrentItem;
                EditDeliveryNoteView editDeliveryNoteView = new EditDeliveryNoteView();
                EditDeliveryNoteViewModel editDeliveryNoteViewModel = new EditDeliveryNoteViewModel();
                EventHandler handle = delegate { editDeliveryNoteView.Close(); };
                editDeliveryNoteViewModel.RequestClose += handle;
                WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById(ac.WarehouseDeliveryNoteItem.IdWarehouseDeliveryNote);
                editDeliveryNoteViewModel.Init(wdn);
                editDeliveryNoteView.DataContext = editDeliveryNoteViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                var ownerInfo = (tblView as FrameworkElement);
                editDeliveryNoteView.Owner = Window.GetWindow(ownerInfo);
                editDeliveryNoteView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method DNHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DNHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        /// </summary>
        public void SetMaximizedElementPosition()
        {

            if (GeosApplication.Instance.UserSettings != null)
            {
                if (GeosApplication.Instance.UserSettings.ContainsKey("Appearance"))
                {
                    if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["Appearance"].ToString()))
                    {
                        MaximizedElementPosition = MaximizedElementPosition.Right;
                    }
                    else
                    {
                        MaximizedElementPosition = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["Appearance"].ToString(), true);
                    }
                }
                else
                {
                    MaximizedElementPosition = MaximizedElementPosition.Right;
                }

            }
        }

        private void ExportWorkLogCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkLogCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Work Log List" + " - " + OT.Code;
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
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportWorkLogCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWorkLogCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // End Of Methods
    }
}
