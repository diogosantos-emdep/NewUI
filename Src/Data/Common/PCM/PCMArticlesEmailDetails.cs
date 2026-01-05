using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{//[Sudhir.Jangra][GEOS2-3891][21/02/2023]
    [DataContract]
    public class PCMArticlesEmailDetails
    {
        #region Fields
        DateTime articleCreatedIn;
        DateTime articleModifiedIn;
        string articleReference;
        string articleDescription;
        #endregion

        #region Properties
        [DataMember]
        public DateTime ArticleCreatedIn
        {
            get { return articleCreatedIn; }
            set { articleCreatedIn = value; }
        }
        [DataMember]
        public DateTime ArticleModifiedIn
        {
            get { return articleModifiedIn; }
            set { articleModifiedIn = value; }
        }
        [DataMember]
        public string ArticleReference
        {
            get { return articleReference; }
            set { articleReference = value; }
        }
        [DataMember]
        public string ArticleDescription
        {
            get { return articleDescription; }
            set { articleDescription = value; }
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
