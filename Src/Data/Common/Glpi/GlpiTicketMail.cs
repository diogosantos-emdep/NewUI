using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common.Glpi
{
    [DataContract]
    public class GlpiTicketMail
    {
        #region Fields
        string title;
        string requesters;
        DateTime openingDate;
        string requestSource;
        string urgency;
        string priority;
        string description;
        string activeUserMailId;
        Dictionary<string, byte[]> attachments;
        #endregion

        #region Properties

        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }


       [DataMember]
        public string Requesters
        {
            get { return requesters; }
            set { requesters = value; }
        }

        [DataMember]
        public DateTime OpeningDate
        {
            get { return openingDate; }
            set { openingDate = value; }
        }

        [DataMember]
        public string RequestSource
        {
            get { return requestSource; }
            set { requestSource = value; }
        }

        [DataMember]
        public string Urgency
        {
            get { return urgency; }
            set { urgency = value; }
        }

        [DataMember]
        public string Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        public string ActiveUserMailId
        {
            get { return activeUserMailId; }
            set { activeUserMailId = value; }
        }

        [DataMember]
        public Dictionary<string, byte[]> Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        #endregion
    }
}
