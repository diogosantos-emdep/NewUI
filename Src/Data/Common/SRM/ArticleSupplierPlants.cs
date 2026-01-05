using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    public class ArticleSupplierPlants : ModelBase, IDisposable
    {
        #region Declaration
        Int64 idSite;
        bool allow;
        string name;
        #endregion
        #region Properties
        [Key]
        [Column("IdSite")]
        [DataMember]
        public Int64 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Key]
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

        [Key]
        [Column("Allow")]
        [DataMember]
        public bool Allow
        {
            get { return allow; }
            set
            {
                allow = value;
                OnPropertyChanged("Allow");
            }
        }
        #endregion
        #region Constructor

        public ArticleSupplierPlants()
        {
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
