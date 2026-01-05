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
    [Table("employee_family_members")]
    [DataContract]
    public class EmployeeFamilyMember : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idEmployeeFamilyMember;
        Int32 idEmployee;
        string familyMemberFirstName;
        string familyMemberLastName;
        string familyMemberNativeName;
        DateTime? familyMemberBirthDate;
        Int64 familyMemberIdNationality;
        UInt32 familyMemberIdRelationshipType;
        UInt32 familyMemberIdGender;
        UInt16 familyMemberDisability;
        byte familyMemberIsDependent;
        string familyMemberRemarks;

        Country familyMemberNationality;
        LookupValue familyMemberRelationshipType;
        LookupValue familyMemberGender;

        #endregion

        #region Constructor

        public EmployeeFamilyMember()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeFamilyMember")]
        [DataMember]
        public ulong IdEmployeeFamilyMember
        {
            get { return idEmployeeFamilyMember; }
            set
            {
                idEmployeeFamilyMember = value;
                OnPropertyChanged("IdEmployeeFamilyMember");
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

        [Column("FamilyMemberFirstName")]
        [DataMember]
        public string FamilyMemberFirstName
        {
            get { return familyMemberFirstName; }
            set
            {
                familyMemberFirstName = value;
                OnPropertyChanged("FamilyMemberFirstName");
            }
        }

        [Column("FamilyMemberLastName")]
        [DataMember]
        public string FamilyMemberLastName
        {
            get { return familyMemberLastName; }
            set
            {
                familyMemberLastName = value;
                OnPropertyChanged("FamilyMemberLastName");
            }
        }

        [Column("FamilyMemberNativeName")]
        [DataMember]
        public string FamilyMemberNativeName
        {
            get { return familyMemberNativeName; }
            set
            {
                familyMemberNativeName = value;
                OnPropertyChanged("FamilyMemberNativeName");
            }
        }

        [Column("FamilyMemberBirthDate")]
        [DataMember]
        public DateTime? FamilyMemberBirthDate
        {
            get { return familyMemberBirthDate; }
            set
            {
                familyMemberBirthDate = value;
                OnPropertyChanged("FamilyMemberBirthDate");
            }
        }

        [Column("FamilyMemberIdNationality")]
        [DataMember]
        public Int64 FamilyMemberIdNationality
        {
            get { return familyMemberIdNationality; }
            set
            {
                familyMemberIdNationality = value;
                OnPropertyChanged("FamilyMemberIdNationality");
            }
        }

        [Column("FamilyMemberIdRelationshipType")]
        [DataMember]
        public uint FamilyMemberIdRelationshipType
        {
            get { return familyMemberIdRelationshipType; }
            set
            {
                familyMemberIdRelationshipType = value;
                OnPropertyChanged("FamilyMemberIdRelationshipType");
            }
        }

        [Column("FamilyMemberIdGender")]
        [DataMember]
        public uint FamilyMemberIdGender
        {
            get { return familyMemberIdGender; }
            set
            {
                familyMemberIdGender = value;
                OnPropertyChanged("FamilyMemberIdGender");
            }
        }

        [Column("FamilyMemberDisability")]
        [DataMember]
        public ushort FamilyMemberDisability
        {
            get { return familyMemberDisability; }
            set
            {
                familyMemberDisability = value;
                OnPropertyChanged("FamilyMemberDisability");
            }
        }

        [Column("FamilyMemberIsDependent")]
        [DataMember]
        public byte FamilyMemberIsDependent
        {
            get { return familyMemberIsDependent; }
            set
            {
                familyMemberIsDependent = value;
                OnPropertyChanged("FamilyMemberIsDependent");
            }
        }

        [Column("FamilyMemberRemarks")]
        [DataMember]
        public string FamilyMemberRemarks
        {
            get { return familyMemberRemarks; }
            set
            {
                familyMemberRemarks = value;
                OnPropertyChanged("FamilyMemberRemarks");
            }
        }

        [NotMapped]
        [DataMember]
        public Country FamilyMemberNationality
        {
            get { return familyMemberNationality; }
            set
            {
                familyMemberNationality = value;
                OnPropertyChanged("FamilyMemberNationality");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue FamilyMemberGender
        {
            get { return familyMemberGender; }
            set
            {
                familyMemberGender = value;
                OnPropertyChanged("FamilyMemberGender");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue FamilyMemberRelationshipType
        {
            get { return familyMemberRelationshipType; }
            set
            {
                familyMemberRelationshipType = value;
                OnPropertyChanged("FamilyMemberRelationshipType");
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
            EmployeeFamilyMember employeeFamilyMember = (EmployeeFamilyMember)this.MemberwiseClone();

            if (employeeFamilyMember.FamilyMemberNationality != null)
                employeeFamilyMember.FamilyMemberNationality = (Country)this.FamilyMemberNationality.Clone();

            if (employeeFamilyMember.FamilyMemberRelationshipType != null)
                employeeFamilyMember.FamilyMemberRelationshipType = (LookupValue)this.FamilyMemberRelationshipType.Clone();

            if (employeeFamilyMember.FamilyMemberGender != null)
                employeeFamilyMember.FamilyMemberGender = (LookupValue)this.FamilyMemberGender.Clone();

            return employeeFamilyMember;
        }

        #endregion
    }
}
