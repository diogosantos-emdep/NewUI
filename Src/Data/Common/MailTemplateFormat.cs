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
    public class MailTemplateFormat : ModelBase, IDisposable
    {
        #region Fields

        string sendToUserName;
        string createdByUserName;
        string sentToMail;
        string accountName;
        string projectName;
        string carOEMName;
        string emailForSection;
        string parentName;//[Sudhir.Jangra][GEOS2-3693]
        string createdOrUpdated;//[Sudhir.Jangra][GEOS2-3693]
        string createdByEmail;//[Sudhir.jangra][GEOS2-3693]
        string createdUser;//[Sudhir.jangra][GEOS2-3693]
        byte? isNew;
        #endregion

        #region Constructor

        public MailTemplateFormat()
        {
        }

        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public string SendToUserName
        {
            get { return sendToUserName; }
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
            get { return createdByUserName; }
            set
            {
                createdByUserName = value;
                OnPropertyChanged("CreatedByUserName");
            }
        }

        [NotMapped]
        [DataMember]
        public string SentToMail
        {
            get { return sentToMail; }
            set
            {
                sentToMail = value;
                OnPropertyChanged("SentToMail");
            }
        }

        [NotMapped]
        [DataMember]
        public string AccountName
        {
            get { return accountName; }
            set
            {
                accountName = value;
                OnPropertyChanged("AccountName");
            }
        }

        [NotMapped]
        [DataMember]
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        [NotMapped]
        [DataMember]
        public string CarOEMName
        {
            get { return carOEMName; }
            set
            {
                carOEMName = value;
                OnPropertyChanged("CarOEMName");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmailForSection
        {
            get { return emailForSection; }
            set
            {
                emailForSection = value;
                OnPropertyChanged("EmailForSection");
            }
        }

        //[Sudhir.Jangra][GEOS2-3693]
        [DataMember]
        public string ParentName
        {
            get { return parentName; }
            set
            {
                parentName = value;
                OnPropertyChanged("ParentName");
            }
        }


        //[Sudhir.Jangra][GEOS2-3693]
        [DataMember]
        public string CreatedOrUpdated
        {
            get { return createdOrUpdated; }
            set
            {
                createdOrUpdated = value;
                OnPropertyChanged("CreatedOrUpdated");
            }
        }

        [DataMember]
        public string CreatedByEmail
        {
            get { return createdByEmail; }
            set
            {
                createdByEmail = value;
                OnPropertyChanged("CreatedByEmail");
            }
        }

        [DataMember]
        public string CreatedUser
        {
            get { return createdUser; }
            set
            {
                createdUser = value;
                OnPropertyChanged("CreatedUser");
            }
        }

        [DataMember]
        public byte? IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged("IsNew");
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

