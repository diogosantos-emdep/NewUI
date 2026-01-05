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
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        //end
        #endregion

        #region Properties
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

                List<Ots> SelectedOtsList = SendEmailWorkOrderOtsList.Where(x => x.IsSendReadyExpeditionEmail == true).ToList();

                if (SelectedOtsList.Count > 0)
                {
                    var idOTs = string.Join(",", SelectedOtsList.Select(i => i.IdOT));
                    //[001] Changed service method GetDetailsofReadyShippingOTItems to GetReadyForShippingOTItems
                    //[002] Changed service method GetReadyForShippingOTItems to GetReadyForShippingOTItems_V2040
                    //[003] Changed service method GetReadyForShippingOTItems_V2040 to GetReadyForShippingOTItems_V2041
                    List<Tuple<string, string, string>> EmailSendingErrorMsgList = WarehouseService.GetReadyForShippingOTItems_V2042(WarehouseCommon.Instance.Selectedwarehouse, idOTs);

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
        public void Init(List<Ots> workOrderOtsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);
                WindowHeader = System.Windows.Application.Current.FindResource("NewNotification").ToString();
                List<Ots> tempSendEmailWorkOrderOtsList = new List<Ots>();
                SendEmailWorkOrderOtsList = new List<Ots>();
                SendEmailWorkOrderOtsList = workOrderOtsList.Select(item => (Ots)item.Clone()).ToList();
                //[001] Added service method
                tempSendEmailWorkOrderOtsList = WarehouseService.GetNotPackingButOTDeliveryDateInCurrentWeek(WarehouseCommon.Instance.Selectedwarehouse).ToList();

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
