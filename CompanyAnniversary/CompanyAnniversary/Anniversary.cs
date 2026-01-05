using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Configuration;

namespace CompanyAnniversary
{
    public class Anniversary
    {
        public void Init(string EmpName,string Subject,DateTime AnniversaryDate,string Years, string EmpCode, byte[] ImageByte,string AnniversaryEmailPath, string MailTo,string MailHost,int MailPort)
        {
            #region Assign Values 
            AnniversaryReport AnniversaryReport = new AnniversaryReport();
            AnniversaryReport.LblEmpName.Text = EmpName;
            AnniversaryReport.LblYear.Text = Years;
            if (Convert.ToInt32(Years) == 1)
                AnniversaryReport.LblYearString.Text = "year of dedicated service";
            else
                AnniversaryReport.LblYearString.Text = "years of dedicated service";      
            string formattedDate = AnniversaryDate.ToString("dd'" + GetOrdinalIndicator(DateTime.Now.Day) + " " + "' MMMM yyyy");
            AnniversaryReport.LblDate.Text = formattedDate;
            byte[] Empbytes = null;
            byte[] Reportbytes = null;
            #endregion

            #region Employee Image
            if (ImageByte != null)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(ImageByte))
                {
                    BitmapImage image = new BitmapImage();

                    image.BeginInit();
                    image.StreamSource = ms;
                    image.EndInit();
                    Bitmap img = new Bitmap(image.StreamSource);
                    AnniversaryReport.xrPictureBox2.Image = img;
                }
            }
            #endregion

            #region Get Generated Report Image
            AnniversaryReport.ExportToImage(AnniversaryEmailPath + "Report.png");
            if (File.Exists(AnniversaryEmailPath + "Report.png"))
            {
                using (System.IO.FileStream stream = new System.IO.FileStream((AnniversaryEmailPath + "Report.png"), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    Reportbytes = new byte[stream.Length];
                    int numBytesToRead = (int)stream.Length;
                    int numBytesRead = 0;

                    while (numBytesToRead > 0)
                    {
                        int n = stream.Read(Reportbytes, numBytesRead, numBytesToRead);
                        if (n == 0)
                            break;
                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                }
            }
            #endregion

            #region Email Code
            MailMessage mailmessage = new MailMessage();
            mailmessage.From = new MailAddress("HRM-noreply@emdep.com");
            mailmessage.To.Add(new MailAddress(MailTo, ""));           
            string htmlBody = "<html><body><div style='text-align: center;'><img src='cid:EmbeddedContent_1' style='display: inline-block;'></div></body></html>";
            MemoryStream imgGeosLogo = new MemoryStream(Reportbytes);
            System.Net.Mail.LinkedResource LRGeosLogo = new System.Net.Mail.LinkedResource(imgGeosLogo, "Image/png");
            LRGeosLogo.ContentId = "EmbeddedContent_1";
            LRGeosLogo.ContentLink = new Uri("cid:" + LRGeosLogo.ContentId);
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
            htmlView.LinkedResources.Add(LRGeosLogo);
            mailmessage.AlternateViews.Add(htmlView);
            mailmessage.Subject = Subject;
            mailmessage.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = MailHost;
            smtp.Port = MailPort;
            smtp.Send(mailmessage);           
            #endregion
        }
        private static string GetOrdinalIndicator(int day)
        {
            if (day >= 11 && day <= 13)
            {
                return "th";
            }

            switch (day % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
    }   
}
