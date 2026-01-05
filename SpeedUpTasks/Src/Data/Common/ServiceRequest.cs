using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("project_milestone_dates")]
    [DataContract(IsReference = true)]
    public class ServiceRequest:ModelBase,IDisposable
    {
        #region  Fields
        Int64 idServiceRequest;
        string code;
        string description;
        byte idCategory;
        Int32 postedBy;
        Int32? idCustomer;
        byte idStatus;
        byte idPriority;
        string replyListCustomer;
        DateTime startDate;
        DateTime endDate;
        string solution;
        string attachment;
        string replyListEmdep;
        string assignedTo;
        Int32 responsible;
        Int32 idProduct;
        string relatedComplaint;
        string cause;
        sbyte isInternal;
        Int64 idServiceRequestType;
        DateTime? expectedEndDate;
        #endregion

        #region Constructor
        public ServiceRequest()
        {

        }
        #endregion

        #region Properties

        [Key]
        [Column("IdServiceRequest")]
        [DataMember]
        public Int64 IdServiceRequest
        {
            get
            {
                return idServiceRequest;
            }
            set
            {
                idServiceRequest = value;
                OnPropertyChanged("IdServiceRequest");
            }
        }

        [Column("Code")]
        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("IdCategory")]
        [DataMember]
        public byte IdCategory
        {
            get
            {
                return idCategory;
            }
            set
            {
                idCategory = value;
                OnPropertyChanged("IdCategory");
            }
        }

        [Column("PostedBy")]
        [DataMember]
        public Int32 PostedBy
        {
            get
            {
                return postedBy;
            }

            set
            {
                postedBy = value;
                OnPropertyChanged("PostedBy");
            }
        }

        [Column("IdCustomer")]
        [DataMember]
        public Int32? IdCustomer
        {
            get
            {
                return idCustomer;
            }

            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [Column("IdStatus")]
        [DataMember]
        public byte IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [Column("IdPriority")]
        [DataMember]
        public byte IdPriority
        {
            get
            {
                return idPriority;
            }

            set
            {
                idPriority = value;
                OnPropertyChanged("IdPriority");
            }
        }

        [Column("ReplyListCustomer")]
        [DataMember]
        public string ReplyListCustomer
        {
            get
            {
                return replyListCustomer;
            }

            set
            {
                replyListCustomer = value;
                OnPropertyChanged("ReplyListCustomer");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Column("EndDate")]
        [DataMember]
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [Column("Solution")]
        [DataMember]
        public string Solution
        {
            get
            {
                return solution;
            }

            set
            {
                solution = value;
                OnPropertyChanged("Solution");
            }
        }

        [Column("Attachment")]
        [DataMember]
        public string Attachment
        {
            get
            {
                return attachment;
            }

            set
            {
                attachment = value;
                OnPropertyChanged("Attachment");
            }
        }

        [Column("ReplyListEmdep")]
        [DataMember]
        public string ReplyListEmdep
        {
            get
            {
                return replyListEmdep;
            }

            set
            {
                replyListEmdep = value;
                OnPropertyChanged("ReplyListEmdep");
            }
        }

        [Column("AssignedTo")]
        [DataMember]
        public string AssignedTo
        {
            get
            {
                return assignedTo;
            }

            set
            {
                assignedTo = value;
                OnPropertyChanged("AssignedTo");
            }
        }

        [Column("Responsible")]
        [DataMember]
        public Int32 Responsible
        {
            get
            {
                return responsible;
            }

            set
            {
                responsible = value;
                OnPropertyChanged("Responsible");
            }
        }

        [Column("IdProduct")]
        [DataMember]
        public Int32 IdProduct
        {
            get
            {
                return idProduct;
            }

            set
            {
                idProduct = value;
                OnPropertyChanged("IdProduct");
            }
        }

        [Column("RelatedComplaint")]
        [DataMember]
        public string RelatedComplaint
        {
            get
            {
                return relatedComplaint;
            }

            set
            {
                relatedComplaint = value;
                OnPropertyChanged("RelatedComplaint");
            }
        }

        [Column("Cause")]
        [DataMember]
        public string Cause
        {
            get
            {
                return cause;
            }

            set
            {
                cause = value;
                OnPropertyChanged("Cause");
            }
        }

        [Column("IsInternal")]
        [DataMember]
        public sbyte IsInternal
        {
            get
            {
                return isInternal;
            }

            set
            {
                isInternal = value;
                OnPropertyChanged("IsInternal");
            }
        }

        [Column("IdServiceRequestType")]
        [DataMember]
        public Int64 IdServiceRequestType
        {
            get
            {
                return idServiceRequestType;
            }

            set
            {
                idServiceRequestType = value;
                OnPropertyChanged("IdServiceRequestType");
            }
        }

        [Column("ExpectedEndDate")]
        [DataMember]
        public DateTime? ExpectedEndDate
        {
            get
            {
                return expectedEndDate;
            }

            set
            {
                expectedEndDate = value;
                OnPropertyChanged("ExpectedEndDate");
            }
        }

        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
