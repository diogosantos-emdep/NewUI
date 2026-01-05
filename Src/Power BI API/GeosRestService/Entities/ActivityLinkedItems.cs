using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class ActivityLinkedItem
    {
        [DataMember]
        public string CarProject { get; set; }
        [DataMember]
        public string Caroem { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataMember]
        public string Competitor { get; set; }
        [DataMember]
        public string Oppurtunities { get; set; }
    }
}
