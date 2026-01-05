using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-4502][11/07/2023]
    [DataContract]
    public class ValueKey : ModelBase, IDisposable
    {
        #region Fields
        Int32 idLookupKey;
        string lookupKeyName;
        #endregion

        #region Property
        [DataMember]
        public string LookupKeyName
        {
            get { return lookupKeyName; }
            set
            {
                lookupKeyName = value;
                OnPropertyChanged("LookupKeyName");
            }
        }
        [DataMember]
        public Int32 IdLookupKey
        {
            get { return idLookupKey; }
            set
            {
                idLookupKey = value;
                OnPropertyChanged("IdLookupKey");
            }
        }
        #endregion

        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
