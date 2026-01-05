using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{


    public class PODetail
    {
        #region Fields
        DateTime creationDate;
        sbyte isEmdep;
        double unitPrice;
        UInt32 idCurrency;
        UInt32 idArticle;
        double discount;
        #endregion


        #region Properties
        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
            }
        }

        [DataMember]
        public sbyte IsEmdep
        {
            get
            {
                return isEmdep;
            }

            set
            {
                isEmdep = value;
            }
        }

        [DataMember]
        public double UnitPrice
        {
            get
            {
                return unitPrice;
            }

            set
            {
                unitPrice = value;
            }
        }


        [DataMember]
        public UInt32 IdCurrency
        {
            get
            {
                return idCurrency;
            }

            set
            {
                idCurrency = value;
            }
        }

        [DataMember]
        public UInt32 IdArticle
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
        [DataMember]
        public double Discount
        {
            get { return discount; }
            set
            {
                discount = value;              
            }
        }
        #endregion


    }
}
