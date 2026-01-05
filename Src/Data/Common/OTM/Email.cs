using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.OTM
{
    [DataContract]
    public class Email : ModelBase, IDisposable
    {
        #region Fields
        long idEmail;
        private string senderName;
        private string senderEmail;
        private string recipientName;
        private string recipientEmail;
        private string subject;
        private string body;
        private Nullable<DateTime> createdIn;
        private Nullable<DateTime> modifiedIn;
        private bool isDeleted;
        private int createdBy;
        private Nullable<int> modifiedBy;
        private string cCEmail;
        private string bCCEmail;
        private List<Emailattachment> emailattachments;
        private int poRequestStatus;
        string cCName;
        string sourceInboxId;
        int idPORequest;
        //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
        private Nullable<DateTime> emailsentat;
        private string senderIdPerson;
        private string toIdPerson;
        private string ccIdPerson;
        private string group;
        private string plant;
        #endregion     

        #region Constructor
        public Email()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public int PoRequestStatus
        {
            get
            {
                return poRequestStatus;
            }

            set
            {
                poRequestStatus = value;
                OnPropertyChanged("PoRequestStatus");
            }
        }
        [DataMember]
        public Int64 IdEmail
        {
            get
            {
                return idEmail;
            }

            set
            {
                idEmail = value;
                OnPropertyChanged("IdEmail");
            }
        }
        [DataMember]
        public string SenderName
        {
            get { return senderName; }
            set
            {
                senderName = value;
                OnPropertyChanged("SenderName");
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
        public string RecipientName
        {
            get { return recipientName; }
            set
            {
                recipientName = value;
                OnPropertyChanged("RecipientName");
            }
        }

        [DataMember]
        public string RecipientEmail
        {
            get { return recipientEmail; }
            set
            {
                recipientEmail = value;
                OnPropertyChanged("RecipientEmail");
            }
        }

        [DataMember]
        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                OnPropertyChanged("Subject");
            }
        }

        [DataMember]
        public string Body
        {
            get { return body; }
            set
            {
                body = value;
                OnPropertyChanged("Body");
            }
        }

        [DataMember]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [DataMember]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        [DataMember]
        public int CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public int? ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [DataMember]
        public string CCEmail
        {
            get { return cCEmail; }
            set
            {
                cCEmail = value;
                OnPropertyChanged("CCEmail");
            }
        }

        [DataMember]
        public string BCCEmail
        {
            get { return bCCEmail; }
            set
            {
                bCCEmail = value;
                OnPropertyChanged("BCCEmail");
            }
        }

        [DataMember]
        public virtual List<Emailattachment> EmailattachmentList
        {
            get { return emailattachments; }
            set
            {
                emailattachments = value;
                OnPropertyChanged("emailattachments");
            }
        }


        [DataMember]
        public string CCName
        {
            get { return cCName; }
            set
            {
                cCName = value;
                OnPropertyChanged("CCName");
            }
        }

        [DataMember]
        public string SourceInboxId
        {
            get { return sourceInboxId; }
            set
            {
                sourceInboxId = value;
                OnPropertyChanged("SourceInboxId");
            }
        }

        [DataMember]
        public int IdPORequest
        {
            get { return idPORequest; }
            set
            {
                idPORequest = value;
                OnPropertyChanged("IdPORequest");
            }
        }
        //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
        [DataMember]
        public DateTime? EmailSentAt
        {
            get { return emailsentat; }
            set
            {
                emailsentat = value;
                OnPropertyChanged("EmailSentAt");
            }
        }
        [DataMember]
        public string SenderIdPerson
        {
            get { return senderIdPerson; }
            set
            {
                senderIdPerson = value;
                OnPropertyChanged("SenderIdPerson");
            }
        }
        [DataMember]
        public string ToIdPerson
        {
            get { return toIdPerson; }
            set
            {
                toIdPerson = value;
                OnPropertyChanged("toIdPerson");
            }
        }
        [DataMember]
        public string CCIdPerson
        {
            get { return ccIdPerson; }
            set
            {
                ccIdPerson = value;
                OnPropertyChanged("CCIdPerson");
            }
        }
        [DataMember]
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }
        [DataMember]
        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        Int32 idCustomer;
        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set { idCustomer = value; }
        }


        int idPlant;
        [DataMember]
        public int IdPlant
        {
            get { return idPlant; }
            set
            {
                idPlant = value;
                OnPropertyChanged("IdPlant");
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
