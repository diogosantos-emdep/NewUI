using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Drawing;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Utility;
using Prism.Logging;
using System.Threading;
using System.IO;
using Microsoft.Office.Interop.Outlook;
using Prism.Logging;
using Emdep.Geos.UI.CustomControls;
using System.Windows;

namespace GEOS_Workbench_Outlook_Addin
{
    public partial class RibbonToOutLook
    {
        ICrmService CrmStartUp;
        IWorkbenchStartUp WStartUp;

        private void CreateServiceInstances()
        {
            if (CrmStartUp == null)
                CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

            if (WStartUp == null)
                WStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        }

        private void RibbonToOutLook_Load(object sender, RibbonUIEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("RibbonToOutLook_Load()", category: Prism.Logging.Category.Info, priority: Priority.Low);
            if (GeosApplication.Instance.ActiveUser != null)
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 7))
                {
                    CRMMenu.Enabled = true;
                }
                else
                {
                    CRMMenu.Enabled = false;
                    //ExchangeUser exchangeUser = Application.Session.CurrentUser.AddressEntry.GetExchangeUser();
                    //CustomMessageBox.Show(string.Format("The Outlook account FirstName+LastName does not have permission to use CRM module.", exchangeUser.Name), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }
            else
            {
                CRMMenu.Enabled = false;
                GeosApplication.Instance.Logger.Log("RibbonToOutLook_Load(): CRMMenu Disabled...", category: Prism.Logging.Category.Info, priority: Priority.Low);
            }
        }

        private void NewOpportunity_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                CreateServiceInstances();
                GeosApplication.Instance.IdCurrencyByRegion = 1;

                MailItem emailItem = null;
                foreach (MailItem email in new Microsoft.Office.Interop.Outlook.Application().ActiveExplorer().Selection)
                {
                    emailItem = email as MailItem;
                    break;
                }

                if (emailItem == null)
                {
                    GeosApplication.Instance.Logger.Log("Error in NewOpportunity_Click() Method - Mail is not selected", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                    return;
                }

                LeadAddViewModel leadAddViewModel = new LeadAddViewModel();
                LeadsAddView leadsAddView = new LeadsAddView();
                string senderEmail = getSenderEmailAddress(emailItem);
                leadAddViewModel.InitAddin(emailItem.Subject, senderEmail);
                leadAddViewModel.RFQReceptionDate = emailItem.ReceivedTime;

                EventHandler handle = delegate { leadsAddView.Close(); };
                leadAddViewModel.RequestClose += handle;
                leadsAddView.DataContext = leadAddViewModel;

                leadsAddView.ShowDialogWindow();

                if (leadAddViewModel.OfferData != null)
                {
                    string tempFolderPath = Path.GetTempPath() + @"Emdep\Mail\";

                    if (!Directory.Exists(tempFolderPath))
                    {
                        Directory.CreateDirectory(tempFolderPath);
                    }

                    tempFolderPath += emailItem.Subject.Trim('/', '\\') + ".msg";
                    emailItem.SaveAs(tempFolderPath);

                    FileDetail fileDetail = new FileDetail();
                    fileDetail.FileName = emailItem.Subject.Trim('/', '\\') + ".msg";
                    fileDetail.FileByte = System.IO.File.ReadAllBytes(tempFolderPath);
                    Offer offer = new Offer();
                    offer.Code = leadAddViewModel.OfferData.Code;
                    offer.Year = DateTime.Now.Year;
                    offer.Site = new Company { Name = leadAddViewModel.OfferData.Site.FullName };

                    CrmStartUp.SaveCopyOfEMail(offer, fileDetail);

                    if (File.Exists(tempFolderPath))
                        File.Delete(tempFolderPath);
                }
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in NewOpportunity_Click() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }
        }

        private string getSenderEmailAddress(MailItem mail)
        {
            AddressEntry sender = mail.Sender;
            string SenderEmailAddress = "";

            if (sender.AddressEntryUserType == OlAddressEntryUserType.olExchangeUserAddressEntry || sender.AddressEntryUserType == OlAddressEntryUserType.olExchangeRemoteUserAddressEntry)
            {
                ExchangeUser exchUser = sender.GetExchangeUser();
                if (exchUser != null)
                {
                    SenderEmailAddress = exchUser.PrimarySmtpAddress;
                }
            }
            else
            {
                SenderEmailAddress = mail.SenderEmailAddress;
            }

            return SenderEmailAddress;
        }

        private void NewAppointment_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                CreateServiceInstances();

                MailItem emailItem = null;
                foreach (MailItem email in new Microsoft.Office.Interop.Outlook.Application().ActiveExplorer().Selection)
                {
                    emailItem = email as MailItem;
                    break;
                }

                if (emailItem == null)
                {
                    GeosApplication.Instance.Logger.Log("Error in NewAppointment_Click() Method - Mail is not selected", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                    return;     //error
                }

                string tempFolderPath = Path.GetTempPath() + @"Emdep\Mail\";

                if (!Directory.Exists(tempFolderPath))
                {
                    Directory.CreateDirectory(tempFolderPath);
                }

                tempFolderPath += emailItem.Subject.Trim('/', '\\') + ".msg";
                emailItem.SaveAs(tempFolderPath);
                string senderEmail = getSenderEmailAddress(emailItem);

                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                AddActivityView addActivityView = new AddActivityView();

                addActivityViewModel.InitAppointment(senderEmail); // (emailItem);
                addActivityViewModel.Subject = emailItem.Subject;
                addActivityViewModel.DueDate = emailItem.ReceivedTime;
                addActivityViewModel.IsCompleted = true;
                addActivityViewModel.AddAttachement(tempFolderPath);

                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                addActivityView.ShowDialog();

                if (File.Exists(tempFolderPath))
                    File.Delete(tempFolderPath);
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in NewAppointment_Click() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }
        }

        private void NewCall_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                CreateServiceInstances();

                MailItem emailItem = null;
                foreach (MailItem email in new Microsoft.Office.Interop.Outlook.Application().ActiveExplorer().Selection)
                {
                    emailItem = email as MailItem;
                    break;
                }

                if (emailItem == null)
                {
                    GeosApplication.Instance.Logger.Log("Error in NewCall_Click() Method - Mail is not selected", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                    return;
                }

                string tempFolderPath = Path.GetTempPath() + @"Emdep\Mail\";

                if (!Directory.Exists(tempFolderPath))
                {
                    Directory.CreateDirectory(tempFolderPath);
                }

                tempFolderPath += emailItem.Subject.Trim('/', '\\') + ".msg";
                emailItem.SaveAs(tempFolderPath);

                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                AddActivityView addActivityView = new AddActivityView();

                string senderEmail = getSenderEmailAddress(emailItem);
                addActivityViewModel.InitCall(senderEmail);
                addActivityViewModel.Subject = emailItem.Subject;
                addActivityViewModel.DueDate = emailItem.ReceivedTime;
                addActivityViewModel.AddAttachement(tempFolderPath);
                addActivityViewModel.IsCompleted = true;
                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                addActivityView.ShowDialog();

                if (File.Exists(tempFolderPath))
                    File.Delete(tempFolderPath);
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in NewCall_Click() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }
        }

        private void NewEmail_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                CreateServiceInstances();
                GeosApplication.Instance.IdCurrencyByRegion = 1;

                MailItem emailItem = null;
                foreach (MailItem email in new Microsoft.Office.Interop.Outlook.Application().ActiveExplorer().Selection)
                {
                    emailItem = email as MailItem;
                    break;
                }

                if (emailItem == null)
                {
                    return;     //error
                }

                string tempFolderPath = Path.GetTempPath() + @"Emdep\Mail\";
                if (!Directory.Exists(tempFolderPath))
                {
                    Directory.CreateDirectory(tempFolderPath);
                }

                tempFolderPath += emailItem.Subject.Trim('/', '\\') + ".msg";
                emailItem.SaveAs(tempFolderPath);

                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                AddActivityView addActivityView = new AddActivityView();

                string senderEmail = getSenderEmailAddress(emailItem);
                addActivityViewModel.InitEmail(senderEmail);
                addActivityViewModel.Subject = emailItem.Subject;
                addActivityViewModel.DueDate = emailItem.ReceivedTime;
                addActivityViewModel.IsCompleted = true;
                addActivityViewModel.AddAttachement(tempFolderPath);

                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                addActivityView.ShowDialog();

                if (File.Exists(tempFolderPath))
                    File.Delete(tempFolderPath);
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in NewEmail_Click() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }
        }

        private void NewTask_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                CreateServiceInstances();
                GeosApplication.Instance.IdCurrencyByRegion = 1;

                MailItem emailItem = null;
                foreach (MailItem email in new Microsoft.Office.Interop.Outlook.Application().ActiveExplorer().Selection)
                {
                    emailItem = email as MailItem;
                    break;
                }

                if (emailItem == null)
                {
                    GeosApplication.Instance.Logger.Log("Error in NewTask_Click() Method - Mail is not selected", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                    return;     //error
                }

                string tempFolderPath = Path.GetTempPath() + @"Emdep\Mail\";

                if (!Directory.Exists(tempFolderPath))
                {
                    Directory.CreateDirectory(tempFolderPath);
                }

                tempFolderPath += emailItem.Subject.Trim('/', '\\') + ".msg";
                emailItem.SaveAs(tempFolderPath);

                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                AddActivityView addActivityView = new AddActivityView();

                string senderEmail = getSenderEmailAddress(emailItem);
                addActivityViewModel.InitTask(senderEmail);
                addActivityViewModel.Subject = emailItem.Subject;
                addActivityViewModel.DueDate = emailItem.ReceivedTime;
                addActivityViewModel.IsCompleted = true;
                addActivityViewModel.AddAttachement(tempFolderPath);

                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                addActivityView.ShowDialog();

                if (File.Exists(tempFolderPath))
                    File.Delete(tempFolderPath);
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in NewTask_Click() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }
        }

        private void AttachToActivity_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                CreateServiceInstances();
                //GeosApplication.Instance.IdCurrencyByRegion = 1;

                MailItem emailItem = null;
                foreach (MailItem email in new Microsoft.Office.Interop.Outlook.Application().ActiveExplorer().Selection)
                {
                    emailItem = email as MailItem;
                    break;
                }
                if (emailItem == null)
                {
                    GeosApplication.Instance.Logger.Log("Error in AttachToActivity_Click() Method - Mail is not selected", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                    return;     //error
                }

                string tempFolderPath = Path.GetTempPath() + @"Emdep\Mail\";

                if (!Directory.Exists(tempFolderPath))
                {
                    Directory.CreateDirectory(tempFolderPath);
                }

                tempFolderPath += emailItem.Subject.Trim('/', '\\') + ".msg";
                emailItem.SaveAs(tempFolderPath);

                AttachToActivityViewModel attachToActivityViewModel = new AttachToActivityViewModel(emailItem.Subject, tempFolderPath);
                AttachToActivityView attachToActivityView = new AttachToActivityView();
                EventHandler handle = delegate { attachToActivityView.Close(); };
                attachToActivityViewModel.RequestClose += handle;
                attachToActivityView.DataContext = attachToActivityViewModel;
                attachToActivityView.ShowDialogWindow();

                //if (attachToActivityViewModel.IsAccept == true)
                //{
                //    if (emailItem != null)
                //    {
                //        string fileName = @"D:/" + emailItem.Subject + ".msg";// Path.GetTempFileName();
                //        emailItem.SaveAs(fileName, Microsoft.Office.Interop.Outlook.OlSaveAsType.olMSG);
                //    }
                //}

                if (File.Exists(tempFolderPath))
                    File.Delete(tempFolderPath);
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AttachToActivity_Click() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }
        }

        private void AttachToOpportunity_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                CreateServiceInstances();

                MailItem emailItem = null;
                foreach (MailItem email in new Microsoft.Office.Interop.Outlook.Application().ActiveExplorer().Selection)
                {
                    emailItem = email as MailItem;
                    break;
                }

                if (emailItem == null)
                {
                    GeosApplication.Instance.Logger.Log("Error in AttachToOpportunity_Click() Method - Mail is not selected", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                    return;  //error
                }

                string tempFolderPath = Path.GetTempPath() + @"Emdep\Mail\";

                if (!Directory.Exists(tempFolderPath))
                {
                    Directory.CreateDirectory(tempFolderPath);
                }

                tempFolderPath += emailItem.Subject.Trim('/', '\\') + ".msg";
                emailItem.SaveAs(tempFolderPath);

                AttachToOpportunityViewModel attachToOpportunityViewModel = new AttachToOpportunityViewModel(emailItem.Subject, tempFolderPath);
                AttachToOpportunityView attachToOpportunityView = new AttachToOpportunityView();
                EventHandler handle = delegate { attachToOpportunityView.Close(); };
                attachToOpportunityViewModel.RequestClose += handle;
                attachToOpportunityView.DataContext = attachToOpportunityViewModel;
                attachToOpportunityView.ShowDialogWindow();

                if (File.Exists(tempFolderPath))
                    File.Delete(tempFolderPath);
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AttachToOpportunity_Click() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }
        }

        private void NewAction_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                CreateServiceInstances();

                MailItem emailItem = null;
                foreach (MailItem email in new Microsoft.Office.Interop.Outlook.Application().ActiveExplorer().Selection)
                {
                    emailItem = email as MailItem;
                    break;
                }

                if (emailItem == null)
                {
                    GeosApplication.Instance.Logger.Log("Error in NewAction_Click() Method - Mail is not selected", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                    return;  //error
                }

                //string tempFolderPath = Path.GetTempPath() + @"Emdep\Mail\";

                //if (!Directory.Exists(tempFolderPath))
                //{
                //    Directory.CreateDirectory(tempFolderPath);
                //}

                //tempFolderPath += emailItem.Subject.Trim('/', '\\') + ".msg";
                //emailItem.SaveAs(tempFolderPath);

                AddNewActionsViewModel addActivityViewModel = new AddNewActionsViewModel();
                AddNewActionsView addNewActionsView = new AddNewActionsView();

                addActivityViewModel.Subject = emailItem.Subject;
                addActivityViewModel.DueDate = emailItem.ReceivedTime;
                string senderEmail = getSenderEmailAddress(emailItem);
                addActivityViewModel.InitAddin(senderEmail);
                //addActivityViewModel.AddAttachement(tempFolderPath);

                EventHandler handle = delegate { addNewActionsView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addNewActionsView.DataContext = addActivityViewModel;
                addNewActionsView.ShowDialog();
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in NewAction_Click() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for check application setting and and create user setting if not created.
        /// Set language dictionary as per user setting.
        /// [001][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        /// </summary>
        //public void GetApplicationSettings()
        //{
        //    GeosApplication.Instance.Logger.Log("Get user setting ", category: Category.Info, priority: Priority.Low);

        //    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
        //    //string pathUserSetting = GeosApplication.Instance.UserSettingFilePath;

        //    List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
        //    if (GeosApplication.Instance.UserSettings.Count == 0)
        //    {
        //        lstUserConfiguration.Add(new Tuple<string, string>("ThemeName", "BlackAndBlue"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("Language", Thread.CurrentThread.CurrentCulture.Name.ToString()));
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedModule", string.Empty));
        //        lstUserConfiguration.Add(new Tuple<string, string>("NotificationPageCount", "10"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("IsServiceIconShow", "false"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("ServiceRefreshSeconds", "20"));

        //        lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferPeriod", DateTime.Now.Year.ToString()));
        //        lstUserConfiguration.Add(new Tuple<string, string>("CustomPeriodOption", "0"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferFromInterval", DateTime.Now.ToShortDateString()));
        //        lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferToInterval", DateTime.Now.ToShortDateString()));

        //        lstUserConfiguration.Add(new Tuple<string, string>("CrmTopOffers", "10"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("CrmTopCustomers", "10"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("UserSessionDetail", string.Empty));

        //        var regionCulture = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency", regionCulture.ISOCurrencySymbol));
        //        lstUserConfiguration.Add(new Tuple<string, string>("AutoRefresh", "Yes"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("CurrentProfile", ""));
        //        lstUserConfiguration.Add(new Tuple<string, string>("LoadDataOn", ""));
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedCRMSectionLoadData", ""));

        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedwarehouseId", "0"));
        //        PrinterSettings settings = new PrinterSettings();
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedPrinter", settings.PrinterName));
        //        lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterModel", string.Empty));
        //        lstUserConfiguration.Add(new Tuple<string, string>("ParallelPort", string.Empty));

        //        lstUserConfiguration.Add(new Tuple<string, string>("PickingTimer", false.ToString()));
        //        lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceDataSourceSelectedIndex", "0"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceTextSeparator", "0"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceOdbcDns", "0"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceOdbcTableName", "0"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceSourceFieldSelectedIndex", ""));
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedScaleModel", ""));
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedPort", ""));
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedParity", ""));
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedStopBit", ""));
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedBaudRate", ""));
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedDataBit", ""));
        //        //[001] added
        //        lstUserConfiguration.Add(new Tuple<string, string>("Appearance", ""));
        //        //PCM_Appearence
        //        lstUserConfiguration.Add(new Tuple<string, string>("PCM_Appearance", ""));

        //        //CRM Shortcuts
        //        lstUserConfiguration.Add(new Tuple<string, string>("Opportunity", "Shift + O"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("Contact", "Shift + C"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("Account", "Shift + A"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("Appointment", "Shift + P"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("Call", "Shift + L"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("Task", "Shift + T"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("Email", "Shift + E"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("Action", "Shift + S"));
        //        lstUserConfiguration.Add(new Tuple<string, string>("SearchOpportunityOrOrder", "Ctrl + O"));


        //        var regionCulture_warehouse = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
        //        lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency_Warehouse", regionCulture_warehouse.ISOCurrencySymbol));


        //        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
        //        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
        //        GeosApplication.Instance.CrmOfferYear = DateTime.Now.Year;
        //        GeosApplication.Instance.CrmTopCustomers = 10;
        //        GeosApplication.Instance.CrmTopOffers = 10;
        //        //For Printer setting.
        //        GeosApplication.Instance.SelectedPrinter = settings.PrinterName;
        //    }
        //    else
        //    {
        //        CreateUserSettingFileAfterVarification();

        //        GeosApplication.Instance.CrmOfferYear = Convert.ToInt64(GeosApplication.Instance.UserSettings["CrmOfferPeriod"].ToString());
        //        GeosApplication.Instance.CrmTopCustomers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["CrmTopCustomers"].ToString());
        //        GeosApplication.Instance.CrmTopOffers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["CrmTopOffers"].ToString());
        //        GeosApplication.Instance.SelectedPrinter = GeosApplication.Instance.UserSettings["SelectedPrinter"].ToString();
        //        GeosApplication.Instance.LabelPrinter = GeosApplication.Instance.UserSettings["LabelPrinter"];
        //        GeosApplication.Instance.LabelPrinterModel = GeosApplication.Instance.UserSettings["LabelPrinterModel"];
        //        GeosApplication.Instance.ParallelPort = GeosApplication.Instance.UserSettings["ParallelPort"];
        //    }

        //    GeosApplication.Instance.Logger.Log("Get user setting successfully.", category: Category.Info, priority: Priority.Low);

        //    SetLanguageDictionary();
        //    //FillTheme();

        //    GeosApplication.Instance.Logger.Log("Get application setting ", category: Category.Info, priority: Priority.Low);
        //    // GeosApplication.Instance.ApplicationSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.ApplicationSettingFilePath);
        //    string Appfilepath = GeosApplication.Instance.ApplicationSettingFilePath;// application setting file path 

        //    if (!File.Exists(GeosApplication.Instance.ApplicationSettingFilePath))
        //    {
        //        if (!Directory.Exists(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName))
        //            Directory.CreateDirectory(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName);
        //        File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationSettingFileName, Appfilepath);
        //        GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);

        //        //GeosServiceProviderList = new List<GeosServiceProvider>();
        //        //GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;

        //        //UserConfigurationView objUserConfigurationView = new UserConfigurationView();

        //        //UserConfigurationViewModel UserConfigurationViewModel = new UserConfigurationViewModel();
        //        //// UserConfigurationViewModel.ListGeosServiceProviders  = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(serviceProviderName => serviceProviderName.Name).ToList());
        //        //EventHandler handle = delegate { objUserConfigurationView.Close(); };
        //        //UserConfigurationViewModel.RequestClose += handle;
        //        //objUserConfigurationView.DataContext = UserConfigurationViewModel;
        //        //objUserConfigurationView.ShowDialogWindow();

        //    }
        //    else
        //    {
        //        GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);
        //        GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;
        //        GeosApplication.Instance.ServicePath = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl).FirstOrDefault();

        //        if (GeosApplication.Instance.ServicePath == null)
        //        {
        //            //    UserConfigurationView objUserConfigurationView = new UserConfigurationView();
        //            //    UserConfigurationViewModel UserConfigurationViewModel = new UserConfigurationViewModel();
        //            //    EventHandler handle = delegate { objUserConfigurationView.Close(); };
        //            //    UserConfigurationViewModel.RequestClose += handle;
        //            //    objUserConfigurationView.DataContext = UserConfigurationViewModel;
        //            //    objUserConfigurationView.ShowDialogWindow();
        //            //    GeosApplication.Instance.ServicePath = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl).FirstOrDefault();
        //        }

        //        if (GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
        //        {
        //            GeosApplication.Instance.ApplicationSettings["ServicePath"] = GeosApplication.Instance.ServicePath;
        //        }
        //        else
        //        {
        //            GeosApplication.Instance.ApplicationSettings.Add("ServicePath", GeosApplication.Instance.ServicePath);
        //        }

        //    }

        //    GeosApplication.Instance.SiteName = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderName => serviceProviderName.Name).FirstOrDefault();

        //}

    }
}
