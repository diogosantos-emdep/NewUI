using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{    
    [DataContract]
    public class ModuleSubFamily : ModelBase, IDisposable
    {
        #region Declaration

        Int32 idModuleSubfamily;
        string nameSubfamily;
        Int32 idModuleFamily;

        #endregion

        #region Properties


        [DataMember]
        public Int32 IdModuleSubfamily
        {
            get { return idModuleSubfamily; }
            set
            {
                idModuleSubfamily = value;
                OnPropertyChanged("IdModuleSubfamily");
            }
        }
    
        [DataMember]
        public String NameSubfamily
        {
            get { return nameSubfamily; }
            set
            {
                nameSubfamily = value;
                OnPropertyChanged("NameSubfamily");
            }
        }
    
        [DataMember]
        public Int32 IdModuleFamily
        {
            get { return idModuleFamily; }
            set
            {
                idModuleFamily = value;
                OnPropertyChanged("IdModuleFamily");
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
