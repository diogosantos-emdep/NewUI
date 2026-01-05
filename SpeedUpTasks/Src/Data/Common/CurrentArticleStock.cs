using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class CurrentArticleStock
    {
        #region Fields
        Int32 idArticle;
        Int64 articleStock;
        #endregion

        #region Constructor
        public CurrentArticleStock()
        {
        }

        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int32 IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
           
            }
        }
             
        [NotMapped]
        [DataMember]
        public Int64 ArticleStock
        {
            get
            {
                return articleStock;
            }

            set
            {
                articleStock = value;
                
            }
        }

        #endregion

    }
}
