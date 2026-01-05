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
    public class ModuleFamily : ModelBase, IDisposable
    {
        #region Declaration
        string name;
        Int32 idModuleFamily;       
        List<ModuleSubFamily> moduleSubFamilies;
        #endregion

        #region Properties

        [Key]
        [Column("idModuleFamily")]
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

        [Column("Name")]
        [DataMember]
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
    
        [DataMember]
        public List<ModuleSubFamily> ModuleSubFamilies
        {
            get { return moduleSubFamilies; }
            set
            {
                moduleSubFamilies = value;
                OnPropertyChanged("ModuleSubFamilies");
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
