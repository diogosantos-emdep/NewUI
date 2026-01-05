using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class WOItem : ModelBase, IDisposable
    {
        #region Fields
        Int64 idOT;
        string workOrder;
        string workOrderCode;
        Int32 item;
        Int64 idArticle;
        string reference;
        double weight;
        Int64 qty;
        string partNumberCode;
        byte[] articleImageInBytes;
        string imagePath;
        string description;
        double articleWeight;
        Int64 idPartNumber;
        Int64 idOTItem;
        Int64 originalQty;
        Int64 idOffer;
        string articleComment;
        bool showComment;
        DateTime? articleCommentDateOfExpiry;
        string articleWeightWithText;//[Sudhir.jangra][GEOS2-5740]
        bool weightFormatted = false;
        int totalWorkorderCount;//[Sudhir.Jangra][GEOS2-5705]
        int unpackedCount;//[Sudhir.Jangra][GEOS2-5705]
        int packedCount;//[Sudhir.Jangra][GEOS2-5705]
        string totalItemCounted;//[Sudhir.Jangra][GEOS2-5705]
        string totalCountPercentage;//[Sudhir.Jangra][GEOS2-5705]
        string totalCountColor;//[Sudhir.Jangra][GEOS2-5705]
        #endregion

        #region Constructor
        public WOItem()
        {
        }
        #endregion

        #region Properties


        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }


        [DataMember]
        public string WorkOrder
        {
            get { return workOrder; }
            set
            {
                workOrder = value;
                OnPropertyChanged("WorkOrder");
            }
        }

        [DataMember]
        public Int32 Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged("Item");
            }
        }


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

        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }


        [DataMember]
        public double Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }


        [DataMember]
        public Int64 Qty
        {
            get { return qty; }
            set
            {
                qty = value;
                OnPropertyChanged("Qty");
            }
        }

        [DataMember]
        public string PartNumberCode
        {
            get { return partNumberCode; }
            set
            {
                partNumberCode = value;
                OnPropertyChanged("PartNumberCode");
            }
        }

        [DataMember]
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }



        [DataMember]
        public byte[] ArticleImageInBytes
        {
            get { return articleImageInBytes; }
            set
            {
                articleImageInBytes = value;
                OnPropertyChanged("ArticleImageInBytes");
            }
        }


        [DataMember]
        public string WorkOrderCode
        {
            get { return workOrderCode; }
            set
            {
                workOrderCode = value;
                OnPropertyChanged("WorkOrderCode");
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public double ArticleWeight
        {
            get { return articleWeight; }
            set
            {
                articleWeight = value;
                OnPropertyChanged("ArticleWeight");

                var cultureInfo = CultureInfo.CurrentCulture;
                var numberFormat = cultureInfo.NumberFormat;
                weightFormatted = true;
                if (articleWeight < 1)
                {

                    var grams = Math.Round(articleWeight * 1000, 0);
                    if (grams == 1000)
                    {
                        ArticleWeightWithText = string.Format(cultureInfo, "{0} Kg", 1);
                    }
                    else
                    {
                        ArticleWeightWithText = string.Format(cultureInfo, "{0} gr", grams);
                    }
                }
                else
                {
                    ArticleWeightWithText = string.Format(cultureInfo, "{0} Kg", Math.Round(articleWeight, 3));
                }
                weightFormatted = false;
            }
        }



        [DataMember]
        public Int64 IdPartNumber
        {
            get { return idPartNumber; }
            set
            {
                idPartNumber = value;
                OnPropertyChanged("IdPartNumber");
            }
        }

        [DataMember]
        public Int64 IdOTItem
        {
            get { return idOTItem; }
            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOtItem");
            }
        }


        [DataMember]
        public Int64 OriginalQty
        {
            get { return originalQty; }
            set
            {
                originalQty = value;
                OnPropertyChanged("OriginalQty");
            }
        }

        [DataMember]
        public Int64 IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }


        [DataMember]
        public string ArticleComment
        {
            get { return articleComment; }
            set
            {
                articleComment = value;
                OnPropertyChanged("ArticleComment");
            }
        }

        [DataMember]
        public bool ShowComment
        {
            get
            {
                return showComment;
            }

            set
            {
                showComment = value;
                OnPropertyChanged("ShowComment");
            }
        }


        [DataMember]
        public DateTime? ArticleCommentDateOfExpiry
        {
            get
            {
                return articleCommentDateOfExpiry;
            }

            set
            {
                articleCommentDateOfExpiry = value;
                OnPropertyChanged("ArticleCommentDateOfExpiry");
            }
        }

        Int64 idCarriageMethod;
        [DataMember]
        public Int64 IdCarriageMethod
        {
            get { return idCarriageMethod; }
            set
            {
                idCarriageMethod = value;
                OnPropertyChanged("IdCarriageMethod");
            }
        }
        string carriageMethod;
        [DataMember]
        public string CarriageMethod
        {
            get { return carriageMethod; }
            set
            {
                carriageMethod = value;
                OnPropertyChanged("CarriageMethod");
            }
        }
        string carriageMethodHtmlColor;
        [DataMember]
        public string CarriageMethodHtmlColor
        {
            get { return carriageMethodHtmlColor; }
            set
            {
                carriageMethodHtmlColor = value;
                OnPropertyChanged("CarriageMethodHtmlColor");
            }
        }

        [NotMapped]
        [DataMember]
        public string ArticleWeightWithText
        {
            get { return articleWeightWithText; }
            set
            {
                if (weightFormatted)
                {
                    articleWeightWithText = value;
                    OnPropertyChanged("ArticleWeightWithText");
                }
            }
        }

        Int64? idCountryGroup;
        [Column("IdCountryGroup")]
        [DataMember]
        public Int64? IdCountryGroup
        {
            get
            {
                return idCountryGroup;
            }

            set
            {
                idCountryGroup = value;
                OnPropertyChanged("IdCountryGroup");
            }
        }

        CountryGroup countryGroup;
        [DataMember]
        public CountryGroup CountryGroup
        {
            get
            {
                return countryGroup;
            }

            set
            {
                countryGroup = value;
                OnPropertyChanged("CountryGroup");
            }
        }

        Int32 idCompany;
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [NotMapped]
        [DataMember]
        public int TotalWorkOrderCount
        {
            get { return totalWorkorderCount; }
            set
            {
                totalWorkorderCount = value;
                OnPropertyChanged("TotalWorkOrderCount");
            }
        }

        [NotMapped]
        [DataMember]
        public int UnpackedCount
        {
            get { return unpackedCount; }
            set
            {
                unpackedCount = value;
                OnPropertyChanged("UnpackedCount");
            }
        }


        [NotMapped]
        [DataMember]
        public int PackedCount
        {
            get { return packedCount; }
            set
            {
                packedCount = value;
                OnPropertyChanged("PackedCount");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalItemCounted
        {
            get { return totalItemCounted; }
            set
            {
                totalItemCounted = value;
                OnPropertyChanged("TotalItemCounted");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalCountPercentage
        {
            get { return totalCountPercentage; }
            set
            {
                totalCountPercentage = value;
                OnPropertyChanged("TotalCountPercentage");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalCountColor
        {
            get { return totalCountColor; }
            set
            {
                totalCountColor = value;
                OnPropertyChanged("TotalCountColor");
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

        public override string ToString()
        {
            return WorkOrder;
        }
        #endregion
    }
}
