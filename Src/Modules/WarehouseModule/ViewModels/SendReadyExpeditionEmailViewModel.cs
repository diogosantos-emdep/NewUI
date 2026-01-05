using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Utils.Design.Internal;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class SendReadyExpeditionEmailViewModel : INotifyPropertyChanged
    {

        #region Task Log
        //[000][skale][15-01-2020][GEOS2-1840] Ready for shipping automatic email
        #endregion

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");

        #endregion //End Of Services

        #region Declaration
        //[000] added
        private string windowHeader;
        private double dialogHeight;
        private List<Ots> sendEmailWorkOrderOtsList;
        public List<Ots> SendEmailWorkOrderOtsList
        {
            get { return sendEmailWorkOrderOtsList; }
            set
            {
                sendEmailWorkOrderOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SendEmailWorkOrderOtsList"));
            }
        }
        private List<Warehouses> warehousesList;
        public List<Warehouses> WarehousesList
        {
            get { return warehousesList; }
            set
            {
                warehousesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehousesList"));
            }
        }
        private Warehouses selectedwarehousesItem;
        public Warehouses SelectedwarehousesItem
        {
            get { return selectedwarehousesItem; }
            set
            {
                selectedwarehousesItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedwarehousesItem"));
            }
        }

        List<Ots> selectedOtsList;
        //end
        #endregion

        #region Properties

        public List<Ots> SelectedOtsList
        {
            get { return selectedOtsList; }
            set
            {
                selectedOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOtsList"));
            }
        }
        public string WindowHeader
        {
            get { return windowHeader; }
            set { windowHeader = value; OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader")); }
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

        #endregion // End Of Events

        #region  ICommand

        public ICommand CancelButtonCommand { get; set; }
        public ICommand SendButtonCommand { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; set; }
        public ICommand CustomColumnDisplayTextCommand { get; set; }


        #endregion

        #region Constructor
        public SendReadyExpeditionEmailViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SendReadyExpeditionEmailViewModel()...", category: Category.Info, priority: Priority.Low);

                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 130;
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                SendButtonCommand = new RelayCommand(new Action<object>(SendButtonCommandAction));
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
                CustomColumnDisplayTextCommand = new DelegateCommand<CustomColumnDisplayTextEventArgs>(CustomColumnDisplayTextActionCommand);

                GeosApplication.Instance.Logger.Log("Constructor SendReadyExpeditionEmailViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Constructor SendReadyExpeditionEmailViewModel().....{0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Command Action
        private void CloseWindow(object obj)
        {

            if (SendEmailWorkOrderOtsList != null && SendEmailWorkOrderOtsList.Count > 0)
            {
                var SelectedItemList = SendEmailWorkOrderOtsList.Where(x => x.IsSendReadyExpeditionEmail == true).ToList();
                SelectedItemList.ForEach(c => c.IsSendReadyExpeditionEmail = false);
            }
            RequestClose(null, null);
        }

        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseEditValueChangedCommandAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

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

              string st=   SelectedwarehousesItem.Company.Alias;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction() executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseEditValueChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CustomColumnDisplayTextActionCommand(DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomColumnDisplayTextActionCommand...", category: Category.Info, priority: Priority.Low);

                if (e.Column.FieldName == "DeliveryDate")
                {
                    DateTime DeliveryDate = (DateTime)e.Value;

                    e.DisplayText = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DeliveryDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString().PadLeft(2, '0');
                }
                GeosApplication.Instance.Logger.Log("Method CustomColumnDisplayTextActionCommand()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Method CustomColumnDisplayTextActionCommand....{0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// [001][cpatil][29-01-2020][GEOS2-1840]Ready for shipping automatic email
        /// [002][cpatil][28-02-2020][GEOS2-2116]Include Orders without PO in the list to send notifications
        /// [003][cpatil][17-03-2020][GEOS2-2184]Ready for shipment email is not including pending items in same quotation
        /// [004][cpatil][21-11-2020][GEOS2-2195]Add new column “Observations” in the email sent informing about ready for shipment items
        /// [005][cpatil][24-05-2021][GEOS2-2962]Allow to have Email distribution lists by warehouse for the same customer
        /// [006][cpatil][28-07-2021][GEOS2-2901]Add type of transport in the automatic mail, for plants
        /// [008][cpatil][16-03-2023][GEOS2-4148]
        private void SendButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendButtonCommandAction...", category: Category.Info, priority: Priority.Low);

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

                SelectedOtsList = SendEmailWorkOrderOtsList.Where(x => x.IsSendReadyExpeditionEmail == true).ToList();

                if (SelectedOtsList.Count > 0)
                {
                    var idOTs = string.Join(",", SelectedOtsList.Select(i => i.IdOT));
                    #region Service comments
                    //WarehouseService=new WarehouseServiceController("localhost:6699");
                    //[001] Changed service method GetDetailsofReadyShippingOTItems to GetReadyForShippingOTItems
                    //[002] Changed service method GetReadyForShippingOTItems to GetReadyForShippingOTItems_V2040
                    //[003] Changed service method GetReadyForShippingOTItems_V2040 to GetReadyForShippingOTItems_V2041
                    //[004] Changed service method GetReadyForShippingOTItems_V2051 to GetReadyForShippingOTItems_V2080
                    //[005] Changed service method GetReadyForShippingOTItems_V2080 to GetReadyForShippingOTItems_V2150
                    //[006] Changed service method GetReadyForShippingOTItems_V2150 to GetReadyForShippingOTItems_V2170
                    //[007] Changed service method GetReadyForShippingOTItems_V2170 to GetReadyForShippingOTItems_V2290
                    //[008] Changed service method
                    // shubham[skadam] GEOS2-3907 Error when trying to send the Email Ready for Shipment  06 Sep 2022   
                    //List<Tuple<string, string, string>> EmailSendingErrorMsgList = WarehouseService.GetReadyForShippingOTItems_V2300(WarehouseCommon.Instance.Selectedwarehouse, idOTs,SelectedwarehousesItem.IdWarehouse);
                    //List<Tuple<string, string, string>> EmailSendingErrorMsgList = WarehouseService.GetReadyForShippingOTItems_V2370(WarehouseCommon.Instance.Selectedwarehouse, idOTs, SelectedwarehousesItem.IdWarehouse);
                    //Shubham[skadam] GEOS2-3999 Missing Expected supplier dates in pending materials in Shipping email 26 05 2023
                    //Service GetReadyForShippingOTItems_V2390 Updated with GetReadyForShippingOTItems_V2450 by  [rdixit][GEOS2-4860][25.10.2023]
                    //Service GetReadyForShippingOTItems_V2450 Updated with GetReadyForShippingOTItems_V2500 by [GEOS2-5421][rdixit][12.03.2024]
                    //Service GetReadyForShippingOTItems_V2500 Updated with GetReadyForShippingOTItems_V2520 by //rajashri GEOS2-5487[14-05-2024]
                    #endregion
                    List<Tuple<string, string, string>> EmailSendingErrorMsgList = WarehouseService.GetReadyForShippingOTItems_V2520(WarehouseCommon.Instance.Selectedwarehouse, idOTs, SelectedwarehousesItem.IdWarehouse);
                    //[rdixit][GEOS2-3551][20.07.2022]
                    List<string> EmailSentPlant = new List<string>();
                    List<string> EmailNotSentPlant = new List<string>();
                    List<string> EmailNotAddedPlant = new List<string>();

                    foreach (var item in EmailSendingErrorMsgList)
                    {
                        if (item.Item1 == "EmailSentToPlant" && item.Item2 != null)
                        {
                            EmailSentPlant.Add(item.Item2);
                        }
                        else if (item.Item1 == "NoDeliveredItemsEmailNotSendToPlant" && item.Item2 != null)
                        {
                            EmailNotSentPlant.Add(item.Item2);
                        }
                        else if (item.Item1 == "HtmlFileNotExistEmailNotSendToPlant")
                        {
                            GeosApplication.Instance.Logger.Log(item.Item3, category: Category.Exception, priority: Priority.Low);
                        }
                        else if (item.Item1 == "EmailToNotAddedEmailNotSendToPlant" && item.Item2 != null)
                        {
                            EmailNotAddedPlant.Add(item.Item2);
                        }
                        else if (item.Item1 == "Exception")
                        {
                            GeosApplication.Instance.Logger.Log(item.Item3, category: Category.Exception, priority: Priority.Low);
                        }

                        //GeosApplication.Instance.Logger.Log(item, category: Category.Info, priority: Priority.Low);
                    }

                    //Ready for shipping email has been sent successfully.
                    String FinalMessage = null;

                    if (EmailSentPlant.Count > 0)
                    {
                        FinalMessage += string.Format("Ready for shipping email has been sent successfully - {0}. \n", string.Join(",", EmailSentPlant));

                    }
                    if (EmailNotSentPlant.Count > 0)
                    {
                        FinalMessage += string.Format("Email has not been sent successfully - {0}. \n", string.Join(",", EmailNotSentPlant));
                    }
                    if (EmailNotAddedPlant.Count > 0)
                    {
                        FinalMessage += string.Format("Email has not been added to plants - {0}", string.Join(",", EmailNotAddedPlant));
                    }

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    if (EmailSentPlant.Count > 0)
                    {
                        GeosApplication.Instance.Logger.Log("Method SendButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                        CustomMessageBox.Show(FinalMessage.TrimEnd('\n'), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        //[pramod.misal][GEOS2-5094][19.12.2023]
                        #region GEOS2-5094

                        var itemToDo = SelectedOtsList.Where(item => string.Equals(item.WorkflowStatus.Name, "To Do", StringComparison.OrdinalIgnoreCase));

                        if (itemToDo.Any())
                        {
                            ChangeDeliveryDateOfOTsView changeDeliveryDateOfOTsView = new ChangeDeliveryDateOfOTsView();
                            ChangeDeliveryDateOfOTsViewModel changeDeliveryDateOfOTsViewModel = new ChangeDeliveryDateOfOTsViewModel();
                            EventHandler handle = delegate { changeDeliveryDateOfOTsView.Close(); };
                            changeDeliveryDateOfOTsViewModel.RequestClose += handle;
                            changeDeliveryDateOfOTsViewModel.Init(SelectedOtsList);
                            changeDeliveryDateOfOTsView.DataContext = changeDeliveryDateOfOTsViewModel;
                            changeDeliveryDateOfOTsView.ShowDialogWindow();
                        }

                        #endregion
                    }
                    else if (EmailNotSentPlant.Count > 0 || EmailNotAddedPlant.Count > 0)
                    {
                        GeosApplication.Instance.Logger.Log("Method SendButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                        CustomMessageBox.Show(FinalMessage.TrimEnd('\n'), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                        
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log("Method SendButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                        CustomMessageBox.Show(System.Windows.Application.Current.FindResource("EmailNotSentSuccessfullyMessage").ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                       
                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["SelectAtleastoneWoMessage"].ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                var SelectedItemList = SendEmailWorkOrderOtsList.Where(x => x.IsSendReadyExpeditionEmail == true).ToList();
                SelectedItemList.ForEach(c => c.IsSendReadyExpeditionEmail = false);

                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Method SendButtonCommandAction().....{0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(ex.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }
        #endregion

        #region Method
        /// [002][cpatil][28-02-2020][GEOS2-2117]Add OTs not PICKED but expected to be sent in Notifications list
        /// [003][cpatil][21-11-2020][GEOS2-2620]Show all pending ots in “Send Notification” window
        /// [004][cpatil][10-03-2022][GEOS2-3329]Closed shipments appear in send email of incoming shipment option
        /// [005][cpatil][27-04-2022][GEOS2-3717]
        public void Init(List<Ots> workOrderOtsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);
                WindowHeader = System.Windows.Application.Current.FindResource("NewNotification").ToString();
                List<Ots> tempSendEmailWorkOrderOtsList = new List<Ots>();
                SendEmailWorkOrderOtsList = new List<Ots>();
                SendEmailWorkOrderOtsList = workOrderOtsList.Select(item => (Ots)item.Clone()).ToList();
                WarehousesList = WarehouseCommon.Instance.WarehouseList;
                SelectedwarehousesItem = WarehouseCommon.Instance.Selectedwarehouse;
                //[001] Added service method
                // [002] Changed service method
                // [003] Changed service method
                // [004] Changed service method

                #region Service Commited For GEOS2-5093
                //tempSendEmailWorkOrderOtsList = WarehouseService.GetNotPacking_V2260(WarehouseCommon.Instance.Selectedwarehouse).ToList();
                #endregion
                //[pramod.misal][GEOS2-5093][15-12-2023]
                tempSendEmailWorkOrderOtsList = WarehouseService.GetNotPacking_V2470(WarehouseCommon.Instance.Selectedwarehouse).ToList();             

                if (tempSendEmailWorkOrderOtsList != null)
                {
                    foreach (Int64 item in tempSendEmailWorkOrderOtsList.Select(i => i.IdOT).ToList())
                    {
                        if (!SendEmailWorkOrderOtsList.Any(se => se.IdOT == item))
                        {
                            SendEmailWorkOrderOtsList.Add(tempSendEmailWorkOrderOtsList.Where(ts => ts.IdOT == item).FirstOrDefault());                          
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method Init....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Method Init.....{0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion



    }
}
