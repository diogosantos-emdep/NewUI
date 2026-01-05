using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class PCMAnnouncementEmailDetails
    {
        List<PCMCPTypesEmailDetails> pcmCPTypeDetails;
        List<PCMArticlesEmailDetails> pcmArticleDetails;
        List<PCMDetectionEmailDetails> detectionEmailDetails;

        [DataMember]
        public List<PCMDetectionEmailDetails> DetectionEmailDetails
        {
            get { return detectionEmailDetails; }
            set { detectionEmailDetails = value; }
        }
        [DataMember]
        public List<PCMCPTypesEmailDetails> PCMCPTypesDetails
        {
            get { return pcmCPTypeDetails; }
            set { pcmCPTypeDetails = value; }
        }
        [DataMember]
        public List<PCMArticlesEmailDetails> PCMArticleDetails
        {
            get { return pcmArticleDetails; }
            set { pcmArticleDetails = value; }
        }
    }
}
