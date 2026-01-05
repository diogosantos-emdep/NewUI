using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    //[Rahul.Gadhave][GEOS2-9880][Date:28-11-2025]
    [DataContract]
    public class PdfResultDto : ModelBase, IDisposable
    {

        #region Properties
        [DataMember]
        public byte[] FileBytes { get; set; }
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FileFullPath { get; set; }
        #endregion

        #region Constructor
        public PdfResultDto()
        {
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
        #endregion
    }
}
