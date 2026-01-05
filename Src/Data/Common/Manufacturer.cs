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
    public class Manufacturer : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idManufacturer;
        string name;
        string code;
        #endregion

        #region Properties

        [Key]
        [Column("IdManufacturer")]
        [DataMember]
        public long IdManufacturer
        {
            get { return idManufacturer; }
            set
            {
                idManufacturer = value;
                OnPropertyChanged("IdManufacturer");
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


        [NotMapped]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        #endregion

        #region Constructor

        public Manufacturer()
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
