using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-4502]
    [DataContract]
    public class ValueType : ModelBase, IDisposable
    {
        #region Field
        Int32 idLookupValue;
        string name;
        #endregion

        #region Property
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [DataMember]
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
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
