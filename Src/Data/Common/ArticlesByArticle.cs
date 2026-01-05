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
 
    [DataContract]
    public class ArticlesByArticle : ModelBase, IDisposable
    {
        #region Declaration
        Int32 idArticle;
        Int32 idComponent;
        float quantity;
        #endregion

        #region Properties

       [DataMember]
        public Int32 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

    
        [DataMember]
        public Int32 IdComponent
        {
            get { return idComponent; }
            set
            {
                idComponent = value;
                OnPropertyChanged("IdComponent");
            }
        }

        [DataMember]
        public float Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

     

        #endregion

        #region Constructor

        public ArticlesByArticle()
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
