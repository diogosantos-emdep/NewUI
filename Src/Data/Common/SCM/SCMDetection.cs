using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class SCMDetection : ModelBase, IDisposable
    {
        #region Fields
        private string name;
        private ushort idDetection;
        private int quantity;
        int idDetectionType;
        string detectionType;
        #endregion

        #region Properties

        [DataMember]
        public ushort IdDetection
        {
            get { return idDetection; }
            set
            {
                idDetection = value;
                OnPropertyChanged(nameof(IdDetection));
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
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }


        [DataMember]
        public string DetectionType
        {
            get
            {
                return detectionType;
            }

            set
            {
                detectionType = value;
                OnPropertyChanged("DetectionType");
            }
        }

        [DataMember]
        public int IdDetectionType
        {
            get { return idDetectionType; }
            set
            {
                idDetectionType = value;
                OnPropertyChanged(nameof(IdDetectionType));
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
