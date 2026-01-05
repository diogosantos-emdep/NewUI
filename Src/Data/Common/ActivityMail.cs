using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class ActivityMail : ModelBase, IDisposable
    {
        #region Fields
        string sendToUserName;
        string createdByUserName;
        string activityType;
        string activitySubject;
        string activityDescription;
        string activityDueDate;
        string activitySentToMail;
        #endregion

        #region Constructor
        public ActivityMail()
        {

        }
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public string SendToUserName
        {
            get
            {
                return sendToUserName;
            }

            set
            {
                sendToUserName = value;
                OnPropertyChanged("SendToUserName");
            }
        }

        [NotMapped]
        [DataMember]
        public string CreatedByUserName
        {
            get
            {
                return createdByUserName;
            }

            set
            {
                createdByUserName = value;
                OnPropertyChanged("CreatedByUserName");
            }
        }

        [NotMapped]
        [DataMember]
        public string ActivityType
        {
            get
            {
                return activityType;
            }

            set
            {
                activityType = value;
                OnPropertyChanged("ActivityType");
            }
        }

        [NotMapped]
        [DataMember]
        public string ActivitySubject
        {
            get
            {
                return activitySubject;
            }

            set
            {
                activitySubject = value;
                OnPropertyChanged("ActivitySubject");
            }
        }

        [NotMapped]
        [DataMember]
        public string ActivityDescription
        {
            get
            {
                return activityDescription;
            }

            set
            {
                activityDescription = value;
                OnPropertyChanged("ActivityDescription");
            }
        }

        [NotMapped]
        [DataMember]
        public string ActivityDueDate
        {
            get
            {
                return activityDueDate;
            }

            set
            {
                activityDueDate = value;
                OnPropertyChanged("ActivityDueDate");
            }
        }


        [NotMapped]
        [DataMember]
        public string ActivitySentToMail
        {
            get
            {
                return activitySentToMail;
            }

            set
            {
                activitySentToMail = value;
                OnPropertyChanged("ActivitySentToMail");
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
