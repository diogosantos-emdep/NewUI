using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.APM
{
    //[nsatpute][24-10-2024][GEOS2-6018]
    [DataContract]
    public class ActionPlanComment : ModelBase, IDisposable
    {
        #region Fields

        uint id;
        ulong idCPType;
        uint idUser;
        DateTime datetime;
        string comment;
        string userName;
        uint idLogEntryType;
        People people;
        uint createdBy;
        uint modifiedBy;
        DateTime modifiedDate;
        long idActionPlan;
        bool isDeleteButtonEnabled;
        #endregion

        #region Constructor
        public ActionPlanComment()
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public uint Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }


        [DataMember]
        public uint IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [DataMember]
        public DateTime Datetime
        {
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }

        [DataMember]
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

        [DataMember]
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        [DataMember]
        public uint IdLogEntryType
        {
            get { return idLogEntryType; }
            set
            {
                idLogEntryType = value;
                OnPropertyChanged("IdLogEntryType");
            }
        }

        [DataMember]
        public People People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }
        [DataMember]
        public uint CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }
        [DataMember]
        public uint ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get { return modifiedDate; }
            set
            {
                modifiedDate = value;
                OnPropertyChanged("ModifiedDate");
            }
        }


        [NotMapped]
        [DataMember]
        public long IdActionPlan
        {
            get { return idActionPlan; }
            set { idActionPlan = value; OnPropertyChanged("IdActionPlan"); }
        }

        [NotMapped]
        [DataMember]
        public bool IsDeleteButtonEnabled
        {
            get { return isDeleteButtonEnabled; }
            set
            {
                isDeleteButtonEnabled = value;
                OnPropertyChanged("IsDeleteButtonEnabled");
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
