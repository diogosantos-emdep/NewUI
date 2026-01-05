using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
// Save this file as Emdep.Geos.Utility.cs
// Compile with: csc Emdep.Geos.Utility.cs /doc:Results.xml 
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility
{
    /// <summary>
    /// This class is to mail in standard format.
    /// </summary>
    public class MailControl
    {
        /// <summary>
        /// This method is to send html mail
        /// </summary>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         MailControl.SendHtmlMail("Forget Password","mail body","cpatil@emdep.com","noreply@emdep.com","mail.emdep.com","25",dict);
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="title">Get Subject</param>
        /// <param name="description">Get body</param>
        /// <param name="mailTo">Get recevier mail id </param>
        /// <param name="mailFrom">Get sender mail id</param>
        /// <param name="mailServerName">Get host name</param>
        /// <param name="mailServerPort">Get server port</param>
        /// <param name="Dictionary">Get files</param>
        public static void SendHtmlMail(string title, string description, string mailTo, string mailFrom, string mailServerName, string mailServerPort, Dictionary<string, string> Dictionary)
        {
            MailMessage mailmessage = new MailMessage(mailFrom, mailTo);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(description, null, "text/html");
            foreach (KeyValuePair<string, string> pair in Dictionary)
            {
                //Add Image
                LinkedResource theEmailImage = new LinkedResource(pair.Value.ToString());
                theEmailImage.ContentId = pair.Key.ToString();
                //Add the Image to the Alternate view
                htmlView.LinkedResources.Add(theEmailImage);
                theEmailImage.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            }

            //Add view to the Email Message
            mailmessage.AlternateViews.Add(htmlView);
            //set the Email subject
            mailmessage.Subject = title;

            mailmessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            //smtp.EnableSsl = true;
            //System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }

        /// <summary>
        /// This method is to send mail
        /// </summary>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         MailControl.SendMail("Forget Password",mail body,"cpatil@emdep.com","noreply@emdep.com","mail.emdep.com","25");
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="title">Get Subject</param>
        /// <param name="description">Get body</param>
        /// <param name="mailTo">Get recevier mail id</param>
        /// <param name="mailFrom">Get sender mail id</param>
        /// <param name="mailServerName">Get host name</param>
        /// <param name="mailServerPort">Get server port</param>
        public static void SendMail(string title, string description, string mailTo, string mailFrom, string mailServerName, string mailServerPort)
        {
            MailMessage mailmessage = new MailMessage(mailFrom, mailTo);

            //set the Email subject
            mailmessage.Subject = title;
            mailmessage.Body = description;
            mailmessage.IsBodyHtml = false;
            mailmessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailmessage.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="mailTo"></param>
        /// <param name="ccAddress"></param>
        /// <param name="mailFrom"></param>
        /// <param name="mailServerName"></param>
        /// <param name="mailServerPort"></param>
        /// <param name="imageList"></param>
        public static void SendHtmlMail(string title, string description, string mailTo, List<string> ccAddress, string mailFrom, string mailServerName, string mailServerPort, List<LinkedResource> imageList)
        {
            MailMessage mailmessage = new MailMessage();
            mailmessage.From = new MailAddress(mailFrom);

            foreach (var address in mailTo.Split(';'))
            {
                mailmessage.To.Add(new MailAddress(address, ""));
            }

            foreach (var item in ccAddress)
            {
                MailAddress copy = new MailAddress(item);
                mailmessage.CC.Add(copy);
            }

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(description, null, "text/html");

            if (imageList != null)
                foreach (LinkedResource imgResorce in imageList)
                {
                    htmlView.LinkedResources.Add(imgResorce);
                }

            mailmessage.AlternateViews.Add(htmlView);
            mailmessage.Subject = title;
            mailmessage.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }



        public static void SendHtmlMail(string title, string description, string mailTo, List<string> ccAddress, string mailFrom, string mailServerName, string mailServerPort, List<LinkedResource> imageList, byte[] AttachmentData, string Filename)
        {
            MailMessage mailmessage = new MailMessage();
            mailmessage.From = new MailAddress(mailFrom);

            foreach (var address in mailTo.Split(';'))
            {
                mailmessage.To.Add(new MailAddress(address, ""));
            }

            foreach (var item in ccAddress)
            {
                MailAddress copy = new MailAddress(item);
                mailmessage.CC.Add(copy);
            }

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(description, null, "text/html");

            if (imageList != null)
                foreach (LinkedResource imgResorce in imageList)
                {
                    htmlView.LinkedResources.Add(imgResorce);
                }
            //[rdixit][GEOS2-3551][22.07.2022]
            string AttachmentFileName = Filename + ".xlsx";
            System.IO.MemoryStream AttachmentStream = new System.IO.MemoryStream(AttachmentData);
            mailmessage.Attachments.Add(new System.Net.Mail.Attachment(AttachmentStream, AttachmentFileName, "application/vnd.ms-excel"));
            mailmessage.AlternateViews.Add(htmlView);
            mailmessage.Subject = title;
            mailmessage.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }
		//[nsatpute][15-10-2024][GEOS2-5933]
        public static void SendHtmlMailWithPdfAttachment(string title, string description, string mailTo, List<string> ccAddress, string mailFrom, string mailServerName, string mailServerPort, List<LinkedResource> imageList, byte[] attachmentData, string attachmentFileName)
        {
            // Create the MailMessage object
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(mailFrom),
                Subject = title,
                IsBodyHtml = true
            };

            // Add recipients
            foreach (var address in mailTo.Split(';'))
            {
                mailMessage.To.Add(new MailAddress(address.Trim(), ""));
            }

            // Add CC addresses
            foreach (var item in ccAddress)
            {
                MailAddress copy = new MailAddress(item);
                mailMessage.CC.Add(copy);
            }

            // Create an HTML view for the email body
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(description, null, "text/html");

            // Add linked resources (images) if any
            if (imageList != null)
            {
                foreach (LinkedResource imgResource in imageList)
                {
                    htmlView.LinkedResources.Add(imgResource);
                }
            }

            // Set the attachment filename and MIME type for PDF
            MemoryStream attachmentStream = new MemoryStream(attachmentData);
            mailMessage.Attachments.Add(new System.Net.Mail.Attachment(attachmentStream, attachmentFileName, "application/pdf"));

            // Add the HTML view to the message
            mailMessage.AlternateViews.Add(htmlView);

            // Set up the SMTP client and send the email
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = mailServerName;
                smtp.Port = Convert.ToInt32(mailServerPort);
                smtp.Send(mailMessage);
            }

            // Dispose of the stream after sending the email
            attachmentStream.Dispose();
        }


        public static void SendGlpiHtmlMail(string title, string description, string mailTo, string mailFrom, string mailServerName, string mailServerPort, Dictionary<string, byte[]> attachments)
        {
            MailMessage mailmessage = new MailMessage(mailFrom, mailTo);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(description, null, "text/html");


            //Add view to the Email Message
            mailmessage.AlternateViews.Add(htmlView);
            //set the Email subject
            mailmessage.Subject = title;

            mailmessage.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            foreach (var item in attachments)
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(new MemoryStream(item.Value), item.Key);

                mailmessage.Attachments.Add(attachment);
            }

            //smtp.EnableSsl = true;
            //System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }
        public static void SendHtmlMailWithAttachment(string title, string description, string mailTo, List<string> ccAddress, string mailFrom, string mailServerName, string mailServerPort, List<LinkedResource> imageList, string AttachmentFileName, string AttachmentURL)
        {
            MailMessage mailmessage = new MailMessage();
            mailmessage.From = new MailAddress(mailFrom);

            foreach (var address in mailTo.Split(';'))
            {
                mailmessage.To.Add(new MailAddress(address, ""));
            }

            foreach (var item in ccAddress)
            {
                MailAddress copy = new MailAddress(item);
                mailmessage.CC.Add(copy);
            }

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(description, null, "text/html");

            if (imageList != null)
                foreach (LinkedResource imgResorce in imageList)
                {
                    htmlView.LinkedResources.Add(imgResorce);
                }

            mailmessage.AlternateViews.Add(htmlView);
            mailmessage.Subject = title;
            System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
            contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Pdf;
            contentType.Name = AttachmentFileName;
            mailmessage.Attachments.Add(new System.Net.Mail.Attachment(AttachmentURL, contentType));
            mailmessage.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }
        //Shubham[skadam] GEOS2-3891 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 3 01 03 2023
        public static void SendHtmlMailWithMultipleAttachment(string title, string description, string mailTo, string mailFrom, string mailServerName, string mailServerPort, Dictionary<string, byte[]> attachments)
        {
            MailMessage mailmessage = new MailMessage(mailFrom, mailTo);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(description, null, "text/html");


            //Add view to the Email Message
            mailmessage.AlternateViews.Add(htmlView);
            //set the Email subject
            mailmessage.Subject = title;

            mailmessage.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            foreach (var item in attachments)
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(new MemoryStream(item.Value), item.Key);

                mailmessage.Attachments.Add(attachment);
            }

            //smtp.EnableSsl = true;
            //System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }

        //[rdixit][28.06.2023]
        public static void SendHtmlMailWithccAndMultipleAttachment(string title, string description, string mailTo, List<string> ccAddress, string mailFrom, string mailServerName, string mailServerPort, Dictionary<string, byte[]> attachments)
        {
            MailMessage mailmessage = new MailMessage(mailFrom, mailTo);
            foreach (var item in ccAddress)
            {
                MailAddress copy = new MailAddress(item);
                mailmessage.CC.Add(copy);
            }
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(description, null, "text/html");
            //Add view to the Email Message
            mailmessage.AlternateViews.Add(htmlView);
            //set the Email subject
            mailmessage.Subject = title;
            mailmessage.Priority = MailPriority.High;
            mailmessage.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            foreach (var item in attachments)
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(new MemoryStream(item.Value), item.Key);

                mailmessage.Attachments.Add(attachment);
            }
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }

        public static void SendHtmlMailWithReworkAttachment(string title, string description, string mailTo, List<string> ccAddress, string mailFrom, string mailServerName, string mailServerPort, Dictionary<string, byte[]> attachments)
        {
            MailMessage mailmessage = new MailMessage(mailFrom, mailTo);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(description, null, "text/html");


            //Add view to the Email Message
            mailmessage.AlternateViews.Add(htmlView);
            //set the Email subject
            mailmessage.Subject = title;

            mailmessage.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            foreach (var item in attachments)
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(new MemoryStream(item.Value), item.Key);

                mailmessage.Attachments.Add(attachment);
            }
            foreach (var item in ccAddress)
            {
                MailAddress copy = new MailAddress(item);
                mailmessage.CC.Add(copy);
            }

            //smtp.EnableSsl = true;
            //System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }


        public static void SendHtmlMailFORWMS(string title, string body, string mailTo, string mailFrom, string mailServerName, string mailServerPort,string itemReference,string expectedQty, string actualQty,string picker )
        {
            MailMessage mailmessage = new MailMessage(mailFrom, mailTo);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            //foreach (KeyValuePair<string, string> pair in Dictionary)
            //{
            //    //Add Image
            //    LinkedResource theEmailImage = new LinkedResource(pair.Value.ToString());
            //    theEmailImage.ContentId = pair.Key.ToString();
            //Add the Image to the Alternate view
            //  htmlView.LinkedResources.Add(theEmailImage);
            //   theEmailImage.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            mailmessage.AlternateViews.Add(htmlView);
        

            //Add view to the Email Message
            mailmessage.AlternateViews.Add(htmlView);
            //set the Email subject
            mailmessage.Subject = title;

            mailmessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailServerName;
            //smtp.EnableSsl = true;
            //System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(mailServerPort);
            smtp.Send(mailmessage);
        }
    }
}
