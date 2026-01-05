using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.DataAccess;
using Emdep.Geos.Data.Common;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Net.Mime;
using Emdep.Geos.Utility;
using Emdep.Geos.Data.Common.Glpi;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class ApplicationManager
    {
        public string UserName { get; set; }
        public string Code { get; set; }

        /// <summary>
        /// This method is to send code through mail to set new password
        /// </summary>
        /// <param name="Title">Get subject for mail message subject</param>
        /// <param name="Description">Get Description in html format</param>
        /// <param name="mailTo">Get EmailId of receiver</param>
        public void SendForgetPasswordMail(string code, User user, string mailServerName, string mailServerPort, string mailFrom, string emailTemplate)
        {
            //Get ForgetMailFormat.html file
            //var name = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString());
            string body = File.ReadAllText(emailTemplate + "ForgetMailFormat.html");
            body = body.Replace("[USERNAME]", user.FirstName);
            body = body.Replace("[CODE]", code);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("0", emailTemplate + @"Images\MailLogo.png");
            dict.Add("1", emailTemplate + @"Images\MailIcon.png");
            dict.Add("2", emailTemplate + @"Images\SkypeIcon.png");
            MailControl.SendHtmlMail("Forget Password", body, user.CompanyEmail, mailFrom, mailServerName, mailServerPort, dict);
        }

        public void SendActivityMail(ActivityMail activitymail, string mailServerName, string mailServerPort, string MailFrom, string emailTemplate)
        {
            string crmMailFrom = string.Concat("CRM-", MailFrom);
            string subject = string.Format("[CRM] New {0} Activity: {1}", activitymail.ActivityType, activitymail.ActivitySubject);
            string body = File.ReadAllText(emailTemplate + "ActivityMailFormat.html");
            body = body.Replace("[USERNAME]", activitymail.SendToUserName);
            body = body.Replace("[CREATEDBY]", activitymail.CreatedByUserName);
            body = body.Replace("[ACTIVITYTYPE]", activitymail.ActivityType);
            body = body.Replace("[ACTIVITYSUBJECT]", activitymail.ActivitySubject);
            body = body.Replace("[ACTIVITYDESC]", activitymail.ActivityDescription);
            body = body.Replace("[ACTIVITYDUEDATE]", activitymail.ActivityDueDate);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            MailControl.SendHtmlMail(subject, body, activitymail.ActivitySentToMail, crmMailFrom, mailServerName, mailServerPort, dict);
        }

        public void SendNewAccountMail(MailTemplateFormat mailTemplateFormat, string mailServerName, string mailServerPort, string MailFrom, string emailTemplate)
        {
            string crmMailFrom = string.Concat("CRM-", MailFrom);
            string body = File.ReadAllText(emailTemplate + "NewAccountMailFormat.html");
            body = body.Replace("[USERNAME]", mailTemplateFormat.SendToUserName);
            body = body.Replace("[ACCOUNTNAME]", mailTemplateFormat.AccountName);
            body = body.Replace("[CREATEDBY]", mailTemplateFormat.CreatedByUserName);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            MailControl.SendHtmlMail("[CRM] New Account", body, mailTemplateFormat.SentToMail, crmMailFrom, mailServerName, mailServerPort, dict);
        }

        public void SendNewProjectMail(MailTemplateFormat mailTemplateFormat, string mailServerName, string mailServerPort, string MailFrom, string emailTemplate)
        {
            string crmMailFrom = string.Concat("CRM-", MailFrom);
            string body = File.ReadAllText(emailTemplate + "NewProjectMailFormat.html");
            body = body.Replace("[USERNAME]", mailTemplateFormat.SendToUserName);
            body = body.Replace("[PROJECTNAME]", mailTemplateFormat.ProjectName);
            body = body.Replace("[CAROEM]", mailTemplateFormat.CarOEMName);
            body = body.Replace("[CREATEDBY]", mailTemplateFormat.CreatedByUserName);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            MailControl.SendHtmlMail("[CRM] New Project", body, mailTemplateFormat.SentToMail, crmMailFrom, mailServerName, mailServerPort, dict);
        }

        public void SendActivityReminderMail(ActivityMail activityMail, string mailServerName, string mailServerPort, string MailFrom, string emailTemplate)
        {
            string crmMailFrom = string.Concat("CRM-", MailFrom);
            string subject = string.Format("[CRM] Activity Reminder - {0}", activityMail.ActivitySubject);
            string body = File.ReadAllText(emailTemplate + "ActivityReminderMailFormat.html");
            body = body.Replace("[USERNAME]", activityMail.SendToUserName);
            body = body.Replace("[ACTIVITYTYPE]", activityMail.ActivityType);
            body = body.Replace("[ACTIVITYSUBJECT]", activityMail.ActivitySubject);
            body = body.Replace("[ACTIVITYDESC]", activityMail.ActivityDescription);
            body = body.Replace("[ACTIVITYDUEDATE]", activityMail.ActivityDueDate);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            MailControl.SendHtmlMail(subject, body, activityMail.ActivitySentToMail, crmMailFrom, mailServerName, mailServerPort, dict);
        }

        //For Eng Analysis - Created New Project in rddotproject - Send mail.
        public void SendNewRddotProjectMail(MailServer mailServer, string sendTo, string projectName, Offer offer, string DeliveryDate)
        {
            if (File.Exists(mailServer.MailTemplatePath + "NewDotProjectMailFormat.html"))
            {
                //Get single user mail id
                string[] strname = sendTo.Split(';');

                foreach (var item in strname)
                {
                    string crmMailFrom = string.Concat("CRM-", mailServer.MailFrom);
                    string subject = string.Format("[GEOS notification] New Project {0}", offer.Code);
                    string body = File.ReadAllText(mailServer.MailTemplatePath + "NewDotProjectMailFormat.html");

                    //Get User full name by loginname
                    UserManager usermanager = new UserManager();
                    User user = usermanager.GetUserByEmailId(item.Trim());

                    if (user != null)
                    {
                        body = body.Replace("[USERNAME]", user.FullName);
                        if (offer.CreatedByUser != null)
                            body = body.Replace("[CREATEDBY]", offer.CreatedByUser.FullName);
                        else if (offer.ModifiedByUser != null)
                            body = body.Replace("[CREATEDBY]", offer.ModifiedByUser.FullName);

                        body = body.Replace("[Description]", offer.Description);
                        body = body.Replace("[WorkingOrder]", offer.Code);
                        body = body.Replace("[InternalID]", projectName);
                        body = body.Replace("[Customer]", offer.Site.FullName);
                        body = body.Replace("[DeliveryDate]", DeliveryDate);
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        MailControl.SendHtmlMail(subject, body, item, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                    }
                }
            }
        }

        public void SendUpdateRddotProjectMail(MailServer mailServer, string sendTo, Offer offer, string prevDeliveryDate, string deliveryDate)
        {
            if (File.Exists(mailServer.MailTemplatePath + "UpdateDotProjectMailFormat.html"))
            {
                string[] strname = sendTo.Split(';');

                foreach (var item in strname)
                {
                    string crmMailFrom = string.Concat("CRM-", mailServer.MailFrom);
                    string subject = string.Format("[GEOS notification] Date changed");
                    string body = File.ReadAllText(mailServer.MailTemplatePath + "UpdateDotProjectMailFormat.html");

                    //Get User full name by loginname
                    UserManager usermanager = new UserManager();
                    User user = usermanager.GetUserByEmailId(item.Trim());
                    if (user != null)
                    {
                        body = body.Replace("[USERNAME]", user.FullName);
                        body = body.Replace("[CREATEDBY]", offer.ModifiedByUser.FullName);
                        body = body.Replace("[WorkingOrder]", offer.Code);
                        body = body.Replace("[PrevDeliveryDate]", prevDeliveryDate);
                        body = body.Replace("[DeliveryDate]", deliveryDate);
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        MailControl.SendHtmlMail(subject, body, item, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                    }
                }
            }
        }


        public void SendEngineeringAnalysisReadyMail(MailServer mailServer, Offer offer, User validatedByUser, People assignedEngAnalysisPerson, DateTime? otDeliveryDate)
        {
            if (File.Exists(mailServer.MailTemplatePath + "EngineeringAnalysisReadyMailFormat.html"))
            {
                string crmMailFrom = string.Concat("CRM-", mailServer.MailFrom);
                string subject = string.Format("[Engineering analysis ready] Offer number : {0}", offer.Code);

                string body = File.ReadAllText(mailServer.MailTemplatePath + "EngineeringAnalysisReadyMailFormat.html");
                body = body.Replace("[VALIDATEDBY]", validatedByUser.FullName);

                body = body.Replace("[Description]", offer.Description);
                body = body.Replace("[WorkingOrder]", offer.Code);
                body = body.Replace("[Customer]", offer.Site.Customers[0].CustomerName + " - " + offer.Site.Name);
                body = body.Replace("[DeliveryDate]", Convert.ToString(otDeliveryDate != null ? otDeliveryDate.Value.ToString("yyyy-MM-dd") : "-"));

                Dictionary<string, string> dict = new Dictionary<string, string>();
                if (offer.SalesOwner != null && !string.IsNullOrEmpty(offer.SalesOwner.Email))
                {
                    string bodyForSalesOwner = body.Replace("[USERNAME]", offer.SalesOwner.FullName);
                    MailControl.SendHtmlMail(subject, bodyForSalesOwner, offer.SalesOwner.Email, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                }

                if (offer.OfferedByPerson != null && !string.IsNullOrEmpty(offer.OfferedByPerson.Email))
                {
                    string bodyForOfferedBy = body.Replace("[USERNAME]", offer.OfferedByPerson.FullName);
                    MailControl.SendHtmlMail(subject, bodyForOfferedBy, offer.OfferedByPerson.Email, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                }

                if (offer.AssignedToPerson != null && !string.IsNullOrEmpty(offer.AssignedToPerson.Email))
                {
                    string bodyForAssignedTo = body.Replace("[USERNAME]", offer.AssignedToPerson.FullName);
                    MailControl.SendHtmlMail(subject, bodyForAssignedTo, offer.AssignedToPerson.Email, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                }

                if (assignedEngAnalysisPerson != null && !string.IsNullOrEmpty(assignedEngAnalysisPerson.Email))
                {
                    string bodyForAssignedEngAnalysisPerson = body.Replace("[USERNAME]", assignedEngAnalysisPerson.FullName);
                    MailControl.SendHtmlMail(subject, bodyForAssignedEngAnalysisPerson, assignedEngAnalysisPerson.Email, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                }

            }
        }

        public void SendEngineeringAnalysisReopenedMail(MailServer mailServer, Offer offer, People assignedEngAnalysisPerson, DateTime? otDeliveryDate)
        {
            if (File.Exists(mailServer.MailTemplatePath + "EngineeringAnalysisReopenedMailFormat.html"))
            {
                string crmMailFrom = string.Concat("CRM-", mailServer.MailFrom);
                string subject = string.Format("[Engineering analysis re-opened] Offer number : {0}", offer.Code);

                string body = File.ReadAllText(mailServer.MailTemplatePath + "EngineeringAnalysisReopenedMailFormat.html");
                body = body.Replace("[REOPENEDBY]", offer.ModifiedByUser.FullName);

                body = body.Replace("[Description]", offer.Description);
                body = body.Replace("[WorkingOrder]", offer.Code);
                body = body.Replace("[Customer]", offer.Site.Customers[0].CustomerName + " - " + offer.Site.Name);
                body = body.Replace("[DeliveryDate]", Convert.ToString(otDeliveryDate != null ? otDeliveryDate.Value.ToString("yyyy-MM-dd") : "-"));

                Dictionary<string, string> dict = new Dictionary<string, string>();
                if (offer.SalesOwner != null && !string.IsNullOrEmpty(offer.SalesOwner.Email))
                {
                    string bodyForSalesOwner = body.Replace("[USERNAME]", offer.SalesOwner.FullName);
                    MailControl.SendHtmlMail(subject, bodyForSalesOwner, offer.SalesOwner.Email, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                }

                if (offer.OfferedByPerson != null && !string.IsNullOrEmpty(offer.OfferedByPerson.Email))
                {
                    string bodyForOfferedBy = body.Replace("[USERNAME]", offer.OfferedByPerson.FullName);
                    MailControl.SendHtmlMail(subject, bodyForOfferedBy, offer.OfferedByPerson.Email, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                }

                if (offer.AssignedToPerson != null && !string.IsNullOrEmpty(offer.AssignedToPerson.Email))
                {
                    string bodyForAssignedTo = body.Replace("[USERNAME]", offer.AssignedToPerson.FullName);
                    MailControl.SendHtmlMail(subject, bodyForAssignedTo, offer.AssignedToPerson.Email, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                }

                if (assignedEngAnalysisPerson != null && !string.IsNullOrEmpty(assignedEngAnalysisPerson.Email))
                {
                    string bodyForAssignedEngAnalysisPerson = body.Replace("[USERNAME]", assignedEngAnalysisPerson.FullName);
                    MailControl.SendHtmlMail(subject, bodyForAssignedEngAnalysisPerson, assignedEngAnalysisPerson.Email, crmMailFrom, mailServer.MailServerName, mailServer.MailServerPort, dict);
                }
            }
        }

        public string GetEmployeeExitEmailTemplate(MailServer mailServer)
        {
            string text = string.Empty; ;

            try
            {
                text = File.ReadAllText(mailServer.MailTemplatePath + "EmployeeExitMailFormat.txt");
            }
            catch
            {
            }

            return text;
        }


        public void SendGlpiTicketMail(GlpiTicketMail glpiTicketMail, string mailServerName, string mailServerPort, string mailFrom, string emailTemplate, string mailTo)
        {
           MailControl.SendGlpiHtmlMail(glpiTicketMail.Title, glpiTicketMail.Description, mailTo, mailFrom, mailServerName, mailServerPort, glpiTicketMail.Attachments);
        }

    }
}
