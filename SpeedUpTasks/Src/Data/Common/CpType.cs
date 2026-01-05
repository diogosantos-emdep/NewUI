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
    [Table("cptypes")]
    [DataContract]
    public class CpType : ModelBase, IDisposable
    {
        #region Fields

        byte idCPType;
        string name;
        string description;

        

        #endregion

        #region Constructor

        public CpType()
        {

        }

        #endregion

        #region Properties

        [Key]
        [Column("IdCPType")]
        [DataMember]
        public byte IdCPType
        {
            get { return idCPType; }
            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
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

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
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
