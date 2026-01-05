using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class Site : ModelBase, IDisposable
    {

        #region Fields
        UInt32 idSite;
        string name;
        #endregion

        #region Constructor
        public Site()
        {

        }
        #endregion

        #region Properties
       
        [DataMember]
        public UInt32 IdSite
        {
            get
            {
                return idSite;
            }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
