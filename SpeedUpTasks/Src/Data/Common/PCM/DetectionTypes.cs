using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract(IsReference = true)]
    public class DetectionTypes : ModelBase, IDisposable
    {

        #region Fields
        UInt32 idDetectionType;
        string name;
        UInt64 sortOrder;
        string color;
        #endregion

        #region Properties
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
        public string Name
        {
            get { return name; }
            set
            {
                this.name = value;
                OnPropertyChanged("Name");
            }
        }

        [DataMember]
        public UInt64 SortOrder
        {
            get { return sortOrder; }
            set
            {
                this.sortOrder = value;
                OnPropertyChanged("SortOrder");
            }
        }

        [DataMember]
        public string Color
        {
            get { return color; }
            set
            {
                this.color = value;
                OnPropertyChanged("Color");
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
