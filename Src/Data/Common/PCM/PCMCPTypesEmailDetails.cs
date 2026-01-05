using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{//[Sudhir.Jangra][GEOS2-3891][21/02/2023]
    [DataContract]
    public class PCMCPTypesEmailDetails
    {
        #region fields
        DateTime cpCreatedIn;
        DateTime cpModifiedIn;
        string cpName;
        string cpCode;
        #endregion

        #region Properties
        [DataMember]
        public DateTime CPCreatedIn
        {
            get { return cpCreatedIn; }
            set { cpCreatedIn = value; }
        }
        [DataMember]
        public DateTime CPModifiedIn
        {
            get { return cpModifiedIn; }
            set { cpModifiedIn = value; }
        }
        [DataMember]
        public string CPName
        {
            get { return cpName; }
            set { cpName = value; }
        }
        [DataMember]
        public string CPCode
        {
            get { return cpCode; }
            set { cpCode = value; }
        }
        Int32 cpType;
        [DataMember]
        public Int32 CPType
        {
            get { return cpType; }
            set { cpType = value; }
        }

        int week;
        [DataMember]
        public int Week
        {
            get { return week; }
            set { week = value; }
        }

        UInt64 idTemplate;
        [DataMember]
        public UInt64 IdTemplate
        {
            get { return idTemplate; }
            set
            {
                idTemplate = value;
            }
        }
        UInt64 idCPType;
        [DataMember]
        public UInt64 IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
            }
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
