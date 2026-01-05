using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("suppliersbysite")]
    [DataContract]
    public class SupplierBySite : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idArticleSupplier;
        Int32 idSite;
    
        #endregion

        #region Properties

        [Key]
        [Column("idArticleSupplier")]
        [DataMember]
        public Int64 IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [Column("idSite")]
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

       

        #endregion

        #region Constructor

        public SupplierBySite()
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
