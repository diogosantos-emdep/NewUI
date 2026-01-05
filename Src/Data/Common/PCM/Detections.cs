using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class Detections :DetectionDetails, IDisposable
    {
        #region Fields

        UInt32 idDetections;
        string name;
        UInt32 idDetectionType;
        string parent;
        UInt32? idGroup;
        string groupName;
        string key;
        Int32? orderNumber;
        string statusAbbreviation;
        string statusHtmlColor;
        bool isCurrentDetection = false; //[sdeshpande][GEOS2-4098][26-12-2022]
        #endregion

        #region Constructor

        public Detections()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public uint IdDetections
        {
            get { return idDetections; }
            set
            {
                idDetections = value;
                OnPropertyChanged("IdDetections");
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
        [DataMember]
        public string Parent
        {
            get
            {
                return parent;
            }

            set
            {
                if (value != null)
                {
                    parent = value;
                    OnPropertyChanged("Parent");
                }
            }
        }

        [DataMember]
        public uint? IdGroup
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
        public int? OrderNumber
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

        [DataMember]
        public string GroupName
        {
            get
            {
                return groupName;
            }

            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        [DataMember]
        public string StatusAbbreviation
        {
            get
            {
                return statusAbbreviation;
            }

            set
            {
                statusAbbreviation = value;
                OnPropertyChanged("StatusAbbreviation");
            }
        }
        [DataMember]
        public string StatusHtmlColor
        {
            get
            {
                return statusHtmlColor;
            }

            set
            {
                statusHtmlColor = value;
                OnPropertyChanged("StatusHtmlColor");
            }
        }
        //[sdeshpande][GEOS2-4098][26-12-2022]
        [DataMember]
        public bool IsCurrentDetection
        {
            get
            {
                return isCurrentDetection;
            }

            set
            {
                isCurrentDetection = value;
                OnPropertyChanged("IsCurrentDetection");
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
