using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.SRM
{
    [Table("enterprises_group")]
    [DataContract]
    public class Group : ModelBase,IDisposable
    {
        #region Fields

        int idEnterpriseGroup;
        string name;
        

        #endregion

        #region Constructor

        public Group()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdEnterpriseGroup")]
        [DataMember]
        public int IdEnterpriseGroup
        {
            get { return idEnterpriseGroup; }
            set
            {
                idEnterpriseGroup = value;
                OnPropertyChanged("IdEnterpriseGroup");
            }
        }

        [Column("Name")]
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

