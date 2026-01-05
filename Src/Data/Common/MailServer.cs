using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class MailServer : ModelBase, IDisposable
    {
        #region Fields

        string mailServerName;
        string mailServerPort;
        string mailTemplatePath;
        string mailFrom;

        #endregion

        #region Constructor

        public MailServer()
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
