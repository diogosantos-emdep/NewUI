using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class EditProductInspectionArticles : ModelBase, IDisposable
    {
        //[Sudhir.Jangra][GEOS2-3531][17/01/2023]
        #region Fields
        string supplierName;
        string description;
        LookupValue status;
        decimal weight;
        decimal length;
        decimal width;
        decimal height;
        string imagePath;
        byte[] articleImageInBytes;
        ProductInspectionImageInfo productInspectionImage;
        string stageComments;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public string SupplierName
        {
            get { return supplierName; }
            set
            {
                supplierName = value;
                OnPropertyChanged("SupplierName");
            }
        }

        [NotMapped]
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
        [NotMapped]
        [DataMember]
        public LookupValue Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        [NotMapped]
        [DataMember]
        public decimal Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }
        [NotMapped]
        [DataMember]
        public decimal Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }
        [NotMapped]
        [DataMember]
        public decimal Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }
        [NotMapped]
        [DataMember]
        public decimal Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }
        [NotMapped]
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
        [NotMapped]
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
        [NotMapped]
        [DataMember]
        public ProductInspectionImageInfo ProductInspectionImage
        {
            get { return productInspectionImage; }
            set
            {
                productInspectionImage = value;
                OnPropertyChanged("ProductInspectionImage");
            }
        }

        private Int64 idArticleWarehouseInspection;
        [NotMapped]
        [DataMember]
        public Int64 IdArticleWarehouseInspection
        {
            get { return idArticleWarehouseInspection; }
            set
            {
                idArticleWarehouseInspection = value;
                OnPropertyChanged("IdArticleWarehouseInspection");
            }
        }

        private Int32 idArticle;
        [NotMapped]
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
        private List<string> descriptionList;
        [NotMapped]
        [DataMember]
        public List<string> DescriptionList
        {
            get { return descriptionList; }
            set
            {
                descriptionList = value;
                OnPropertyChanged("DescriptionList");
            }
        }

        private Int64 idArticleWarehouseInspectionItem;
        [NotMapped]
        [DataMember]
        public Int64 IdArticleWarehouseInspectionItem
        {
            get { return idArticleWarehouseInspectionItem; }
            set
            {
                idArticleWarehouseInspectionItem = value;
                OnPropertyChanged("IdArticleWarehouseInspectionItem");
            }
        }

        private Int64 idWarehouseDeliveryNoteItem;
        [NotMapped]
        [DataMember]
        public Int64 IdWarehouseDeliveryNoteItem
        {
            get { return idWarehouseDeliveryNoteItem; }
            set
            {
                idWarehouseDeliveryNoteItem = value;
                OnPropertyChanged("IdWarehouseDeliveryNoteItem");
            }
        }

        private Int64 quantityInspected;
        [NotMapped]
        [DataMember]
        public Int64 QuantityInspected
        {
            get { return quantityInspected; }
            set
            {
                quantityInspected = value;
                OnPropertyChanged("QuantityInspected");
            }
        }

        string articleDescription;
        [NotMapped]
        [DataMember]
        public string ArticleDescription
        {
            get { return articleDescription; }
            set
            {
                articleDescription = value;
                OnPropertyChanged("ArticleDescription");
            }
        }


        private Int64 idArticleCategory;
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


        [NotMapped]
        [DataMember]
        List<ProductInspectionReworkCauses> reworkCauses;
        public List<ProductInspectionReworkCauses> ReworkCauses
        {
            get { return reworkCauses; }
            set
            {
                reworkCauses = value;
                OnPropertyChanged("ReworkCauses");
            }
        }

        string comments;
        [NotMapped]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }


     
        #endregion

        #region Constructor
        public EditProductInspectionArticles()
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
