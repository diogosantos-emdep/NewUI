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
    [Table("articlesuppliertypes")]
    [DataContract]
    public class ArticleSupplierType : ModelBase, IDisposable
    {
        #region Declaration

        Int32 idArticleSupplierType;
        String name;
        String htmlColor;

        #endregion

        #region Properties

        [Key]
        [Column("IdArticleSupplierType")]
        [DataMember]
        public Int32 IdArticleSupplierType
        {
            get { return idArticleSupplierType; }
            set
            {
                idArticleSupplierType = value;
                OnPropertyChanged("IdArticleSupplierType");
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

        [Column("HtmlColor")]
        [DataMember]
        public String HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        #endregion

        #region Constructor

        public ArticleSupplierType()
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
