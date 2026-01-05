using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class Company : ModelBase, IDisposable
    {

        #region Fields
        private uint id;
        private string name;
        private uint idCustomerType;
        private string web;
        private string logo;
        private string patternForConnectorReferences;
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

        [DataMember]
        public uint IdCustomerType
        {
            get { return idCustomerType; }
            set { idCustomerType = value; OnPropertyChanged("IdCustomerType"); }
        }

        [DataMember]
        public string Web
        {
            get { return web; }
            set { web = value; OnPropertyChanged("Web"); }
        }

        [DataMember]
        public string Logo
        {
            get { return logo; }
            set { logo = value; OnPropertyChanged("Logo"); }
        }

        [DataMember]
        public string PatternForConnectorReferences
        {
            get { return patternForConnectorReferences; }
            set { patternForConnectorReferences = value; OnPropertyChanged("PatternForConnectorReferences"); }
        }   
        #endregion
        public override string ToString()
        {
            return Name;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
