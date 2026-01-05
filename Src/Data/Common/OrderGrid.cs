using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Serializable]
    [DataContract]
    public class OrderGrid : INotifyPropertyChanged, ICloneable
    {
        #region Fields
        List<SalesOwnerList> salesOwnerList;
        Int64 idOffer;
        byte? idOfferType;
        Int32 connectPlantId;
        string code;
        sbyte isChecked;
        string group;
        Int32 idsite;
        string plant;
        string project;
        string country;
        string region;
        string description;
        string status;
        string htmlColor;
        double amount;
        double invoiceAmount;
        string offerCloseDate;
        Int32 offerConfidentialLevel;
        string iso;
        string oem;
        string source;
        string businessUnit;
        string rfqReceptionDate;
        string quoteSentDate;
        string rfq;
        string category1;
        string category2;
        string lostCompetitor;
        string lostReason;
        string lostDescription;
        List<OptionsByOfferGrid> optionsByOffers;
        Int32? idSalesResponsible;
        Int32? idSalesResponsibleAssemblyBU;
        Int32? idSalesOwner;
        string currency;
        Int64 idProductCategory;
        ProductCategoryGrid productCategory;
        string lastActivityDate;
        Brush htmlBrushColor;
        Int32 idSalesStatusType;
        Int32 offerStatusid;
        string poReceptionDate;
        string shipmentDate;
        string customerPOCodes;
        string alias;
        Int32? idBusinessUnit;
        Int32? idCarOEM;
        Int64? idCarProject;
        Int32? idSource;

        string offerOwner;
        string offeredTo;
        Int32? offeredBy;
        float discount;
        float? discountNullable;
        byte[] countryIconbytes;
        ImageSource countryIconImage;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        [DataMember]
        public int IdSite
        {
            get { return idsite; }
            set { idsite = value; OnPropertyChanged("IdSite"); }
        }

        [DataMember]
        public string Iso
        {
            get { return iso; }
            set { iso = value; OnPropertyChanged("iso"); }
        }


        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        [DataMember]
        public byte? IdOfferType
        {
            get { return idOfferType; }
            set { idOfferType = value; OnPropertyChanged("IdOfferType"); }
        }

        [DataMember]
        public Int32? IdSalesResponsible
        {
            get { return idSalesResponsible; }
            set { idSalesResponsible = value; OnPropertyChanged("IdSalesResponsible"); }
        }

        [DataMember]
        public Int32? IdSalesResponsibleAssemblyBU
        {
            get { return idSalesResponsibleAssemblyBU; }
            set { idSalesResponsibleAssemblyBU = value; OnPropertyChanged("IdSalesResponsibleAssemblyBU"); }
        }

        [DataMember]
        public Int64 IdOffer
        {
            get
            {
                return idOffer;
            }

            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [DataMember]
        public int ConnectPlantId
        {
            get
            {
                return connectPlantId;
            }

            set
            {
                connectPlantId = value;
                OnPropertyChanged("ConnectPlantId");
            }
        }

        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }


        [DataMember]
        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        [DataMember]
        public sbyte IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        [DataMember]
        public float? DiscountNullable
        {
            get
            {
                return discountNullable;
            }

            set
            {
                discountNullable = value;
                OnPropertyChanged("DiscountNullable");
            }
        }
        //[DataMember]
        //public int Idsite
        //{
        //    get
        //    {
        //        return idsite;
        //    }

        //    set
        //    {
        //        idsite = value;
        //        OnPropertyChanged("Idsite");
        //    }
        //}

        [DataMember]
        public string Plant
        {
            get
            {
                return plant;
            }

            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [DataMember]
        public string Project
        {
            get
            {
                return project;
            }

            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }

        [DataMember]
        public string Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [DataMember]
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [DataMember]
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }

            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        [DataMember]
        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        [DataMember]
        public string OfferCloseDate
        {
            get
            {
                return offerCloseDate;
            }

            set
            {
                offerCloseDate = value;
                OnPropertyChanged("OfferCloseDate");
            }
        }

        [DataMember]
        public int OfferConfidentialLevel
        {
            get
            {
                return offerConfidentialLevel;
            }

            set
            {
                offerConfidentialLevel = value;
                OnPropertyChanged("OfferConfidentialLevel");
            }
        }

        [DataMember]
        public string Oem
        {
            get
            {
                return oem;
            }

            set
            {
                oem = value;
                OnPropertyChanged("Oem");
            }
        }

        [DataMember]
        public string Source
        {
            get
            {
                return source;
            }

            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        [DataMember]
        public string BusinessUnit
        {
            get
            {
                return businessUnit;
            }

            set
            {
                businessUnit = value;
                OnPropertyChanged("BusinessUnit");
            }
        }

        [DataMember]
        public string RfqReceptionDate
        {
            get
            {
                return rfqReceptionDate;
            }

            set
            {
                rfqReceptionDate = value;
                OnPropertyChanged("RfqReceptionDate");
            }
        }

        [DataMember]
        public string QuoteSentDate
        {
            get
            {
                return quoteSentDate;
            }

            set
            {
                quoteSentDate = value;
                OnPropertyChanged("QuoteSentDate");
            }
        }

        [DataMember]
        public string Rfq
        {
            get
            {
                return rfq;
            }

            set
            {
                rfq = value;
                OnPropertyChanged("Rfq");
            }
        }

        [DataMember]
        public string Category1
        {
            get
            {
                return category1;
            }

            set
            {
                category1 = value;
                OnPropertyChanged("Category1");
            }
        }

        [DataMember]
        public string Category2
        {
            get
            {
                return category2;
            }

            set
            {
                category2 = value;
                OnPropertyChanged("Category2");
            }
        }

        [DataMember]
        public string LostCompetitor
        {
            get
            {
                return lostCompetitor;
            }

            set
            {
                lostCompetitor = value;
                OnPropertyChanged("LostCompetitor");
            }
        }

        [DataMember]
        public string LostReason
        {
            get
            {
                return lostReason;
            }

            set
            {
                lostReason = value;
                OnPropertyChanged("LostReason");
            }
        }

        [DataMember]
        public string LostDescription
        {
            get
            {
                return lostDescription;
            }

            set
            {
                lostDescription = value;
                OnPropertyChanged("LostDescription");
            }
        }


        [DataMember]
        public List<OptionsByOfferGrid> OptionsByOffers
        {
            get
            {
                return optionsByOffers;
            }

            set
            {
                optionsByOffers = value;
                OnPropertyChanged("OptionsByOffers");
            }
        }


        [DataMember]
        public Int32? IdSalesOwner
        {
            get
            {
                return idSalesOwner;
            }

            set
            {
                idSalesOwner = value;
                OnPropertyChanged("IdSalesOwner");
            }
        }


        [DataMember]
        public string Currency
        {
            get
            {
                return currency;
            }

            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        [DataMember]
        public Int64 IdProductCategory
        {
            get
            {
                return idProductCategory;
            }

            set
            {
                idProductCategory = value;
                OnPropertyChanged("IdProductCategory");
            }
        }


        [DataMember]
        public ProductCategoryGrid ProductCategory
        {
            get
            {
                return productCategory;
            }

            set
            {
                productCategory = value;
                OnPropertyChanged("ProductCategory");
            }
        }


        [DataMember]
        public string LastActivityDate
        {
            get
            {
                return lastActivityDate;
            }

            set
            {
                lastActivityDate = value;
                OnPropertyChanged("LastActivityDate");
            }
        }



        [DataMember]
        public Brush HtmlBrushColor
        {
            get
            {
                return htmlBrushColor;
            }
            set
            {
                htmlBrushColor = value;
                OnPropertyChanged("HtmlBrushColor");
            }
        }

        [DataMember]
        public Int32 IdSalesStatusType
        {
            get
            {
                return idSalesStatusType;
            }
            set
            {
                idSalesStatusType = value;
                OnPropertyChanged("IdSalesStatusType");
            }
        }

        [DataMember]
        public Int32 OfferStatusid
        {
            get
            {
                return offerStatusid;
            }

            set
            {
                offerStatusid = value;
                OnPropertyChanged("OfferStatusid");
            }
        }

        [DataMember]
        public string PoReceptionDate
        {
            get
            {
                return poReceptionDate;
            }

            set
            {
                poReceptionDate = value;
                OnPropertyChanged("PoReceptionDate");
            }
        }

        [DataMember]
        public string ShipmentDate
        {
            get
            {
                return shipmentDate;
            }

            set
            {
                shipmentDate = value;
                OnPropertyChanged("ShipmentDate");
            }
        }


        [DataMember]
        public double InvoiceAmount
        {
            get
            {
                return invoiceAmount;
            }

            set
            {
                invoiceAmount = value;
                OnPropertyChanged("InvoiceAmount");
            }
        }


        [DataMember]
        public string CustomerPOCodes
        {
            get { return customerPOCodes; }
            set
            {
                customerPOCodes = value;
                OnPropertyChanged("CustomerPOCodes");
            }
        }

        [DataMember]
        public string Alias
        {
            get { return alias; }
            set
            {
                alias = value;
                OnPropertyChanged("Alias");
            }
        }

        [DataMember]
        public Int32? IdBusinessUnit
        {
            get { return idBusinessUnit; }
            set
            {
                idBusinessUnit = value;
                OnPropertyChanged("IdBusinessUnit");
            }
        }

        [DataMember]
        public Int32? IdCarOEM
        {
            get { return idCarOEM; }
            set
            {
                idCarOEM = value;
                OnPropertyChanged("IdCarOEM");
            }
        }

        [DataMember]
        public Int64? IdCarProject
        {
            get { return idCarProject; }
            set
            {
                idCarProject = value;
                OnPropertyChanged("IdCarProject");
            }
        }

        [DataMember]
        public Int32? IdSource
        {
            get { return idSource; }
            set
            {
                idSource = value;
                OnPropertyChanged("IdSource");
            }
        }


        [DataMember]
        public string OfferOwner
        {
            get
            {
                return offerOwner;
            }

            set
            {
                offerOwner = value;
                OnPropertyChanged("OfferOwner");
            }
        }

        [DataMember]
        public Int32? OfferedBy
        {
            get
            {
                return offeredBy;
            }

            set
            {
                offeredBy = value;
                OnPropertyChanged("OfferedBy");
            }
        }

        [DataMember]
        public string OfferedTo
        {
            get
            {
                return offeredTo;
            }

            set
            {
                offeredTo = value;
                OnPropertyChanged("OfferedTo");
            }
        }
        [DataMember]
        public float Discount
        {
            get
            {
                return discount;
            }

            set
            {
                discount = value;
                OnPropertyChanged("Discount");
            }
        }


        [DataMember]
        public byte[] CountryIconBytes
        {
            get { return countryIconbytes; }
            set
            {
                countryIconbytes = value;
                OnPropertyChanged("CountryIconBytes");
            }
        }

        [DataMember]
        public ImageSource CountryIconImage
        {
            get { return countryIconImage; }
            set
            {
                countryIconImage = value;
                OnPropertyChanged("CountryIconImage");
            }
        }

        [DataMember]
        public List<SalesOwnerList> SalesOwnerList
        {
            get { return salesOwnerList; }
            set
            {
                salesOwnerList = value;
                OnPropertyChanged("SalesOwnerList");
            }
        }

        #endregion

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}

