using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    /// <summary>
    /// Transfer Request Object
    /// </summary>
    public class FileTransferRequest
    {
        /// <summary>
        /// Gets or sets File Name
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public byte[] Content { get; set; }
    }
}
