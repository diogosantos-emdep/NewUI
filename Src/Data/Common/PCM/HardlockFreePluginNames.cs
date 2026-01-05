using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    public class HardlockFreePluginNames : ModelBase, IDisposable
    {

        #region Declaration

        string name;
        Int64 idplugin;
        #endregion


        #region Properties

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

        [DataMember]
        public Int64 IdPlugin
        {
            get
            {
                return idplugin;
            }

            set
            {
                idplugin = value;
                OnPropertyChanged("IdPlugin");
            }
        }
        #endregion
        #region Constructor

        public HardlockFreePluginNames()
        {


        }


        #endregion


        #region Implementation of IDisposable
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}

