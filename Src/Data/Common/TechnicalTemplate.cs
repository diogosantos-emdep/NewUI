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
    [Table("technicaltemplates")]
    [DataContract]
    public class TechnicalTemplate : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idTechnicalTemplate;
        string name;

        #endregion

        #region Constructor

        public TechnicalTemplate()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdTechnicalTemplate")]
        [DataMember]
        public uint IdTechnicalTemplate
        {
            get { return idTechnicalTemplate; }
            set
            {
                idTechnicalTemplate = value;
                OnPropertyChanged("IdTechnicalTemplate");
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
