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
    [Table("projects")]
    [DataContract]
    public class GeosProject : ModelBase, IDisposable
    {
        #region Fields
        Int64 idProject;
        string code;
        string title;
        Int64 idCustomer;
        string description;
        string contact;
        Int64 probabilityOfSuccess;
        double _value;
        string idCurrency;
        Int32 createdBy;
        DateTime createdIn;
        Int32 modifiedBy;
        DateTime modifiedIn;
        string comments;
        Int64 idSite;
        Int64 idProjectStatusType;
        DateTime closeDate;
        #endregion

        #region Constructor
        public GeosProject()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdProject")]
        [DataMember]
        public Int64 IdProject
        {
            get
            {
                return idProject;
            }

            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
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

        [Column("Title")]
        [DataMember]
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        [Column("IdCustomer")]
        [DataMember]
        public Int64 IdCustomer
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

        [Column("Contact")]
        [DataMember]
        public string Contact
        {
            get
            {
                return contact;
            }

            set
            {
                contact = value;
                OnPropertyChanged("Contact");
            }
        }

        [Column("ProbabilityOfSuccess")]
        [DataMember]
        public Int64 ProbabilityOfSuccess
        {
            get
            {
                return probabilityOfSuccess;
            }

            set
            {
                probabilityOfSuccess = value;
                OnPropertyChanged("ProbabilityOfSuccess");
            }
        }

        [Column("Value")]
        [DataMember]
        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        [Column("IdCurrency")]
        [DataMember]
        public string IdCurrency
        {
            get
            {
                return idCurrency;
            }

            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("CreatedIn")]
        [DataMember]
        public DateTime CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public Int64 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("IdProjectStatusType")]
        [DataMember]
        public Int64 IdProjectStatusType
        {
            get
            {
                return idProjectStatusType;
            }

            set
            {
                idProjectStatusType = value;
                OnPropertyChanged("IdProjectStatusType");
            }
        }

        [Column("CloseDate")]
        [DataMember]
        public DateTime CloseDate
        {
            get
            {
                return closeDate;
            }

            set
            {
                closeDate = value;
                OnPropertyChanged("CloseDate");
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
