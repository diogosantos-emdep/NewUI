using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class BulkPicking : ModelBase, IDisposable
    {
        #region Declaration
        private Int32 idArticle;
        private string reference;
        #endregion

        #region Properties
        [DataMember]
        public Int32 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }
        #endregion

        #region Constructor
        public BulkPicking()
        {

        }


        #endregion

        #region  Methods
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
