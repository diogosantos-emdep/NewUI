using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class MicroSiga_Article_taxes : ModelBase
    {
        #region Declaration
        Int32 idArticleTax;
        Int32 idCompany;
        Int32 idArticle;
        Int32 idTaxType;
        double ipivalue;
        #endregion

        [DataMember]
        public Int32 IdArticleTax
        {
            get
            {
                return idArticleTax;
            }

            set
            {
                idArticleTax = value;
                OnPropertyChanged("IdArticleTax");
            }
        }

        [DataMember]
        public Int32 IdCompany
        {
            get
            {
                return idCompany;
            }

            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

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
                OnPropertyChanged("IdArticle");
            }
        }

        [DataMember]
        public Int32 IdTaxType
        {
            get
            {
                return idTaxType;
            }

            set
            {
                idTaxType = value;
                OnPropertyChanged("IdTaxType");
            }
        }
        [DataMember]
        public double Value
        {
            get
            {
                return ipivalue;
            }

            set
            {
                ipivalue = value;
                OnPropertyChanged("Value");
            }
        }
    }
}
