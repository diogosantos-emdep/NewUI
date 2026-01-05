using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [Table("caroems")]
    [DataContract]
    public class CarOEM : ModelBase, IDisposable
    {
        #region Fields
        Int32 idCarOEM;
        string name;
        byte[] carOEMFileBytes;
        ImageSource carOEMImage;
        #endregion

        #region Constructor
        public CarOEM()
        {
           
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdCarOEM")]
        [DataMember]
        public Int32 IdCarOEM
        {
            get
            {
                return idCarOEM;
            }

            set
            {
                idCarOEM = value;
                OnPropertyChanged("IdCarOEM");
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

        [DataMember]
        public byte[] CarOEMFileBytes
        {
            get
            {
                return carOEMFileBytes;
            }

            set
            {
                carOEMFileBytes = value;
                OnPropertyChanged("CarOEMFileBytes");
            }
        }

        [DataMember]
        public ImageSource CarOEMImage
        {
            get
            {
                return carOEMImage;
            }

            set
            {
                carOEMImage = value;
                OnPropertyChanged("CarOEMImage");
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
