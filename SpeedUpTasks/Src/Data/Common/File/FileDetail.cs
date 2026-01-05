using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.File
{
    [DataContract]
    public class FileDetail : ModelBase, IDisposable
    {
        #region Fields

        string fileName;
        Byte[] fileByte;

        #endregion

        #region Constructor
        public FileDetail()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        [DataMember]
        public Byte[] FileByte
        {
            get { return fileByte; }
            set
            {
                fileByte = value;
                OnPropertyChanged("FileByte");
            }
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
