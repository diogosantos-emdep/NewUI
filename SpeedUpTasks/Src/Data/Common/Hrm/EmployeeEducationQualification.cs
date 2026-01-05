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
    [Table("employee_education_qualifications")]
    [DataContract]
    public class EmployeeEducationQualification : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idEmployeeEducationQualification;
        Int32 idEmployee;
        UInt32 idEducationQualification;
        string qualificationName;
        string qualificationEntity;
        string qualificationClassification;
        DateTime? qualificationStartDate;
        DateTime? qualificationEndDate;
        string qualificationFileName;
        byte[] qualificationFileInBytes;
        string qualificationRemarks;

        EducationQualification educationQualification;
        bool isQualificationFileDeleted;
        Attachment attachment;
        string oldFileName;

        string fileName;

        #endregion

        #region Constructor

        public EmployeeEducationQualification()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeEducationQualification")]
        [DataMember]
        public ulong IdEmployeeEducationQualification
        {
            get { return idEmployeeEducationQualification; }
            set
            {
                idEmployeeEducationQualification = value;
                OnPropertyChanged("IdEmployeeEducationQualification");
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

        [Column("IdEducationQualification")]
        [DataMember]
        public uint IdEducationQualification
        {
            get { return idEducationQualification; }
            set
            {
                idEducationQualification = value;
                OnPropertyChanged("IdEducationQualification");
            }
        }

        [Column("QualificationName")]
        [DataMember]
        public string QualificationName
        {
            get { return qualificationName; }
            set
            {
                qualificationName = value;
                OnPropertyChanged("QualificationName");
            }
        }

        [Column("QualificationEntity")]
        [DataMember]
        public string QualificationEntity
        {
            get { return qualificationEntity; }
            set
            {
                qualificationEntity = value;
                OnPropertyChanged("QualificationEntity");
            }
        }

        [Column("QualificationClassification")]
        [DataMember]
        public string QualificationClassification
        {
            get { return qualificationClassification; }
            set
            {
                qualificationClassification = value;
                OnPropertyChanged("QualificationClassification");
            }
        }

        [Column("QualificationStartDate")]
        [DataMember]
        public DateTime? QualificationStartDate
        {
            get { return qualificationStartDate; }
            set
            {
                qualificationStartDate = value;
                OnPropertyChanged("QualificationStartDate");
            }
        }

        [Column("QualificationEndDate")]
        [DataMember]
        public DateTime? QualificationEndDate
        {
            get { return qualificationEndDate; }
            set
            {
                qualificationEndDate = value;
                OnPropertyChanged("QualificationEndDate");
            }
        }

        [Column("QualificationFileName")]
        [DataMember]
        public string QualificationFileName
        {
            get { return qualificationFileName; }
            set
            {
                qualificationFileName = value;
                OnPropertyChanged("QualificationFileName");
            }
        }

        [Column("QualificationRemarks")]
        [DataMember]
        public string QualificationRemarks
        {
            get { return qualificationRemarks; }
            set
            {
                qualificationRemarks = value;
                OnPropertyChanged("QualificationRemarks");
            }
        }

        [NotMapped]
        [DataMember]
        public EducationQualification EducationQualification
        {
            get { return educationQualification; }
            set
            {
                educationQualification = value;
                OnPropertyChanged("EducationQualification");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] QualificationFileInBytes
        {
            get { return qualificationFileInBytes; }
            set
            {
                qualificationFileInBytes = value;
                OnPropertyChanged("QualificationFileInBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsQualificationFileDeleted
        {
            get { return isQualificationFileDeleted; }
            set
            {
                isQualificationFileDeleted = value;
                OnPropertyChanged("IsQualificationFileDeleted");
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
        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
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
            EmployeeEducationQualification employeeEducationQualification = (EmployeeEducationQualification)this.MemberwiseClone();

            if (employeeEducationQualification.EducationQualification != null)
                employeeEducationQualification.EducationQualification = (EducationQualification)this.EducationQualification.Clone();

            if (employeeEducationQualification.Attachment != null)
                employeeEducationQualification.Attachment = (Attachment)this.Attachment.Clone();

            return employeeEducationQualification;
        }

        #endregion
    }
}
