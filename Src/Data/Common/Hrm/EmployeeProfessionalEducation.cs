using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("employee_professional_education")]
    [DataContract]
    public class EmployeeProfessionalEducation : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idEmployeeProfessionalEducation;
        Int32 idEmployee;
        Int32 idType;
        string name;
        string entity;
        DateTime startDate;
        DateTime endDate;
        UInt16 durationValue;
        Int32 idDurationUnit;
        string fileName;
        string remarks;

        LookupValue type;
        LookupValue durationUnit;

        Attachment attachment;
        string oldFileName;
        bool isProfessionalFileDeleted;
        byte[] professionalFileInBytes;
        private bool isEnableForDeleteBtn;
        Int32 idProfessionalTraining;
        #endregion

        #region Constructor
        public EmployeeProfessionalEducation()
        {
        }
        #endregion

        #region Properties

        [Column("IdProfessionalTraining")]
        [DataMember]
        public int IdProfessionalTraining
        {
            get { return idProfessionalTraining; }
            set
            {
                idProfessionalTraining = value;
                OnPropertyChanged("IdProfessionalTraining");
            }
        }

        [DataMember]
        public bool IsEnableForDeleteBtn
        {
            get { return isEnableForDeleteBtn; }
            set
            {
                isEnableForDeleteBtn = value;
                OnPropertyChanged("IsEnableForDeleteBtn");
            }
        }
        [Key]
        [Column("IdEmployeeProfessionalEducation")]
        [DataMember]
        public ulong IdEmployeeProfessionalEducation
        {
            get { return idEmployeeProfessionalEducation; }
            set
            {
                idEmployeeProfessionalEducation = value;
                OnPropertyChanged("IdEmployeeProfessionalEducation");
            }
        }

        [Column("IdEmployee")]
        [DataMember]
        public int IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [Column("IdType")]
        [DataMember]
        public int IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("Entity")]
        [DataMember]
        public string Entity
        {
            get { return entity; }
            set
            {
                entity = value;
                OnPropertyChanged("Entity");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public DateTime StartDate
        {
            get { return startDate; }
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
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [Column("DurationValue")]
        [DataMember]
        public ushort DurationValue
        {
            get { return durationValue; }
            set
            {
                durationValue = value;
                OnPropertyChanged("DurationValue");
            }
        }

        [Column("IdDurationUnit")]
        [DataMember]
        public int IdDurationUnit
        {
            get { return idDurationUnit; }
            set
            {
                idDurationUnit = value;
                OnPropertyChanged("IdDurationUnit");
            }
        }

        [Column("FileName")]
        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        [Column("Remarks")]
        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue DurationUnit
        {
            get { return durationUnit; }
            set
            {
                durationUnit = value;
                OnPropertyChanged("DurationUnit");
            }
        }

        [NotMapped]
        [DataMember]
        public Attachment Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged("Attachment");
            }
        }

        [NotMapped]
        [DataMember]
        public string OldFileName
        {
            get { return oldFileName; }
            set
            {
                oldFileName = value;
                OnPropertyChanged("OldFileName");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] ProfessionalFileInBytes
        {
            get { return professionalFileInBytes; }
            set
            {
                professionalFileInBytes = value;
                OnPropertyChanged("ProfessionalFileInBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsProfessionalFileDeleted
        {
            get { return isProfessionalFileDeleted; }
            set
            {
                isProfessionalFileDeleted = value;
                OnPropertyChanged("IsProfessionalFileDeleted");
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
            EmployeeProfessionalEducation employeeProfessionalEducation = (EmployeeProfessionalEducation)this.MemberwiseClone();

            if (employeeProfessionalEducation.Type != null)
                employeeProfessionalEducation.Type = (LookupValue)this.Type.Clone();

            if (employeeProfessionalEducation.DurationUnit != null)
                employeeProfessionalEducation.DurationUnit = (LookupValue)this.DurationUnit.Clone();

            if (employeeProfessionalEducation.Attachment != null)
                employeeProfessionalEducation.Attachment = (Attachment)this.Attachment.Clone();

            return employeeProfessionalEducation;
        }

        #endregion
    }
}
