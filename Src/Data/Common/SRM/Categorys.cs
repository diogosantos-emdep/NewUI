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
    [Table("articlesuppliertypes")]
    [DataContract]
    public class Categorys : ModelBase,IDisposable
    {
        #region Fields

        int idArticleSupplierType;
        string categoryName;
        

        #endregion

        #region Constructor

        public Categorys()
        {
        }

        #endregion

        #region Properties

        [Key]
       // [Column("IdEnterpriseGroup")]
        [DataMember]
        public int IdArticleSupplierType
        {
            get { return idArticleSupplierType; }
            set
            {
                idArticleSupplierType = value;
                OnPropertyChanged("IdArticleSupplierType");
            }
        }

       // [Column("CategoryName")]
        [DataMember]
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
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

