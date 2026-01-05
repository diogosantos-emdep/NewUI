using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class DetectionsByCatalogueItem : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idCatalogueItem;
        UInt32 idDetectionType;
        UInt32 idDetection;

        #endregion

        #region Constructor

        public DetectionsByCatalogueItem()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdCatalogueItem
        {
            get { return idCatalogueItem; }
            set
            {
                idCatalogueItem = value;
                OnPropertyChanged("IdCatalogueItem");
            }
        }

        [DataMember]
        public UInt32 IdDetectionType
        {
            get { return idDetectionType; }
            set
            {
                idDetectionType = value;
                OnPropertyChanged("IdDetectionType");
            }
        }

        [DataMember]
        public uint IdDetection
        {
            get { return idDetection; }
            set
            {
                idDetection = value;
                OnPropertyChanged("IdDetection");
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
