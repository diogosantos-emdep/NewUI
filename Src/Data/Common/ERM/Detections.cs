using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class Detections : ModelBase, IDisposable
    {

        #region Field
        private Int32 cPProductID;
        private Int32 idDetectionType;
        private Int32 nO_OF_Way;
        private string nO_OF_Way_String; //[nsatpute][18.07.2025][GEOS2-8935]
        private string wayName;
        private Int32 idDetection;
        #endregion
        #region Property


        [DataMember]
        public Int32 IdDetection
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
        public Int32 CPProductID
        {
            get
            {
                return cPProductID;
            }

            set
            {
                cPProductID = value;
                OnPropertyChanged("CPProductID");
            }
        }

        [DataMember]
        public Int32 IdDetectionType
        {
            get
            {
                return idDetectionType;
            }

            set
            {
                idDetectionType = value;
                OnPropertyChanged("IdDetectionType");
            }
        }

        [DataMember]
        public Int32 NO_OF_Way
        {
            get
            {
                return nO_OF_Way;
            }

            set
            {
                nO_OF_Way = value;
                OnPropertyChanged("NO_OF_Way");
            }
        }
        //[nsatpute][18.07.2025][GEOS2-8935]
        [DataMember]
        public string NO_OF_Way_String
        {
            get
            {
                return nO_OF_Way_String;
            }

            set
            {
                nO_OF_Way_String = value;
                OnPropertyChanged("NO_OF_Way_String");
            }
        }

        [DataMember]
        public string WayName
        {
            get
            {
                return wayName;
            }

            set
            {
                wayName = value;
                OnPropertyChanged("WayName");
            }
        }
        
        #endregion


        #region Constructor
        public Detections()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
