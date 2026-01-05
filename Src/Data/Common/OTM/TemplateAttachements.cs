using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    [DataContract]
    public class TemplateAttachements : ModelBase, IDisposable
    {
        #region Properties
        private string savedFileName;
        private byte[] templateAttachementsDocInBytes;
        #endregion

        #region Declartion
        [DataMember]
        public string SavedFileName
        {
            get
            {
                return savedFileName;
            }
            set
            {
                savedFileName = value;
                OnPropertyChanged("SavedFileName");
            }
        }

        [DataMember]
        public byte[] TemplateAttachementsDocInBytes
        {
            get
            {
                return templateAttachementsDocInBytes;
            }
            set
            {
                templateAttachementsDocInBytes = value;
                OnPropertyChanged("TemplateAttachementsDocInBytes");
            }
        }

        #endregion


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
