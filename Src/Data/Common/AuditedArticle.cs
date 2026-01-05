using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class AuditedArticle
    {

        Int32 idWarehouseLocation;
        [DataMember]
        public Int32 IdWarehouseLocation
        {
            get { return idWarehouseLocation; }
            set { idWarehouseLocation = value; }
        }

        Int32 idWarehouseInventoryAuditArticle;
        [DataMember]
        public Int32 IdWarehouseInventoryAuditArticle
        {
            get { return idWarehouseInventoryAuditArticle; }
            set { idWarehouseInventoryAuditArticle = value; }
        }

        string fullName;
        [DataMember]
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        string reference;
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }

    }
}
