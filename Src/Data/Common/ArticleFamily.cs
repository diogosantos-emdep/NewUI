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
    [Table("articlefamilies")]
    [DataContract]
    public class ArticleFamily : ModelBase, IDisposable
    {
        #region Declaration

        Int32 idArticleFamily;
        string name;
      

        #endregion

        #region Properties

        [Key]
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
