using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.OTM
{
    [DataContract]
    public class BlackListEmail : ModelBase, IDisposable
    {
        #region Fields
        long idBlackListEmail;
        string senderEmail;
        string domain;
        int isDeleted;
        #endregion

        #region Constructor
        public BlackListEmail()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public long IdBlackListEmail
        {
            get
            {
                return idBlackListEmail;
            }

            set
            {
                idBlackListEmail = value;
                OnPropertyChanged("IdBlackListEmail");
            }
        }
        [DataMember]
        public string SenderEmail
        {
            get { return senderEmail; }
            set
            {
                senderEmail = value;
                OnPropertyChanged("SenderEmail");
            }
        }

        [DataMember]
        public string Domain
        {
            get { return domain; }
            set
            {
                domain = value;
                OnPropertyChanged("Domain");
            }
        }

        [DataMember]
        public int IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
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
