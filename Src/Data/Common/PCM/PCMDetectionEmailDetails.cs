using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    //[Sudhir.Jangra][GEOS2-3891][21/02/2023
    public class PCMDetectionEmailDetails
    {
        #region Fields
        int id;
        DateTime detectionCreatedIn;
        DateTime detectionModifiedIn;
        string detectionName;
        int detectionType;

        #endregion

        #region Properties
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [DataMember]
        public DateTime DetectionCreatedIn
        {
            get { return detectionCreatedIn; }
            set { detectionCreatedIn = value; }
        }
        [DataMember]
        public DateTime DetectionModifiedIn
        {
            get { return detectionModifiedIn; }
            set { detectionModifiedIn = value; }
        }
        [DataMember]
        public string DetectionName
        {
            get { return detectionName; }
            set { detectionName=value; }
        }
        [DataMember]
        public int DetectionType
        {
            get { return detectionType; }
            set { detectionType = value; }
        }

        int week;
        [DataMember]
        public int Week
        {
            get { return week; }
            set { week = value; }
        }

        string type;
        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        bool newChangeType;
        [DataMember]
        public bool NewChangeType
        {
            get { return newChangeType; }
            set { newChangeType = value; }
        }

        bool updateChangeType;
        [DataMember]
        public bool UpdateChangeType
        {
            get { return updateChangeType; }
            set { updateChangeType = value; }
        }

        DateTime detectionDate;
        [DataMember]
        public DateTime ReportDate
        {
            get { return detectionDate; }
            set { detectionDate = value; }
        }

        #endregion
    }
}
