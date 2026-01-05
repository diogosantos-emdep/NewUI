using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows.Media;


namespace Emdep.Geos.Data.Common.Epc
{

    [Table("product_roadmaps")]
    [DataContract(IsReference = true)]
    public class ProductRoadmap : ModelBase,IDisposable
    {
        #region Fields
        Int64 idProductRoadmap;
        Int64 idProduct;
        Int32 idRoadmapSource;
        Int32 idRoadmapPriority;
        Int32 idRoadmapStatus;
        Int32 idProductRoadmapType;
        string linkedTo;
        DateTime? requestDate;
        string sourceFrom;
        string title;
        string description;
        LookupValue roadmapPriority;
        LookupValue roadmapStatus;
        Product product;
        LookupValue roadmapSource;
        ImageSource roadmapImageIcon;
        LookupValue productRoadmapType;
        IList<ProjectTask> projectTasks;
        IList<ProductVersionItem> productVersionItems;
        #endregion

        #region Constructor
        public ProductRoadmap()
        {
            this.ProjectTasks = new List<ProjectTask>();
            this.ProductVersionItems = new List<ProductVersionItem>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdProductRoadmap")]
        [DataMember]
        public Int64 IdProductRoadmap
        {
            get
            {
                return idProductRoadmap;
            }

            set
            {
                idProductRoadmap = value;
                OnPropertyChanged("IdProductRoadmap");
            }
        }


        [Column("IdProduct")]
        [ForeignKey("Product")]
        [DataMember]
        public Int64 IdProduct
        {
            get
            {
                return idProduct;
            }

            set
            {
                idProduct = value;
                OnPropertyChanged("IdProduct");
            }
        }

        [Column("IdRoadmapSource")]
        [ForeignKey("RoadmapSource")]
        [DataMember]
        public Int32 IdRoadmapSource
        {
            get
            {
                return idRoadmapSource;
            }

            set
            {
                idRoadmapSource = value;
                OnPropertyChanged("IdRoadmapSource");
            }
        }


        [Column("IdRoadmapPriority")]
        [ForeignKey("RoadmapPriority")]
        [DataMember]
        public Int32 IdRoadmapPriority
        {
            get
            {
                return idRoadmapPriority;
            }

            set
            {
                idRoadmapPriority = value;
                OnPropertyChanged("IdRoadmapPriority");
            }
        }

        [Column("IdRoadmapStatus")]
        [ForeignKey("RoadmapStatus")]
         [DataMember]
        public Int32 IdRoadmapStatus
        {
            get
            {
                return idRoadmapStatus;
            }

            set
            {
                idRoadmapStatus = value;
                OnPropertyChanged("IdRoadmapStatus");
            }
        }

        [Column("IdProductRoadmapType")]
        [ForeignKey("ProductRoadmapType")]
        [DataMember]
        public Int32 IdProductRoadmapType
        {
            get
            {
                return idProductRoadmapType;
            }

            set
            {
                idProductRoadmapType = value;
                OnPropertyChanged("IdProductRoadmapType");
            }
        }


        [Column("LinkedTo")]
        [DataMember]
        public string LinkedTo
        {
            get
            {
                return linkedTo;
            }

            set
            {
                linkedTo = value;
                OnPropertyChanged("LinkedTo");
            }
        }


        [Column("RequestDate")]
        [DataMember]
        public DateTime? RequestDate
        {
            get
            {
                return requestDate;
            }

            set
            {
                requestDate = value;
                OnPropertyChanged("RequestDate");
            }
        }


        [Column("SourceFrom")]
        [DataMember]
        public string SourceFrom
        {
            get
            {
                return sourceFrom;
            }

            set
            {
                sourceFrom = value;
                OnPropertyChanged("SourceFrom");
            }
        }

        [Column("Title")]
        [DataMember]
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [NotMapped]
        public ImageSource RoadmapImageIcon
        {
            get
            {
                return roadmapImageIcon;
            }

            set
            {
                roadmapImageIcon = value;
            }
        }


        [DataMember]
        public virtual Product Product
        {
            get
            {
                return product;
            }

            set
            {
                product = value;
                OnPropertyChanged("Product");
            }
        }


        [DataMember]
        public virtual LookupValue RoadmapStatus
        {
            get
            {
                return roadmapStatus;
            }

            set
            {
                roadmapStatus = value;
                OnPropertyChanged("RoadmapStatus");
            }
        }

       

        [DataMember]
        public virtual LookupValue RoadmapPriority
        {
            get
            {
                return roadmapPriority;
            }

            set
            {
                roadmapPriority = value;
                OnPropertyChanged("RoadmapPriority");
            }
        }

        [DataMember]
        public virtual LookupValue RoadmapSource
        {
            get
            {
                return roadmapSource;
            }

            set
            {
                roadmapSource = value;
                OnPropertyChanged("RoadmapSource");
            }
        }

        [DataMember]
        public virtual LookupValue ProductRoadmapType
        {
            get
            {
                return productRoadmapType;
            }

            set
            {
                productRoadmapType = value;
                OnPropertyChanged("ProductRoadmapType");
            }
        }

        [DataMember]
        public virtual IList<ProjectTask> ProjectTasks
        {
            get
            {
                return projectTasks;
            }

            set
            {
                projectTasks = value;
                OnPropertyChanged("ProjectTasks");
            }
        }

        [DataMember]
        public virtual IList<ProductVersionItem> ProductVersionItems
        {
            get
            {
                return productVersionItems;
            }

            set
            {
                productVersionItems = value;
                OnPropertyChanged("ProductVersionItems");
            }
        }
        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
