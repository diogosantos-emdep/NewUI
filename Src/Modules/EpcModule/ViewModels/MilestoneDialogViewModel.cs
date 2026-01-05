using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NetOffice.OutlookApi.Enums;
using Outlook = NetOffice.OutlookApi;
using NetOffice;
using System.IO;
using System.ComponentModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class MilestoneDialogViewModel : ViewModelBase
    {
        #region Services

        IEpcService epcControl;

        #endregion

        #region Commands

        public ICommand MilestoneMailCancelButtonCommand { get; set; }
        public ICommand MilestoneMailAcceptButtonCommand { get; set; }
        public ICommand MilestoneMailSelectedIndexChangedCommand { get; set; }

        #endregion

        #region Properties

        private ProjectMilestone projectMilestoneData;
        public ProjectMilestone ProjectMilestoneData
        {
            get { return projectMilestoneData; }
            set
            {
                SetProperty(ref projectMilestoneData, value, () => ProjectMilestoneData);
            }
        }

        public bool IsMilestoneAchieved { get; set; }
        public bool IsMilestoneUpdated { get; set; }

        Visibility isHide;
        public Visibility IsHide
        {
            get { return isHide; }
            set
            {
                isHide = value;
                SetProperty(ref isHide, value, () => IsHide);
            }
        }

        bool isEnable;
        public bool IsVEnable
        {
            get { return isEnable; }
            set
            {
                isEnable = value;
                SetProperty(ref isEnable, value, () => IsVEnable);
            }
        }

        private string mySelectedItem;
        public string MySelectedItem
        {
            get { return mySelectedItem; }
            set
            {
                mySelectedItem = value;
                SetProperty(ref mySelectedItem, value, () => MySelectedItem);
                // NotifyPropertyChanged(mySelectedItem);
            }
        }

        private DateTime newTargetDate = DateTime.Now;
        public DateTime NewTargetDate
        {
            get { return newTargetDate; }
            set
            {
                SetProperty(ref newTargetDate, value, () => NewTargetDate);
            }
        }

        private ObservableCollection<User> userList;
        public ObservableCollection<User> UserList
        {
            get { return userList; }
            set { SetProperty(ref userList, value, () => UserList); }
        }

        private List<object> selectedToUserList;
        public List<object> SelectedToUserList
        {
            get { return selectedToUserList; }
            set { SetProperty(ref selectedToUserList, value, () => SelectedToUserList); }
        }

        private List<object> selectedCCUserList;
        public List<object> SelectedCCUserList
        {
            get { return selectedCCUserList; }
            set { SetProperty(ref selectedCCUserList, value, () => SelectedCCUserList); }
        }

        #endregion

        #region Constructor
        
        public MilestoneDialogViewModel()
        {
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            UserList = new ObservableCollection<User>(epcControl.GetUsers().AsEnumerable());

            MilestoneMailCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            MilestoneMailAcceptButtonCommand = new RelayCommand(new Action<object>(SendMilestoneAcceptanceMail));
            MilestoneMailSelectedIndexChangedCommand = new RelayCommand(SelectedIndexChangedMilestone);
        }

        #endregion

        protected override void OnParameterChanged(object parameter)
        {
            ProjectMilestoneData = (ProjectMilestone)parameter;
            base.OnParameterChanged(parameter);
        }

        public event EventHandler RequestClose;
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void SendMilestoneAcceptanceMail(object obj)
        {
            if (MySelectedItem != null)
            {
                if (MySelectedItem == "Achieved")
                {
                    IsMilestoneAchieved = true;
                    CreateOutlookEmail(SelectedToUserList, SelectedCCUserList, "Milestone Achieved", getTextGreen(ProjectMilestoneData));

                    if (ProjectMilestoneData.ProjectMilestoneDates != null)
                    {
                        ProjectMilestoneDate projectMilestoneDate = ProjectMilestoneData.ProjectMilestoneDates.LastOrDefault();
                        if (projectMilestoneDate != null)
                        {
                            projectMilestoneDate.IdProjectMilestoneStatus = 90;
                            projectMilestoneDate = epcControl.UpdateProjectMilestoneDateById(projectMilestoneDate);
                        }
                    }
                }
                else
                {
                    IsMilestoneUpdated = true;

                    if (MySelectedItem == "Warning")
                    {
                        CreateOutlookEmail(SelectedToUserList, SelectedCCUserList, "Milestone Warning", getTextYellow(ProjectMilestoneData));
                    }
                    else if (MySelectedItem == "Failed")
                    {
                        CreateOutlookEmail(SelectedToUserList, SelectedCCUserList, "Milestone Failed", getTextRed(ProjectMilestoneData));
                    }

                    // Update Milestone's Current Date Status To Failed and Add new Date with Status OnTime.
                    if (ProjectMilestoneData.ProjectMilestoneDates != null)
                    {
                        ProjectMilestoneDate projectMilestoneDate = ProjectMilestoneData.ProjectMilestoneDates.LastOrDefault();
                        if (projectMilestoneDate != null)
                        {
                            projectMilestoneDate.IdProjectMilestoneStatus = 91;
                            projectMilestoneDate = epcControl.UpdateProjectMilestoneDateById(projectMilestoneDate);

                            ProjectMilestoneDate newProjectMilestoneDate = new ProjectMilestoneDate()
                            {
                                IdProjectMilestoneStatus = 92,
                                IdProjectMilestone = ProjectMilestoneData.IdProjectMilestone,
                                TargetDate = NewTargetDate
                            };

                            newProjectMilestoneDate = epcControl.AddProjectMilestoneDate(newProjectMilestoneDate);

                            if (newProjectMilestoneDate != null)
                            {
                                ProjectMilestoneData.ProjectMilestoneDates.Add(newProjectMilestoneDate);
                            }
                        }
                    }
                }
            }

            RequestClose(null, null);
        }


        private void CreateOutlookEmail(List<object> ToEmailList, List<object> CcEmailList, string subject, string body)
        {
            try
            {
                Outlook.Application objApp = new Outlook.Application();
                Outlook.MailItem mailItem = null;
                mailItem = (Outlook.MailItem)objApp.CreateItem(OlItemType.olMailItem);
                mailItem.Subject = subject;

                if (ToEmailList != null && ToEmailList.Count > 0)
                {
                    List<User> To = ToEmailList.Cast<User>().ToList();
                    mailItem.To = String.Join(";", To.Select(user => user.CompanyEmail != null ? user.CompanyEmail.ToString() : ""));
                }

                if (CcEmailList != null && CcEmailList.Count > 0)
                {
                    List<User> Cc = CcEmailList.Cast<User>().ToList();
                    mailItem.CC = String.Join(";", Cc.Select(user => user.CompanyEmail != null ? user.CompanyEmail.ToString() : ""));
                }

                mailItem.BodyFormat = OlBodyFormat.olFormatHTML;
                mailItem.HTMLBody = body;
                mailItem.HTMLBody += ReadSignature();
                mailItem.Importance = OlImportance.olImportanceHigh;

                mailItem.Send();
                //mailItem.Display(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string ReadSignature()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            string signature = string.Empty;
            DirectoryInfo diInfo = new DirectoryInfo(appDataDir);

            if (diInfo.Exists)
            {
                FileInfo[] fiSignature = diInfo.GetFiles("*.htm");

                if (fiSignature.Length > 0)
                {
                    StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
                    signature = sr.ReadToEnd();
                    if (!string.IsNullOrEmpty(signature))
                    {
                        string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
                        signature = signature.Replace(fileName + "_files/", appDataDir + "/" + fileName + "_files/");
                    }
                }

            }
            return signature;
        }

        public void SelectedIndexChangedMilestone(object obj)
        {
            if (MySelectedItem == "Failed")
            {
                IsHide = Visibility.Hidden;
                IsVEnable = false;
            }
        }

        public string getTextGreen(ProjectMilestone projectMilestoneData)
        {
            string str = string.Format(@"<table class='Tablanormal' 
            style='margin-left: -1.15pt; border-collapse: collapse; width: 737px; height: 189px;'
            border='0' cellpadding='0' cellspacing='0'>
            <tbody><tr style='height: 24.95pt;'><td colspan='5'
             style='border: 1pt solid gray; padding: 0in 3.5pt; background: rgb(146, 208, 80) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 514.9pt; height: 24.95pt;'
             nowrap='nowrap' width='687'><p class='MsoNormal' style='text-align: center;'
             align='center'><b><span style='font-size: 14pt; color: black;'>Engineering Milestones Achieved<o:p></o:p></span></b></p>
                  </td></tr><tr style='height: 15pt;'>
                  <td style='border-style: none solid solid; border-color: -moz-use-text-color gray gray; border-width: medium 1pt 1pt; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 74pt; height: 15pt;'
             nowrap='nowrap' width='99'>
                  <p class='MsoNormal'><span style='color: gray;'>OT Code<o:p></o:p></span></p>
                  </td>
                  <td
             style='border-style: none solid solid none; border-color: -moz-use-text-color gray gray -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 83.2pt; height: 15pt;'
             nowrap='nowrap' width='111'>
                  <p class='MsoNormal'><span style='color: gray;'>Project Name<o:p></o:p></span></p>
                  </td>
                  <td
             style='border-style: none solid solid none; border-color: -moz-use-text-color gray gray -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 68.15pt; height: 15pt;'
             nowrap='nowrap' width='91'>
                  <p class='MsoNormal'><span style='color: gray;'>EPC Code<o:p></o:p></span></p></td>
                  <td
             style='border-style: none solid solid none; border-color: -moz-use-text-color gray gray -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 208.75pt; height: 15pt;'
             nowrap='nowrap' width='278'>
                  <p class='MsoNormal'><span style='color: gray;'>Milestones (Ordinal &amp; Description)<o:p></o:p></span></p> </td>
                  <td
             style='border-style: none solid solid none; border-color: -moz-use-text-color gray gray -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 80.8pt; height: 15pt;'
             nowrap='nowrap' width='108'>
                  <p class='MsoNormal'><span style='color: gray;'>Date<o:p></o:p></span></p></td></tr>
                <tr style='height: 60pt;'>
                  <td
             style='border-style: none solid solid; border-color: -moz-use-text-color gray gray; border-width: medium 1pt 1pt; padding: 0in 3.5pt; width: 74pt; height: 60pt;'
             nowrap='nowrap' valign='top' width='99'> {0}
                  </td>
                  <td
             style='border-style: none solid solid none; border-color: -moz-use-text-color gray gray -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 83.2pt; height: 60pt;'
             nowrap='nowrap' valign='top' width='111'>{1}</td>
                  <td
             style='border-style: none solid solid none; border-color: -moz-use-text-color gray gray -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 68.15pt; height: 60pt;'
             nowrap='nowrap' valign='top' width='91'>{2}</td>
                  <td
             style='border-style: none solid solid none; border-color: -moz-use-text-color gray gray -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 208.75pt; height: 60pt;'
             nowrap='nowrap' valign='top' width='278'>{3}
                  </td>
                  <td
             style='border-style: none solid solid none; border-color: -moz-use-text-color gray gray -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 80.8pt; height: 60pt;'
             nowrap='nowrap' valign='top' width='108'>{4}
      
                  </td></tr></tbody></table>", (projectMilestoneData.Project.Offer != null)? projectMilestoneData.Project.Offer.Code:"", 
                                               projectMilestoneData.Project.ProjectName, 
                                               projectMilestoneData.MilestoneTitle, 
                                               projectMilestoneData.ProjectMilestoneDates.Last().TargetDate, 
                                               projectMilestoneData.Project.ProjectCode, 
                                               projectMilestoneData.Description);
            return str;
        }

        public string getTextYellow(ProjectMilestone projectMilestoneData)
        {
            string str = string.Format(@"<table class='Tablanormal' style='margin-left: -1.15pt; border-collapse: collapse;' border='0' cellpadding='0' cellspacing='0'>
              <tbody>
                <tr style='height: 24.95pt;'>
                  <td colspan='5' style='border: 1pt solid rgb(166, 166, 166); padding: 0in 3.5pt; background: yellow none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 681.15pt; height: 24.95pt;' nowrap='nowrap' width='908'>
                  <p class='MsoNormal' style='text-align: center;' align='center'><b><span style='font-size: 14pt; color: black;'>Engineering Milestones Warning<o:p></o:p></span></b></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 24.95pt;'height='33' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
                  <td style='border-style: none solid solid; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166); border-width: medium 1pt 1pt; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 68.15pt; height: 15pt;' nowrap='nowrap' width='91'>
                  <p class='MsoNormal'><span style='color: gray;'>OT Code<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 109.55pt; height: 15pt;' nowrap='nowrap' width='146'>
                  <p class='MsoNormal'><span style='color: gray;'>Project Name<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 196.25pt; height: 15pt;' nowrap='nowrap' width='262'>
                  <p class='MsoNormal'><span style='color: gray;'>Milestones (Ordinal &amp; Description)<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 2.2in; height: 15pt;' nowrap='nowrap' width='211'>
                  <p class='MsoNormal'><span style='color: gray;'>Reasons<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 148.8pt; height: 15pt;' nowrap='nowrap' width='198'>
                  <p class='MsoNormal'><span style='color: gray;'>Date<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 22.95pt;'>
                  <td rowspan='3' style='border-style: none solid solid; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166); border-width: medium 1pt 1pt; padding: 0in 3.5pt; width: 68.15pt; height: 22.95pt;' nowrap='nowrap' width='91'>
                  <p class='MsoNormal'><span style='color: black;'><o:p>&nbsp;</o:p></span></p>{0}
                  </td>
                  <td rowspan='3'style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 109.55pt; height: 22.95pt;' nowrap='nowrap' valign='top' width='146'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>&nbsp;<o:p></o:p></span></p>{1}
                  </td>
                  <td rowspan='3' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 196.25pt; height: 22.95pt;' nowrap='nowrap' valign='top' width='262'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>&nbsp;<o:p></o:p></span></p>{2}
                  </td>
                  <td rowspan='3' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 2.2in; height: 22.95pt;' nowrap='nowrap' valign='top' width='211'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 148.8pt; height: 22.95pt;' nowrap='nowrap' valign='top' width='198'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>12/08/2015<o:p></o:p></span></p>{3}
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 22.95pt;' height='31' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15.1pt;'>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 148.8pt; height: 15.1pt;' nowrap='nowrap' width='198'>
                  <p class='MsoNormal'><span style='color: gray;'>New Date Schedulled<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15.1pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 19.25pt;'>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 148.8pt; height: 19.25pt;' nowrap='nowrap' valign='top' width='198'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'><o:p>&nbsp;</o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 19.25pt;' height='26' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
                  <td style='border-style: none solid solid; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166); border-width: medium 1pt 1pt; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 68.15pt; height: 15pt;' nowrap='nowrap' width='91'>
                  <p class='MsoNormal'><span style='color: gray;'>EPC Code<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 109.55pt; height: 15pt;' nowrap='nowrap' width='146'>
                  <p class='MsoNormal'><span style='color: gray;'>Sure consequences<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 196.25pt; height: 15pt;' nowrap='nowrap' width='262'>
                  <p class='MsoNormal'><span style='color: gray;'>Probable consequences<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 2.2in; height: 15pt;' nowrap='nowrap' width='211'>
                  <p class='MsoNormal'><span style='color: gray;'>Corrective action<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 148.8pt; height: 15pt;' nowrap='nowrap' width='198'>
                  <p class='MsoNormal'><span style='color: gray;'>Project Owner<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 32.55pt;'>
                  <td rowspan='4' style='border-style: none solid solid; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166); border-width: medium 1pt 1pt; padding: 0in 3.5pt; width: 68.15pt; height: 32.55pt;' nowrap='nowrap' width='91'>
                  <p class='MsoNormal' style='text-indent: 10pt;'><span style='font-size: 10pt; color: black;'>&nbsp;<o:p></o:p></span></p>{4}
                  </td>
                  <td rowspan='4' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 109.55pt; height: 32.55pt;' nowrap='nowrap' valign='top' width='146'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
                  <td rowspan='4' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 196.25pt; height: 32.55pt;' nowrap='nowrap' valign='top' width='262'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
                  <td rowspan='4' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 2.2in; height: 32.55pt;' nowrap='nowrap' valign='top' width='211'>
                  <p class='MsoNormal' style='text-indent: 10pt;'><span style='font-size: 10pt; color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 148.8pt; height: 32.55pt;' nowrap='nowrap' width='198'>
                  <p class='MsoNormal' style='text-indent: 10pt;'><span style='font-size: 10pt; color: black;'>&nbsp;<o:p></o:p></span></p>{5}
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 32.55pt;' height='43' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 148.8pt; height: 15pt;' nowrap='nowrap' width='198'>
                  <p class='MsoNormal'><span style='color: gray;'>Milestone Area Owner<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
                  <td rowspan='2' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 148.8pt; height: 15pt;' nowrap='nowrap' width='198'>
                  <p class='MsoNormal' style='text-align: center;' align='center'><span style='font-size: 10pt; color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
              </tbody>
            </table>", (projectMilestoneData.Project.Offer != null)? projectMilestoneData.Project.Offer.Code:"",
                       projectMilestoneData.Project.ProjectName,
                       projectMilestoneData.MilestoneTitle,
                       projectMilestoneData.ProjectMilestoneDates.Last().TargetDate,
                       projectMilestoneData.Project.ProjectCode,
                       projectMilestoneData.Description);
            return str;
        }

        public string getTextRed(ProjectMilestone projectMilestoneData)
        {
            string str = string.Format(@"<table class='Tablanormal'
             style='margin-left: -1.15pt; border-collapse: collapse;'
             border='0' cellpadding='0' cellspacing='0'>
              <tbody>
                <tr style='height: 24.95pt;'>
                  <td colspan='5'style='border: 1pt solid rgb(166, 166, 166); padding: 0in 3.5pt; background: red none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 700.2pt; height: 24.95pt;' nowrap='nowrap' width='934'>
                  <p class='MsoNormal' style='text-align: center;'align='center'><b><span style='font-size: 14pt; color: white;'>Non Compliance Milestone<o:p></o:p></span></b></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 24.95pt;' height='33' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
                  <td style='border-style: none solid solid; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166); border-width: medium 1pt 1pt; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 74pt; height: 15pt;' nowrap='nowrap' width='99'>
                  <p class='MsoNormal'><span style='color: gray;'>OT Code<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 109.55pt; height: 15pt;' nowrap='nowrap' width='146'>
                  <p class='MsoNormal'><span style='color: gray;'>Project Name<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 219pt; height: 15pt;' nowrap='nowrap' width='292'>
                  <p class='MsoNormal'><span style='color: gray;'>Milestones (Ordinal &amp; Description)<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 163pt; height: 15pt;' nowrap='nowrap' width='217'>
                  <p class='MsoNormal'><span style='color: gray;'>Reasons<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 134.65pt; height: 15pt;' nowrap='nowrap' width='180'>
                  <p class='MsoNormal'><span style='color: gray;'>Date<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 20.1pt;'>
                  <td rowspan='3'style='border-style: none solid solid; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166); border-width: medium 1pt 1pt; padding: 0in 3.5pt; width: 74pt; height: 20.1pt;' nowrap='nowrap' valign='top' width='99'>{0}</td>
                  <td rowspan='3' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 109.55pt; height: 20.1pt;' nowrap='nowrap' valign='top' width='146'>
            <p class='MsoNormal' style='text-indent: 11pt;'><spanstyle='color: black;'>&nbsp;<o:p></o:p></span></p>
                  {1}</td>
                  <td rowspan='3' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 219pt; height: 20.1pt;' nowrap='nowrap' valign='top' width='292'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>&nbsp;<o:p></o:p></span></p>{2}
                  </td>
                  <td rowspan='3' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 163pt; height: 20.1pt;' nowrap='nowrap' valign='top' width='217'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 134.65pt; height: 20.1pt;' nowrap='nowrap' valign='top' width='180'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>12/08/2015<o:p></o:p></span></p>{3}
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 20.1pt;' height='27' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 20.1pt;'>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 134.65pt; height: 20.1pt;' nowrap='nowrap' width='180'>
                  <p class='MsoNormal'><span style='color: gray;'>New Date Schedulled<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 20.1pt;' height='27' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 20.1pt;'>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 134.65pt; height: 20.1pt;' nowrap='nowrap' valign='top' width='180'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'><o:p>&nbsp;</o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 20.1pt;' height='27' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
                  <td style='border-style: none solid solid; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166); border-width: medium 1pt 1pt; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 74pt; height: 15pt;' nowrap='nowrap' width='99'>
                  <p class='MsoNormal'><span style='color: gray;'>EPC Code<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 109.55pt; height: 15pt;' nowrap='nowrap' width='146'>
                  <p class='MsoNormal'><span style='color: gray;'>Sure consequences<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 219pt; height: 15pt;' nowrap='nowrap' width='292'>
                  <p class='MsoNormal'><span style='color: gray;'>Probable consequences<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 163pt; height: 15pt;' nowrap='nowrap' width='217'>
                  <p class='MsoNormal'><span style='color: gray;'>Corrective action<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 134.65pt; height: 15pt;'nowrap='nowrap' width='180'>
                  <p class='MsoNormal'><span style='color: gray;'>Project Owner<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 29.75pt;'>
                  <td rowspan='4' style='border-style: none solid solid; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166); border-width: medium 1pt 1pt; padding: 0in 3.5pt; width: 74pt; height: 29.75pt;' nowrap='nowrap' width='99'>
                  <p class='MsoNormal' style='text-indent: 10pt;'><span style='font-size: 10pt; color: black;'>&nbsp;<o:p></o:p></span></p>{4}
                  </td>
                  <td rowspan='4'style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 109.55pt; height: 29.75pt;' nowrap='nowrap' valign='top' width='146'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
                  <td rowspan='4' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 219pt; height: 29.75pt;' nowrap='nowrap' valign='top' width='292'>
                  <p class='MsoNormal' style='text-indent: 11pt;'><span style='color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
                  <td rowspan='4' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 163pt; height: 29.75pt;'nowrap='nowrap' valign='top' width='217'>
                  <p class='MsoNormal' style='text-indent: 10pt;'><span style='font-size: 10pt; color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 134.65pt; height: 29.75pt;' nowrap='nowrap' valign='top' width='180'>
                  <p class='MsoNormal' style='text-indent: 10pt;'><span style='font-size: 10pt; color: black;'>&nbsp;<o:p></o:p></span></p>{5}
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 29.75pt;' height='40' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
                  <td style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; background: rgb(242, 242, 242) none repeat scroll 0% 50%; -moz-background-clip: initial; -moz-background-origin: initial; -moz-background-inline-policy: initial; width: 134.65pt; height: 15pt;' nowrap='nowrap' width='180'>
                  <p class='MsoNormal'><span style='color: gray;'>Milestone Area Owner<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
                  <td rowspan='2' style='border-style: none solid solid none; border-color: -moz-use-text-color rgb(166, 166, 166) rgb(166, 166, 166) -moz-use-text-color; border-width: medium 1pt 1pt medium; padding: 0in 3.5pt; width: 134.65pt; height: 15pt;' nowrap='nowrap' valign='top' width='180'>
                  <p class='MsoNormal'><span style='font-size: 10pt; color: black;'>&nbsp;<o:p></o:p></span></p>
                  </td>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
                <tr style='height: 15pt;'>
            <!--[if !supportMisalignedRows]-->
                  <td style='border: medium none ; height: 15pt;' height='20' width='0'></td>
            <!--[endif]-->
                </tr>
              </tbody>
            </table>", (projectMilestoneData.Project.Offer != null) ? projectMilestoneData.Project.Offer.Code : "",
                       projectMilestoneData.Project.ProjectName, 
                       projectMilestoneData.MilestoneTitle, 
                       projectMilestoneData.ProjectMilestoneDates.Last().TargetDate, 
                       projectMilestoneData.Project.ProjectCode, 
                       projectMilestoneData.Description);
            return str;
        }

    }
}
