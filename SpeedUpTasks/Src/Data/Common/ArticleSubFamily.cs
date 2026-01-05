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
    [Table("articlesubfamilies")]
    [DataContract]
    public class ArticleSubFamily : ModelBase,IDisposable
    {
        #region Declaration

        Int32 idArticleSubfamily;
        string name;
        Int32 idArticleFamily;
        ArticleFamily articleFamily;

        #endregion

        #region Properties

         [Key]
        [Column("idArticleSubfamily")]
        [DataMember]
        public Int32 IdArticleSubfamily
        {
            get { return idArticleSubfamily; }
            set
            {
                idArticleSubfamily = value;
                OnPropertyChanged("IdArticleSubfamily");
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

        [Column("idArticleFamily")]
        [DataMember]
        public Int32 IdArticleFamily
        {
            get { return idArticleFamily; }
            set
            {
                idArticleFamily = value;
                OnPropertyChanged("IdArticleFamily");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleFamily ArticleFamily
        {
            get { return articleFamily; }
            set
            {
                articleFamily = value;
                OnPropertyChanged("ArticleFamily");
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
