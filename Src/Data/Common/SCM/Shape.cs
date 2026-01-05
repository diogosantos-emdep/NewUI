using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class Shape : ModelBase, IDisposable
    {
        #region Fields
        private uint id;
        private string name;
        #endregion

        #region Properties
        [DataMember]
        public uint Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }


        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}