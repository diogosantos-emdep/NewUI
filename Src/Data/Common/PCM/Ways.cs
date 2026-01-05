using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class Ways: DetectionDetails, IDisposable
    {
        #region Fields

        UInt32 idWays;
        string name;
        UInt32 idDetectionType;
        byte isDefaultWay;
        bool isCurrentWays = false;//[sdeshpande][GEOS2-4098][26-12-2022]

        #endregion

        #region Constructor

        public Ways()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public byte IsDefaultWay
        {
            get { return isDefaultWay; }
            set
            {
                isDefaultWay = value;
                OnPropertyChanged("IsDefaultWay");
            }
        }

        [DataMember]
        public uint IdWays
        {
            get { return idWays; }
            set
            {
                idWays = value;
                OnPropertyChanged("IdWays");
            }
        }

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
        //[sdeshpande][GEOS2-4098][26-12-2022]
        [DataMember]
        public bool IsCurrentWays
        {
            get
            {
                return isCurrentWays;
            }

            set
            {
                isCurrentWays = value;
                OnPropertyChanged("IsCurrentWays");
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

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
