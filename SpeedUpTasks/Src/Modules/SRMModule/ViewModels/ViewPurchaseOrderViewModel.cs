using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.SRM.Views;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using OutlookApp = Microsoft.Office.Interop.Outlook.Application;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    class ViewPurchaseOrderViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            
        }

        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());



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

        #endregion // end public events region

        #region Declaration

        private bool isSaveChanges;
        private double dialogHeight;
        private double dialogWidth;
        bool isMoreNeeded = true;
        private WarehousePurchaseOrder warehousePurchaseOrder;
        private WorkflowStatus workflowStatus;
        private List<WorkflowStatus> workflowStatusList;
        private List<WorkflowStatus> workflowStatusButtons;
        private WorkflowStatus selectedWorkflowStatusButton;
        private List<WorkflowTransition> workflowTransitionList;
        #endregion

        #region Properties
        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
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

        public bool IsMoreNeeded
        {
            get
            {
                return isMoreNeeded;
            }
            set
            {
                isMoreNeeded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMoreNeeded"));
            }
        }

        public WarehousePurchaseOrder WarehousePurchaseOrder
        {
            get
            {
                return warehousePurchaseOrder;
            }
            set
            {
                warehousePurchaseOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehousePurchaseOrder"));
            }
        }
        public WorkflowStatus WorkflowStatus
        {
            get
            {
                return workflowStatus;
            }

            set
            {
                workflowStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatus"));
            }
        }

        public List<WorkflowStatus> WorkflowStatusList
        {
            get
            {
                return workflowStatusList;
            }

            set
            {
                workflowStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusList"));
            }
        }
        public List<WorkflowStatus> WorkflowStatusButtons
        {
            get
            {
                return workflowStatusButtons;
            }

            set
            {
                workflowStatusButtons = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusButtons"));
            }
        }
        public WorkflowStatus SelectedWorkflowStatusButton
        {
            get
            {
                return selectedWorkflowStatusButton;
            }

            set
            {
                selectedWorkflowStatusButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkflowStatusButton"));
            }
        }

        public List<WorkflowTransition> WorkflowTransitionList
        {
            get
            {
                return workflowTransitionList;
            }

            set
            {
                workflowTransitionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransitionList"));
            }
        }
        
        #endregion

        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand OpenPdfFileCommand { get; set; }
        public ICommand POItemDetailsViewHyperlinkClickCommand { get; set; }
        public ICommand WorkflowButtonClickCommand { get; set; }
        public ICommand OpenWorkflowDiagramCommand { get; set; }

        public ICommand SendEmailCommand { get; set; }

        #endregion

        #region Constructor

        public ViewPurchaseOrderViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ViewPurchaseOrderViewModel()...", category: Category.Info, priority: Priority.Low);

                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 130;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 50;  

                CancelButtonCommand = new DelegateCommand<object>(CancelButtonAction);
                OpenPdfFileCommand = new DelegateCommand<object> (OpenPDFDocument);
                POItemDetailsViewHyperlinkClickCommand = new DelegateCommand<object>(POItemDetailsViewHyperlinkClickCommandAction);
                WorkflowButtonClickCommand = new DelegateCommand<object>(WorkflowButtonClickCommandAction);
                OpenWorkflowDiagramCommand = new DelegateCommand<object>(OpenWorkflowDiagramCommandAction);
                SendEmailCommand = new DelegateCommand<object>(SendEmailCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor ViewPurchaseOrderViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewPurchaseOrderViewModel ViewPurchaseOrderViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods


        public void Init(long idWarehousePurchaseOrder, Warehouses objWarehouse)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WarehousePurchaseOrder = SRMService.GetPendingPOByIdWarehousePurchaseOrder(idWarehousePurchaseOrder);
                WarehousePurchaseOrder.Warehouse = objWarehouse;
                WorkflowStatusList = new List<WorkflowStatus>(SRMService.GetAllWorkflowStatus());
                WorkflowTransitionList = new List<WorkflowTransition>(SRMService.GetAllWorkflowTransitions());

                List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == WarehousePurchaseOrder.IdWorkflowStatus).Select(a=>a.IdWorkflowStatusTo).Distinct().ToList();
                WorkflowStatusButtons = new List<WorkflowStatus>();

                foreach(byte statusbutton in GetCurrentButtons)
                {
                    WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PoCancelButton()...", category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method PoCancelButton()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PoCancelButton()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenPDFDocument(object obj)
        {
            GridControl deliveryNoteGrid = (GridControl)obj;
            WarehouseDeliveryNote warehouseDeliveryNote = (WarehouseDeliveryNote)deliveryNoteGrid.CurrentItem;
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                documentViewModel.OpenPdfByCode(WarehousePurchaseOrder, warehouseDeliveryNote);

                documentView.DataContext = documentViewModel;
                documentView.Show();

                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //warehouseDeliveryNote.PdfFilePath
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                CustomMessageBox.Show(string.Format("Could not find file '{0}'.", warehouseDeliveryNote.PdfFilePath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Open Article View on hyperlink click
        /// [001][adhatkar][2020-05-21][GEOS2-2042]
        /// </summary>
        /// <param name="obj"></param>
        public void POItemDetailsViewHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method POItemDetailsViewHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
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
                WarehousePurchaseOrderItem otItem = (WarehousePurchaseOrderItem)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = SRMCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                int ArticleSleepDays = SRMCommon.Instance.ArticleSleepDays;
                articleDetailsViewModel.Init_SRM(otItem.Article.Reference, warehouseId, warehouse, ArticleSleepDays);
                articleDetailsView.DataContext = articleDetailsViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (obj as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                //[001] added
                if (articleDetailsViewModel.IsResult)
                {
                    //if (articleDetailsViewModel.UpdateArticle.MyWarehouse != null)
                        //otItem.ArticleMinimumStock = articleDetailsViewModel.UpdateArticle.MyWarehouse.MinimumStock;
                }
                //end
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method POItemDetailsViewHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method POItemDetailsViewHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            
        }
        private void WorkflowButtonClickCommandAction(object obj)
        {
            try
            {
                int status_id = Convert.ToInt32(obj);
                GeosApplication.Instance.Logger.Log("Method WorkflowButtonClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                TransitionWorkflowStatus(status_id);
                GeosApplication.Instance.Logger.Log("Method WorkflowButtonClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkflowButtonClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkflowButtonClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method WorkflowButtonClickCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TransitionWorkflowStatus(int currentStatus)
        {
            List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList = new List<LogEntriesByWarehousePO>();
            WorkflowTransition workflowTransition = new WorkflowTransition();
            workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == currentStatus);
            WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == currentStatus);
            
            if (workflowTransition != null && workflowTransition.IsCommentRequired == 1)
            {
                AddWorkflowStatusCommentView addWorkflowStatusCommentView = new AddWorkflowStatusCommentView();
                AddWorkflowStatusCommentViewModel addWorkflowStatusCommentViewModel = new AddWorkflowStatusCommentViewModel();
                EventHandler handle = delegate { addWorkflowStatusCommentView.Close(); };
                addWorkflowStatusCommentViewModel.RequestClose += handle;
                addWorkflowStatusCommentView.DataContext = addWorkflowStatusCommentViewModel;
                addWorkflowStatusCommentView.ShowDialogWindow();

                if (addWorkflowStatusCommentViewModel.IsSaveChanges == true)
                {
                    LogEntriesByWarehousePO CommentByWarehousePO = new LogEntriesByWarehousePO();
                    CommentByWarehousePO.IdWarehousePurchaseOrder = WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                    CommentByWarehousePO.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                    CommentByWarehousePO.Comments = addWorkflowStatusCommentViewModel.Comment;
                    CommentByWarehousePO.IdLogEntryType = 252;
                    CommentByWarehousePO.IsRtfText = false;
                    CommentByWarehousePO.Datetime = GeosApplication.Instance.ServerDateTime;
                    CommentByWarehousePO.People = new People();
                    CommentByWarehousePO.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                    CommentByWarehousePO.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                    CommentByWarehousePO.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                    LogEntriesByWarehousePOList.Add(CommentByWarehousePO);
                    WarehousePurchaseOrder.WarehousePOComments.Insert(0, CommentByWarehousePO);
                    WarehousePurchaseOrder.WarehousePOComments = new List<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOComments);


                    LogEntriesByWarehousePO logEntriesByWarehousePO_1 = new LogEntriesByWarehousePO();
                    logEntriesByWarehousePO_1.IdWarehousePurchaseOrder = WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                    logEntriesByWarehousePO_1.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                    logEntriesByWarehousePO_1.Comments = string.Format(System.Windows.Application.Current.FindResource("POWorkflowStatusCommentChangeLog").ToString(),WorkflowStatus.Name);
                    logEntriesByWarehousePO_1.IdLogEntryType = 253;
                    logEntriesByWarehousePO_1.IsRtfText = false;
                    logEntriesByWarehousePO_1.Datetime = GeosApplication.Instance.ServerDateTime;
                    logEntriesByWarehousePO_1.People = new People();
                    logEntriesByWarehousePO_1.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                    logEntriesByWarehousePO_1.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                    logEntriesByWarehousePO_1.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                    LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO_1);
                    WarehousePurchaseOrder.WarehousePOLogEntries.Insert(0, logEntriesByWarehousePO_1);
                    WarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOLogEntries);
                }
                else
                {
                    return;
                }
            }
            LogEntriesByWarehousePO logEntriesByWarehousePO = new LogEntriesByWarehousePO();
            logEntriesByWarehousePO.IdWarehousePurchaseOrder = WarehousePurchaseOrder.IdWarehousePurchaseOrder;
            logEntriesByWarehousePO.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
            logEntriesByWarehousePO.Comments = string.Format(System.Windows.Application.Current.FindResource("WorkflowStatusLogEntryByPO").ToString(), WarehousePurchaseOrder.WorkflowStatus.Name, WorkflowStatus.Name);
            logEntriesByWarehousePO.IdLogEntryType = 253;
            logEntriesByWarehousePO.IsRtfText = false;
            logEntriesByWarehousePO.Datetime = GeosApplication.Instance.ServerDateTime;
            logEntriesByWarehousePO.People = new People();
            logEntriesByWarehousePO.People.IdPerson= GeosApplication.Instance.ActiveUser.IdUser;
            logEntriesByWarehousePO.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
            logEntriesByWarehousePO.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

            LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO);
            WarehousePurchaseOrder.WarehousePOLogEntries.Insert(0, logEntriesByWarehousePO);
            WarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOLogEntries);

            IsSaveChanges = SRMService.UpdateWorkflowStatusInPO((uint)WarehousePurchaseOrder.IdWarehousePurchaseOrder, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByWarehousePOList);
            WarehousePurchaseOrder.IdWorkflowStatus = WorkflowStatus.IdWorkflowStatus;
            WarehousePurchaseOrder.WorkflowStatus = WorkflowStatus;
            List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == currentStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
            WorkflowStatusButtons = new List<WorkflowStatus>();

            foreach (byte statusbutton in GetCurrentButtons)
            {
                WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
            }
            WorkflowStatusButtons = new List<WorkflowStatus>(WorkflowStatusButtons);
            //if(IsSaveChanges==true)
            //{
            //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("POWorkflowStatusMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            //}
            //RequestClose(null, null);
        }


        private void OpenWorkflowDiagramCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()...", category: Category.Info, priority: Priority.Low);
                WorkflowDiagramViewModel workflowDiagramViewModel = new WorkflowDiagramViewModel();
                WorkflowDiagramView workflowDiagramView = new WorkflowDiagramView();
                EventHandler handle = delegate { workflowDiagramView.Close(); };
                workflowDiagramViewModel.RequestClose += handle;
                workflowDiagramViewModel.WorkflowStatusList = WorkflowStatusList;
                workflowDiagramViewModel.WorkflowTransitionList = WorkflowTransitionList.OrderByDescending(a=>a.IdWorkflowTransition).ToList();
                workflowDiagramView.DataContext = workflowDiagramViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                workflowDiagramView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenWorkflowDiagramCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SendEmailCommandAction(object obj)
        {
            string tempFilePath = Path.GetTempPath() + warehousePurchaseOrder.Code + ".pdf";
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (WarehousePurchaseOrder.EmailBody!=null && WarehousePurchaseOrder.AttachmentBytes!=null)
                {
                    OutlookApp outlookApp = new OutlookApp();
                    Microsoft.Office.Interop.Outlook.MailItem mailItem = outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                    mailItem.Subject = "PO " + WarehousePurchaseOrder.Code;
                    mailItem.HTMLBody = WarehousePurchaseOrder.EmailBody;
                    mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                    using (MemoryStream ms = new MemoryStream(WarehousePurchaseOrder.AttachmentBytes))
                    {
                        FileStream fs = new FileStream(tempFilePath, FileMode.Create);
                        ms.CopyTo(fs);
                        fs.Close();
                        mailItem.Attachments.Add(tempFilePath, Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue, 1, warehousePurchaseOrder.Code + ".pdf");
                    }
                    //Set a high priority to the message
                    mailItem.Importance = Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh;
                    mailItem.To = warehousePurchaseOrder.ArticleSupplier.ContactEmail;
                    mailItem.Send();

                    mailItem.Display(false);
                }
                else
                {
                    if (WarehousePurchaseOrder.EmailBody == null || warehousePurchaseOrder.EmailBody=="")
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_failure1").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    else if(WarehousePurchaseOrder.AttachmentBytes==null)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_failure").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                //bool IsSendMail = SRMService.SendSupplierPurchaseOrderRequestMail(WarehousePurchaseOrder, GeosApplication.Instance.ActiveUser.CompanyEmail);
                //if(IsSendMail==true)
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_Success").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                //}
                //else
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_failure").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //}
                GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_Success").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                //GeosApplication.Instance.Logger.Log(string.Format("Error in method SendEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                if(WarehousePurchaseOrder.AttachmentBytes!=null && File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        #endregion


    }
}
