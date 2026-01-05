using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Outlook = Microsoft.Office.Interop.Outlook;

//[shweta.thube][GEOS2-8063][27/05/2025]
namespace Emdep.Geos.Modules.APM.ViewModels
{
    public class SendEmailNotificationViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Services
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
        #endregion

        #region public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        #endregion // Events

        #region Declarations
        private ObservableCollection<APMActionPlanTask> responsibleListForEmailNotification;
        private Int64 idActionPlan;
        private Int32 idActionPlanResponsible;
        //string tempFolderPathNewTaskEmailNotificationAttcahmentExcel = @"C:\Users\shweta.thube\Downloads\ETM AP format_v2.xlsx";
        string tempFolderPathNewTaskEmailNotificationAttcahment = @"C:\temp\ETM_OpenTasks_pdfFile\";
        private APMActionPlan actionPlanInfo;
        private string fileName;
        private string yearWeek;
        private APMActionPlanTask manualAttachmentSetting;
        private bool isSendAllEnabled;
        private string toEmail;
        private System.Timers.Timer _mailLifeTimer;
        public APMActionPlanTask selectedEmail;
        #endregion

        #region  public Properties
        public ObservableCollection<APMActionPlanTask> ResponsibleListForEmailNotification
        {
            get { return responsibleListForEmailNotification; }
            set
            {
                responsibleListForEmailNotification = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResponsibleListForEmailNotification"));
            }
        }
        public Int64 IdActionPlan
        {
            get { return idActionPlan; }
            set
            {
                idActionPlan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdActionPlan"));
            }
        }

        public Int32 IdActionPlanResponsible
        {
            get { return idActionPlanResponsible; }
            set
            {
                idActionPlanResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdActionPlanResponsible"));
            }
        }
        //[shweta.thube][GEOS2-8069]
        public APMActionPlan ActionPlanInfo
        {
            get { return actionPlanInfo; }
            set
            {
                actionPlanInfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanInfo"));
            }
        }
        //[shweta.thube][GEOS2-8069]
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }
        //[shweta.thube][GEOS2-8069]
        public string YearWeek
        {
            get { return yearWeek; }
            set
            {
                yearWeek = value;
                OnPropertyChanged(new PropertyChangedEventArgs("YearWeek"));
            }
        }
        //[shweta.thube][GEOS2-8069]
        public APMActionPlanTask ManualAttachmentSetting
        {
            get
            {
                return manualAttachmentSetting;
            }

            set
            {
                manualAttachmentSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ManualAttachmentSetting"));
            }
        }

        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        public bool IsSendAllEnabled
        {
            get { return isSendAllEnabled; }
            set
            {
                isSendAllEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSendAllEnabled"));
            }
        }
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        public string ToEmail
        {
            get { return toEmail; }
            set
            {
                toEmail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToEmail"));
            }
        }
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        public APMActionPlanTask SelectedEmail
        {
            get
            {
                return selectedEmail;
            }

            set
            {
                selectedEmail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmail"));
            }
        }
        #endregion

        #region ICommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand SendMailtoPersonCommand { get; set; }
        public ICommand SendAllButtonCommand { get; set; }//[Shweta.Thube][GEOS2-9129][09-10-2025]
        public ICommand SendAutomaticEmailCommand { get; set; }//[Shweta.Thube][GEOS2-9129][09-10-2025]

        #endregion

        #region Constructor
        public SendEmailNotificationViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendEmailNotificationViewModel ...", category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));

                SendMailtoPersonCommand = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                SendAutomaticEmailCommand = new DelegateCommand<object>(SendAutomaticEmailCommandAction);//[Shweta.Thube][GEOS2-9129][09-10-2025]
                SendAllButtonCommand = new DelegateCommand<object>(SendAllButtonCommandAction);//[Shweta.Thube][GEOS2-9129][09-10-2025]
                GeosApplication.Instance.Logger.Log("Constructor SendEmailNotificationViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendEmailNotificationViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init(Int64 idActionPlan, Int32 idActionPlanResponsible)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);

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
                IdActionPlan = idActionPlan;
                IdActionPlanResponsible = idActionPlanResponsible;
                FillResponsibleListForEmailNotification();
                FillManualAttachmentSetting();
                if (ResponsibleListForEmailNotification.Count > 1 &&
   ResponsibleListForEmailNotification.Count(x => x.IsPreviewEmail && x.SentEmail) > 1)
                {
                    IsSendAllEnabled = true;
                }
                else
                {
                    IsSendAllEnabled = false;
                }
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillResponsibleListForEmailNotification()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillResponsibleListForEmailNotification ...", category: Category.Info, priority: Priority.Low);
                string idPeriods = string.Empty;
                if (APMCommon.Instance.SelectedPeriod != null)
                {
                    List<long> selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
                    idPeriods = string.Join(",", selectedPeriod);
                }
                //ResponsibleListForEmailNotification = new ObservableCollection<APMActionPlanTask>(APMService.GetResponsibleListForEmailNotification_V2650(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser).OrderBy(ap => ap.OpenTaskCount));//[Shweta.Thube][GEOS2-6589]
                //APMService = new APMServiceController("localhost:6699");
                var responsibleList = new ObservableCollection<APMActionPlanTask>(APMService.GetResponsibleListForEmailNotification_V2680(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser).OrderBy(ap => ap.OpenTaskCount));//[Shweta.Thube][09-10-2025][GEOS2-9129]
                foreach (var item in responsibleList)
                {
                    bool hasEmail = !string.IsNullOrEmpty(item.CompanyEmail);
                    item.IsPreviewEmail = hasEmail;
                    item.SentEmail = hasEmail;
                }
                ResponsibleListForEmailNotification = new ObservableCollection<APMActionPlanTask>(responsibleList.OrderBy(ap => ap.OpenTaskCount));
                //ResponsibleListForEmailNotification = new ObservableCollection<APMActionPlanTask>(ResponsibleListForEmailNotification.OrderBy(ap => ap.OpenTaskCount));
                GeosApplication.Instance.Logger.Log("Method FillResponsibleListForEmailNotification() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleListForEmailNotification() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleListForEmailNotification() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleListForEmailNotification() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonCommandAction(object obj)
        {
            try
            {



                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            try
            {
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-8066]
        //[shweta.thube][GEOS2-8066]
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
                //[Shweta.Thube][GEOS2-9133][10-10-2025]
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
                APMActionPlanTask Temp = (APMActionPlanTask)obj;
                SelectedEmail = Temp;
                CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                Calendar calendar = cultureInfo.Calendar;
                CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
                DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
                int weekNumber = calendar.GetWeekOfYear(DateTime.Now, weekRule, firstDayOfWeek);
                if (Temp.EmailTaskDetailsList.Count() > 1 && Temp.EmailTaskDetailsList != null)
                {
                    FileName = "ETM_" + DateTime.Now.Year + "_CW" + weekNumber + "_New_Tasks";
                }
                else
                {
                    FileName = "ETM_" + DateTime.Now.Year + "_CW" + weekNumber + "_New_Task";
                }

                YearWeek = DateTime.Now.Year + "CW" + weekNumber;
                APMActionPlanTask ccPersonEmailID = APMService.GetCCPersonMail_V2650(IdActionPlanResponsible);
                APMActionPlanTask toPersonEmailID = APMService.GetToPersonMail_V2650(Temp.IdEmployee);

                ActionPlanInfo = APMService.GetActionPlanInfo_V2650(IdActionPlan);

                string pdfPath = GetNewTaskEmailNotificationAttcahment(Temp, ActionPlanInfo, FileName, YearWeek);
                StringBuilder htmlBody = PreparePlantHighPriorityOverdueAutomaticEmail(Temp);

                Outlook.Application outlookApp = new Outlook.Application();
                Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);

                //toPersonEmailID.EmployeeContactValue = "shweta.thube@Emdep.com";
                //ccPersonEmailID.EmployeeContactValue = " ";

                mailItem.To = toPersonEmailID.EmployeeContactValue;
                mailItem.CC = ccPersonEmailID.EmployeeContactValue;
                mailItem.Subject = FileName;
                mailItem.HTMLBody = htmlBody.ToString();
                Outlook.ItemEvents_10_Event mailEvents = (Outlook.ItemEvents_10_Event)mailItem;
                mailEvents.Send += MailItem_Send;
                mailEvents.Close += MailItem_Close;
                mailItem.Write += new Outlook.ItemEvents_10_WriteEventHandler(MailItem_Write);

                if (manualAttachmentSetting.SettingInUse == 1)
                {
                    mailItem.Attachments.Add(pdfPath);
                }
                List<GeosAppSetting> timeInterval = new List<GeosAppSetting>();
                timeInterval = WorkbenchStartUp.GetSelectedGeosAppSettings("163");
                int timeIntervalInMinutes = 0;
                if (timeInterval != null && timeInterval.Count > 0)
                {
                    timeIntervalInMinutes = Convert.ToInt32(timeInterval.FirstOrDefault().DefaultValue);
                }
                _mailLifeTimer = new System.Timers.Timer(timeIntervalInMinutes * 60 * 1000); // 10 minutes
                _mailLifeTimer.Elapsed += (s, e) =>
                {
                    try
                    {
                        // If user has not sent or closed the mail manually
                        if (mailItem != null)
                        {
                            // Save the mail as draft before closing
                            mailItem.Save();

                            // Close the email window, saving the draft
                            mailItem.Close(Outlook.OlInspectorClose.olSave);
                            mailItem = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Auto-save Outlook draft failed: " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    finally
                    {
                        _mailLifeTimer.Stop();
                        _mailLifeTimer.Dispose();
                        _mailLifeTimer = null;
                    }
                };
                _mailLifeTimer.Start();

                mailItem.Display(false);
                System.GC.KeepAlive(mailItem);
                System.GC.KeepAlive(outlookApp);
                System.GC.KeepAlive(mailEvents);
                if (File.Exists(pdfPath))
                {
                    File.Delete(pdfPath);
                }

                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[shweta.thube][GEOS2-8066]
        private StringBuilder PreparePlantHighPriorityOverdueAutomaticEmail(APMActionPlanTask temp)
        {
            StringBuilder emailBody = new StringBuilder();
            StringBuilder emailbodyStr = new StringBuilder();
            try
            {
                //APMService = new APMServiceController("localhost:6699");
                string text = APMService.ReadOverdueMailTemplate("SendEmailNotificationTasksTemplate.html");
                text = text.Replace("[OpenTaskCount]", temp.OpenTaskCount);
                //[Shweta.Thube][GEOS2-9618][29-10-2025]
                if (Convert.ToInt32(temp.OpenSubtaskCount) == 0)
                {
                    text = text.Replace("[OpenSubtaskCount]", " ");
                    if (temp.OpenTaskCount != "1")
                    {
                        text = text.Replace("[newTask]", "New Tasks ");
                    }
                    else
                    {
                        text = text.Replace("[newTask]", "New Task ");
                    }
                }
                else
                {
                    if (temp.OpenTaskCount != "1")
                    {
                        text = text.Replace("[newTask]", "New Tasks and");
                    }
                    else
                    {
                        text = text.Replace("[newTask]", "New Task and");
                    }
                }
                text = text.Replace("[OpenSubtaskCount]", temp.OpenSubtaskCount);
                text = text.Replace("[FullName]", temp.Responsible);

                if (Convert.ToInt32(temp.OpenSubtaskCount) > 1)
                {
                    text = text.Replace("[newSubTask]", "New Sub-Tasks");
                }
                else if (Convert.ToInt32(temp.OpenSubtaskCount) == 1)
                {
                    text = text.Replace("[newSubTask]", "New Sub-Task");
                }
                else
                {
                    text = text.Replace("[newSubTask]", "");
                }

                if (temp.EmailTaskDetailsList != null && temp.EmailTaskDetailsList.Count > 0)
                {   //[Shweta.Thube][GEOS2-9618][29-10-2025]
                    temp.EmailTaskDetailsList = temp.EmailTaskDetailsList.OrderByDescending(x => x.DueDays).ThenBy(x => x.IdLookupPriority).ToList();

                    // Add the table
                    emailbodyStr.Append("<table cellpadding='6' border='1' cellspacing='0' style='border-collapse: collapse; text-align:left; font-family: Arial; width:100%;'>");
                    emailbodyStr.Append("<tr>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:7%;'>AP Code</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:6%;'>Item No</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px;'>Title</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:4%;'>Priority</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:7%;'>Due Date</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:7%;'>Due Days</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px;'>Delegated to</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px;'>Created By</th>");
                    emailbodyStr.Append("</tr>");
                    foreach (APMActionPlanTask item in temp.EmailTaskDetailsList)
                    {
                        var rowStyle = item.DueDays > 5 ? " style='background-color: red;'" : "";
                        emailbodyStr.Append("<tr" + rowStyle + ">");
                        emailbodyStr.Append("<td style='padding: 4px; text-align:left;'>" + item.Code + "</td>");
                        emailbodyStr.Append("<td style='padding: 4px;text-align:Center;'>" + item.TaskNumberLable + "</td>");//[Shweta.Thube][GEOS2-9618][29-10-2025]
                        emailbodyStr.Append("<td style='padding: 4px; text-align:left; word-wrap: break-word; white-space: normal;'>" + item.Title + "</td>");
                        emailbodyStr.Append("<td style='padding: 4px; text-align: center;'>"); emailbodyStr.Append("<!--[if mso]>"); emailbodyStr.Append("<v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' style='width:20px;height:20px;v-text-anchor:middle;' arcsize='100%' strokecolor='black' fillcolor='" + item.PriorityHTMLColor + "'>"); emailbodyStr.Append("<w:anchorlock/>"); emailbodyStr.Append("<center style='color:#000000;font-family:Arial,sans-serif;font-size:13px;'></center>"); emailbodyStr.Append("</v:roundrect>"); emailbodyStr.Append("<![endif]-->"); emailbodyStr.Append("<![if !mso]>"); emailbodyStr.Append("<div style='display:inline-block;width:20px;height:20px;border:2px solid black;border-radius:50%;background-color:" + item.PriorityHTMLColor + ";'></div>"); emailbodyStr.Append("<![endif]>");
                        emailbodyStr.Append("</td>");
                        emailbodyStr.Append("<td style='padding: 4px; text-align:left;'>" + item.DueDate.Date.ToString("dd/MM/yyyy") + "</td>");
                        emailbodyStr.Append("<td style='padding: 4px;text-align:Center;'>" + item.DueDays + "</td>");
                        emailbodyStr.Append("<td style='padding: 4px; text-align:left; word-wrap: break-word; white-space: normal;'>" + item.DelegatedTo + "</td>");
                        emailbodyStr.Append("<td style='padding: 4px; text-align:left; word-wrap: break-word; white-space: normal;'>" + item.CreatedByName + "</td>");
                        emailbodyStr.Append("</tr>");
                    }
                    emailbodyStr.Append("</table>");
                    text = text.Replace("[Dynamic Data]", emailbodyStr.ToString());

                }
                else
                {
                    // Add the table
                    emailbodyStr.Append("<table cellpadding='6' border='1' cellspacing='0' style='border-collapse: collapse; text-align:left; font-family: Arial; width:100%;'>");
                    emailbodyStr.Append("<tr>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:7%;'>AP Code</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:6%;'>Item No</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px;'>Title</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:4%;'>Priority</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:7%;'>Status</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px; width:7%;'>DueDate</th>");
                    emailbodyStr.Append("<th style='background-color: #bdbcbc; text-align:left; padding: 6px;'>Responsible</th>");
                    emailbodyStr.Append("</tr>");

                    emailbodyStr.Append("</table>");
                    text = text.Replace("[Dynamic Data]", emailbodyStr.ToString());
                }

                emailBody.Append(text);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PreparePlantHighPriorityOverdueAutomaticEmail() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return emailBody;
        }
        //[shweta.thube][GEOS2-8069]
        //[shweta.thube][GEOS2-8069]
        public string GetNewTaskEmailNotificationAttcahment(APMActionPlanTask temp, APMActionPlan ActionPlanInfo, string FileName, string yearWeek)
        {
            try
            {
                // Load the Excel template as byte array
                byte[] excelBytes = APMService.GetActionPlanExcel();
                if (excelBytes == null || excelBytes.Length == 0)
                    return null;

                // Load workbook from byte array
                using (MemoryStream stream = new MemoryStream(excelBytes))
                {
                    Workbook workbook = new Workbook();
                    workbook.LoadDocument(stream, DocumentFormat.OpenXml); // Assuming .xlsm file

                    Worksheet sheet = workbook.Worksheets[0];
                    sheet.Name = "ETM_" + ActionPlanInfo.Code;

                    // Fill header data
                    sheet.Cells["B7"].Value = ActionPlanInfo.Code;
                    sheet.Cells["C7"].Value = ActionPlanInfo.Location;
                    sheet.Cells["D7"].Value = ActionPlanInfo.Description;
                    sheet.Cells["E7"].Value = ActionPlanInfo.Origin;
                    sheet.Cells["F7"].Value = ActionPlanInfo.BusinessUnit;
                    sheet.Cells["G7"].Value = ActionPlanInfo.Department;
                    sheet.Cells["I7"].Value = ActionPlanInfo.ActionPlanResponsibleDisplayName;
                    sheet.Cells["K7"].Value = ActionPlanInfo.Group;
                    sheet.Cells["L7"].Value = ActionPlanInfo.Site;
                    sheet.Cells["N7"].Value = ActionPlanInfo.CreatedIn;
                    sheet.Cells["N3"].Value = yearWeek;
                    sheet.Cells["N2"].Value = DateTime.Now;

                    // Fill task list starting from row 9
                    int startRow = 9;
                    foreach (var task in temp.EmailTaskDetailsList)
                    {
                        sheet.Cells[startRow, 1].Value = task.TaskNumberLable; //[Shweta.Thube][GEOS2-9618][29-10-2025]
                        sheet.Cells[startRow, 2].Value = task.Title;
                        sheet.Cells[startRow, 3].Value = task.Description;
                        sheet.Cells[startRow, 4].Value = task.Responsible;
                        sheet.Cells[startRow, 5].Value = task.TaskLastComment;
                        sheet.Cells[startRow, 6].Value = task.Theme;
                        sheet.Cells[startRow, 7].Value = task.Priority;
                        sheet.Cells[startRow, 8].Value = task.Status;
                        sheet.Cells[startRow, 9].Value = task.OpenDate?.ToShortDateString();
                        sheet.Cells[startRow, 10].Value = task.LastUpdated?.ToShortDateString();
                        sheet.Cells[startRow, 11].Value = task.DueDate.ToShortDateString();
                        sheet.Cells[startRow, 12].Value = task.Progress + "%";
                        sheet.Cells[startRow, 13].Value = task.CodeNumber;

                        for (int col = 2; col <= 5; col++)
                        {
                            var cell = sheet.Cells[startRow, col];
                            cell.Alignment.WrapText = true;
                        }

                        sheet.Rows[startRow].AutoFitRows();

                        startRow++;
                    }

                    if (!Directory.Exists(tempFolderPathNewTaskEmailNotificationAttcahment))
                    {
                        Directory.CreateDirectory(tempFolderPathNewTaskEmailNotificationAttcahment);
                    }
                    string pdfPath = Path.Combine(tempFolderPathNewTaskEmailNotificationAttcahment, FileName + ".pdf");

                    if (File.Exists(pdfPath))
                    {
                        File.Delete(pdfPath);
                    }

                    if (workbook.Worksheets.Count > 0 && workbook.Worksheets[0] != null)
                    {
                        workbook.ExportToPdf(pdfPath);
                    }
                    //workbook.ExportToPdf(pdfPath);

                    return pdfPath;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(
                    "Error in GetNewTaskEmailNotificationAttcahment(): " + ex.Message,
                    category: Category.Exception,
                    priority: Priority.Low
                );
                return null;
            }
        }

        //[shweta.thube][GEOS2-8069]
        private void FillManualAttachmentSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillManualAttachmentSetting ...", category: Category.Info, priority: Priority.Low);

                ManualAttachmentSetting = APMService.GetManualAttachmentSetting(156);

                GeosApplication.Instance.Logger.Log("Method FillManualAttachmentSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillManualAttachmentSetting() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillManualAttachmentSetting() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillManualAttachmentSetting() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        private void MailItem_Send(ref bool Cancel)
        {
            //APMService = new APMServiceController("localhost:6699");
            APMService.UpdateSendDateTime_V2680(SelectedEmail);

            var index = ResponsibleListForEmailNotification.ToList().FindIndex(x => x.IdEmployee == SelectedEmail.IdEmployee);
            if (index >= 0)
            {
                var updated = ResponsibleListForEmailNotification[index];
                updated.SentDateTime = DateTime.Now;

                // Force UI refresh
                ResponsibleListForEmailNotification[index] = updated;
            }
        }

        private void MailItem_Close(ref bool Cancel)
        {

        }

        private void MailItem_Write(ref bool Cancel)
        {

        }
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        public void SendAutomaticEmailCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendAutomaticEmailCommandAction ...", category: Category.Info, priority: Priority.Low);
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

                APMActionPlanTask Temp = (APMActionPlanTask)obj;

                CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                Calendar calendar = cultureInfo.Calendar;
                CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
                DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
                int weekNumber = calendar.GetWeekOfYear(DateTime.Now, weekRule, firstDayOfWeek);
                if (Temp.EmailTaskDetailsList.Count() > 1 && Temp.EmailTaskDetailsList != null)
                {
                    FileName = "ETM_" + DateTime.Now.Year + "_CW" + weekNumber + "_New_Tasks";
                }
                else
                {
                    FileName = "ETM_" + DateTime.Now.Year + "_CW" + weekNumber + "_New_Task";
                }

                YearWeek = DateTime.Now.Year + "CW" + weekNumber;
                APMActionPlanTask ccPersonEmailID = APMService.GetCCPersonMail_V2650(IdActionPlanResponsible);
                APMActionPlanTask toPersonEmailID = APMService.GetToPersonMail_V2650(Temp.IdEmployee);

                ActionPlanInfo = APMService.GetActionPlanInfo_V2650(IdActionPlan);

                string pdfPath = GetNewTaskEmailNotificationAttcahment(Temp, ActionPlanInfo, FileName, YearWeek);
                StringBuilder htmlBody = PreparePlantHighPriorityOverdueAutomaticEmail(Temp);

                //Outlook.Application outlookApp = new Outlook.Application();
                //Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);

                string To = toPersonEmailID.EmployeeContactValue;
                string CC = ccPersonEmailID.EmployeeContactValue;
                byte[] attachmentData = null;
                //To = "shweta.thube@emdep.com";
                //CC = "";
                Dictionary<string, byte[]> Attachment = new Dictionary<string, byte[]>();
                if (manualAttachmentSetting.SettingInUse == 1)
                {
                    attachmentData = File.ReadAllBytes(pdfPath);
                    Attachment.Add(FileName + ".pdf", attachmentData);
                }
                HashSet<string> distinctEmails = new HashSet<string>();

                if (!string.IsNullOrWhiteSpace(CC))
                {
                    distinctEmails.Add(CC.Trim());
                }
                //APMService = new APMServiceController("localhost:6699");                              
                //ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //bool IsEmailSend = SRMService.SendEmailForPO(To, FileName, htmlBody.ToString(), Attachment, "ETM-noreply@emdep.com", distinctEmails.ToList());
                bool IsEmailSend = APMService.APMEmailSend_V2680(To, FileName, htmlBody.ToString(), Attachment, "ETM-noreply@emdep.com", distinctEmails.ToList());

                if (IsEmailSend)
                {
                    //APMService = new APMServiceController("localhost:6699");
                    APMService.UpdateSendDateTime_V2680(Temp);

                    var index = ResponsibleListForEmailNotification.ToList().FindIndex(x => x.IdEmployee == Temp.IdEmployee);
                    if (index >= 0)
                    {
                        var updated = ResponsibleListForEmailNotification[index];
                        updated.SentDateTime = DateTime.Now;

                        // Force UI refresh
                        ResponsibleListForEmailNotification[index] = updated;
                    }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SendMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }


                if (File.Exists(pdfPath))
                {
                    File.Delete(pdfPath);
                }
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Method SendAutomaticEmailCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendAutomaticEmailCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendAutomaticEmailCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendAutomaticEmailCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Shweta.Thube][GEOS2-9133][10-10-2025]
        public void SendAllButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendAllButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
                //APMActionPlanTask Temp = (APMActionPlanTask)obj;
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
                bool IsEmailSend = false;
                int EmailSentCount = 0;
                string EmailNotsent = string.Empty;
                ToEmail = string.Empty;
                int totalCount = 0;
                totalCount = ResponsibleListForEmailNotification.Count();
                var Email = new ObservableCollection<APMActionPlanTask>(ResponsibleListForEmailNotification.Where(x => string.IsNullOrEmpty(x.CompanyEmail)));
                EmailNotsent = string.Join(", ", Email.Select(x => x.Responsible));
                if (!string.IsNullOrEmpty(EmailNotsent))
                {
                    GeosApplication.Instance.Logger.Log($"Email does not exist for: {EmailNotsent}", category: Category.Info, priority: Priority.Low);
                }
                var tempList = new ObservableCollection<APMActionPlanTask>(ResponsibleListForEmailNotification.Where(x => !string.IsNullOrEmpty(x.CompanyEmail)));

                foreach (APMActionPlanTask item in tempList)
                {
                    try
                    {
                        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                        Calendar calendar = cultureInfo.Calendar;
                        CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
                        DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
                        int weekNumber = calendar.GetWeekOfYear(DateTime.Now, weekRule, firstDayOfWeek);

                        if (item.EmailTaskDetailsList.Count() > 1)
                        {
                            FileName = "ETM_" + DateTime.Now.Year + "_CW" + weekNumber + "_New_Tasks";
                        }
                        else
                        {
                            FileName = "ETM_" + DateTime.Now.Year + "_CW" + weekNumber + "_New_Task";
                        }
                        string yearWeek = DateTime.Now.Year + "CW" + weekNumber;

                        APMActionPlanTask ccPersonEmailID = APMService.GetCCPersonMail_V2650(IdActionPlanResponsible);
                        APMActionPlanTask toPersonEmailID = APMService.GetToPersonMail_V2650(item.IdEmployee);

                        ActionPlanInfo = APMService.GetActionPlanInfo_V2650(IdActionPlan);

                        string pdfPath = GetNewTaskEmailNotificationAttcahment(item, ActionPlanInfo, FileName, yearWeek);
                        StringBuilder htmlBody = PreparePlantHighPriorityOverdueAutomaticEmail(item);

                        string To = toPersonEmailID.EmployeeContactValue;
                        ToEmail = To;
                        string CC = ccPersonEmailID.EmployeeContactValue;
                        if (string.IsNullOrEmpty(CC))
                        {
                            GeosApplication.Instance.Logger.Log($"CC is null because email doesn't exist for Action Plan responsible", category: Category.Info, priority: Priority.Low);
                        }
                        byte[] attachmentData = null;
                        Dictionary<string, byte[]> Attachment = new Dictionary<string, byte[]>();
                        if (manualAttachmentSetting.SettingInUse == 1)
                        {
                            attachmentData = File.ReadAllBytes(pdfPath);
                            Attachment.Add(FileName + ".pdf", attachmentData);
                        }
                        //To = "shweta.thube@emdep.com";
                        //CC = "";
                        HashSet<string> distinctEmails = new HashSet<string>();

                        if (!string.IsNullOrWhiteSpace(CC))
                        {
                            distinctEmails.Add(CC.Trim());
                        }
                        //APMService = new APMServiceController("localhost:6699");                      
                        //ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        IsEmailSend = APMService.APMEmailSend_V2680(To, FileName, htmlBody.ToString(), Attachment, "ETM-noreply@emdep.com", distinctEmails.ToList());
                        //IsEmailSend = SRMService.SendEmailForPO(To, FileName, htmlBody.ToString(), Attachment, "ETM-noreply@emdep.com", distinctEmails.ToList());

                        if (IsEmailSend)
                        {
                            EmailSentCount++;
                            //APMService = new APMServiceController("localhost:6699");
                            APMService.UpdateSendDateTime_V2680(item);

                            var index = ResponsibleListForEmailNotification.ToList().FindIndex(x => x.IdEmployee == item.IdEmployee);
                            if (index >= 0)
                            {
                                var updated = ResponsibleListForEmailNotification[index];
                                updated.SentDateTime = DateTime.Now;

                                // Force UI refresh
                                ResponsibleListForEmailNotification[index] = updated;
                            }
                        }
                        else
                        {
                            // 👇 Add failed email to list
                            if (!string.IsNullOrWhiteSpace(To))
                            {
                                if (!string.IsNullOrEmpty(EmailNotsent))
                                    EmailNotsent += ", ";

                                EmailNotsent += To.Trim();
                            }
                        }
                        if (File.Exists(pdfPath))
                        {
                            File.Delete(pdfPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrWhiteSpace(ToEmail))
                        {
                            if (!string.IsNullOrEmpty(EmailNotsent))
                                EmailNotsent += ", ";

                            EmailNotsent += ToEmail.Trim();
                        }

                        GeosApplication.Instance.Logger.Log($"Error in parallel email send: {ex.Message}", Category.Exception, Priority.Low);
                    }
                }

                if (ResponsibleListForEmailNotification.Count == EmailSentCount)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("FinalEmailSentMessage").ToString(), EmailSentCount, totalCount), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    string FinalEmailSentMessage = string.Format(
                        System.Windows.Application.Current.FindResource("FinalEmailSentMessage").ToString(),
                        EmailSentCount, totalCount);

                    string FinalEmailSentNotMessage = string.Format(
                        System.Windows.Application.Current.FindResource("FinalEmailSentNotMessage").ToString(),
                        EmailNotsent);

                    string FinalEmailSentDetailsMessage = System.Windows.Application.Current.FindResource("FinalEmailSentDetailsMessage").ToString();

                    string finalMessage = $"{FinalEmailSentMessage}\n{FinalEmailSentNotMessage}\n{FinalEmailSentDetailsMessage}";

                    // Show in CustomMessageBox
                    CustomMessageBox.Show(
                        finalMessage,
                        Application.Current.Resources["PopUpSuccessColor"].ToString(),
                        CustomMessageBox.MessageImagePath.Ok,
                        MessageBoxButton.OK);

                }
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.Logger.Log("Method SendAllButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendAllButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendAllButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendAllButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validation

        #endregion

    }
}
