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
    [Table("education_qualifications")]
    [DataContract]
    public class EducationQualification : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idEducationQualification;
        string name;
        UInt32 qualificationLevel;

        #endregion

        #region Constructor

        public EducationQualification()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdEducationQualification")]
        [DataMember]
        public uint IdEducationQualification
        {
            get
            {
                return idEducationQualification;
            }

            set
            {
                idEducationQualification = value;
                OnPropertyChanged("IdEducationQualification");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("QualificationLevel")]
        [DataMember]
        public uint QualificationLevel
        {
            get
            {
                return qualificationLevel;
            }

            set
            {
                qualificationLevel = value;
                OnPropertyChanged("QualificationLevel");
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
