using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class DetectionOrderGroup : ModelBase, IDisposable
    {
        #region Fields

        string key;
        string name;
        string parent;
        UInt32 idGroup;
        UInt32 idDetection;
        Int32 orderNumber;

        #endregion

        #region Constructor

        public DetectionOrderGroup()
        {

        }

        #endregion


        #region Properties

        [DataMember]
        public string Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }

        [DataMember]
        public uint IdGroup
        {
            get
            {
                return idGroup;
            }

            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }

        [DataMember]
        public uint IdDetection
        {
            get
            {
                return idDetection;
            }

            set
            {
                idDetection = value;
                OnPropertyChanged("IdDetection");
            }
        }

        [DataMember]
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

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
        public int OrderNumber
        {
            get
            {
                return orderNumber;
            }

            set
            {
                orderNumber = value;
                OnPropertyChanged("OrderNumber");
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
