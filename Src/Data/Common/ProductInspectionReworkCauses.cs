using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class ProductInspectionReworkCauses : ModelBase, IDisposable
    {
        #region Fields
       
        #endregion

        #region Properties
        Int64 idArticleCategory;
        [NotMapped]
        [DataMember]
        public Int64 IdArticleCategory
        {
            get { return idArticleCategory; }
            set
            {
                idArticleCategory = value;
                OnPropertyChanged("IdArticleCategory");
            }
        }

        Int64 idArticle;
        [NotMapped]
        [DataMember]
        public Int64 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        Int64 idReworkCause;
        [NotMapped]
        [DataMember]
        public Int64 IdReworkCause
        {
            get { return idReworkCause; }
            set
            {
                idReworkCause = value;
                OnPropertyChanged("IdReworkCause");
            }
        }

        string reworkCause;
        [NotMapped]
        [DataMember]
        public string ReworkCause
        {
            get { return reworkCause; }
            set
            {
                reworkCause = value;
                OnPropertyChanged("ReworkCause");
            }
        }

        Int64 position;
        [NotMapped]
        [DataMember]
        public Int64 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }
        #endregion

        #region Constructor
        public ProductInspectionReworkCauses()
        {

        }
        #endregion

        #region Methods

        public override string ToString()
        {
            if (ReworkCause != null)
            {
                return string.Format("{0}", ReworkCause);
            }
            else
            {
                return string.Format("{0}", "---");
            }
        }

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
