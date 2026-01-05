using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class HRMMail : ModelBase, IDisposable
    {

        #region Fields

        string mailServerName;
        string mailServerPort;
        string mailTemplatePath;
        string mailFrom;
        byte[] emdepImageInByte;
        byte[] geosImageInByte;
        string mailTo;
        string mailBodyTemplate;
        #endregion

        #region Constructor

        public HRMMail()
        {
        }

        #endregion

        #region Properties


        [NotMapped]
        [DataMember]
        public string MailServerName
        {
            get { return mailServerName; }
            set
            {
                mailServerName = value;
                OnPropertyChanged("MailServerName");
            }
        }
        [DataMember]
        public string MailBodyTemplate
        {
            get { return mailBodyTemplate; }
            set
            {
                mailBodyTemplate = value;
                OnPropertyChanged("MailBodyTemplate");
            }
        }
        [NotMapped]
        [DataMember]
        public string MailServerPort
        {
            get { return mailServerPort; }
            set
            {
                mailServerPort = value;
                OnPropertyChanged("MailServerPort");
            }
        }

        [NotMapped]
        [DataMember]
        public string MailTemplatePath
        {
            get { return mailTemplatePath; }
            set
            {
                mailTemplatePath = value;
                OnPropertyChanged("MailTemplatePath");
            }
        }

        [NotMapped]
        [DataMember]
        public string MailFrom
        {
            get { return mailFrom; }
            set
            {
                mailFrom = value;
                OnPropertyChanged("MailFrom");
            }
        }
    
        [DataMember]
        public byte[] EmdepImageInByte
        {
            get { return emdepImageInByte; }
            set
            {
                emdepImageInByte = value;
                OnPropertyChanged("EmdepImageInByte");
            }
        }

        [DataMember]
        public byte[] GeosImageInByte
        {
            get { return geosImageInByte; }
            set
            {
                geosImageInByte = value;
                OnPropertyChanged("GeosImageInByte");
            }
        }

        [DataMember]
        public string MailTo
        {
            get { return mailTo; }
            set
            {
                mailTo = value;
                OnPropertyChanged("MailTo");
            }
        }
        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
