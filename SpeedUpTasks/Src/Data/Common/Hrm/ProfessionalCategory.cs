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
    [Table("professional_categories")]
    [DataContract]
    public class ProfessionalCategory : ModelBase, IDisposable
    {
        #region Fields

        UInt16 idProfessionalCategory;
        string name;
        UInt16 idProfessionalCategoryParent;

        #endregion

        #region Properties

        [Key]
        [Column("IdProfessionalCategory")]
        [DataMember]
        public ushort IdProfessionalCategory
        {
            get { return idProfessionalCategory; }
            set
            {
                idProfessionalCategory = value;
                OnPropertyChanged("IdProfessionalCategory");
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

        [Column("IdProfessionalCategoryParent")]
        [DataMember]
        public ushort IdProfessionalCategoryParent
        {
            get { return idProfessionalCategoryParent; }
            set
            {
                idProfessionalCategoryParent = value;
                OnPropertyChanged("IdProfessionalCategoryParent");
            }
        }

        #endregion

        #region Constructor

        public ProfessionalCategory()
        {
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
