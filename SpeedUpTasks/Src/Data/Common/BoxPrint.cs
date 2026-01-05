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
    
    [DataContract]
    public class BoxPrint : ModelBase, IDisposable
    {
        #region Fields
        Int64 idPackingBox;
        string otCode;
        Int32 idCarriageMethod;
        string carriageMethodValue;
        string carriageMethodAbbreviation;
        string siteName;
        string boxNumber;
        double grossWeight;
        Int64 idPartNumber;
        Int64 idOT;
        string siteNameWithCountry;
        string customerName;
        #endregion

        #region Constructor
        public BoxPrint()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public long IdPackingBox
        {
            get
            {
                return idPackingBox;
            }

            set
            {
                idPackingBox = value;
            }
        }

        [DataMember]
        public string OtCode
        {
            get
            {
                return otCode;
            }

            set
            {
                otCode = value;
            }
        }

        [DataMember]
        public int IdCarriageMethod
        {
            get
            {
                return idCarriageMethod;
            }

            set
            {
                idCarriageMethod = value;
            }
        }

        [DataMember]
        public string CarriageMethodValue
        {
            get
            {
                return carriageMethodValue;
            }

            set
            {
                carriageMethodValue = value;
            }
        }

        [DataMember]
        public string CarriageMethodAbbreviation
        {
            get
            {
                return carriageMethodAbbreviation;
            }

            set
            {
                carriageMethodAbbreviation = value;
            }
        }

        [DataMember]
        public string SiteName
        {
            get
            {
                return siteName;
            }

            set
            {
                siteName = value;
            }
        }

        [DataMember]
        public string BoxNumber
        {
            get
            {
                return boxNumber;
            }

            set
            {
                boxNumber = value;
            }
        }

        [DataMember]
        public double GrossWeight
        {
            get
            {
                return grossWeight;
            }

            set
            {
                grossWeight = value;
            }
        }

        [DataMember]
        public long IdPartNumber
        {
            get
            {
                return idPartNumber;
            }

            set
            {
                idPartNumber = value;
            }
        }

        [DataMember]
        public long IdOT
        {
            get
            {
                return idOT;
            }

            set
            {
                idOT = value;
            }
        }


        [DataMember]
        public string SiteNameWithCountry
        {
            get
            {
                return siteNameWithCountry;
            }

            set
            {
                siteNameWithCountry = value;
            }
        }

        [DataMember]
        public string CustomerName
        {
            get
            {
                return customerName;
            }

            set
            {
                customerName = value;
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
