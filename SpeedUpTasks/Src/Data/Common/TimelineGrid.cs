using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [Serializable]
    [DataContract]
    public class TimelineGrid : INotifyPropertyChanged, ICloneable
    {
        #region Fields

        Int64 idOffer;
        Int64 year;
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
        string offerCloseDate;
        Int32 offerConfidentialLevel;

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
        Int64? idPOType;
        string offerOwner;
        string offeredTo;
        Int32? offeredBy;
        ActiveSite activeSite;
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
        public int Idsite
        {
            get
            {
                return idsite;
            }

            set
            {
                idsite = value;
                OnPropertyChanged("Idsite");
            }
        }

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
        public Int64 Year
        {
            get
            {
                return year;
            }

            set
            {
                year = value;
                OnPropertyChanged("Year");
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
        public Int64? IdPOType
        {
            get
            {
                return idPOType;
            }

            set
            {
                idPOType = value;
                OnPropertyChanged("IdPOType");
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
        public ActiveSite ActiveSite
        {
            get
            {
                return activeSite;
            }

            set
            {
                activeSite = value;
                OnPropertyChanged("ActiveSite");
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
