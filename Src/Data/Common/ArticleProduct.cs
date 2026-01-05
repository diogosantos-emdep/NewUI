using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.Xml;

namespace Emdep.Geos.Data.Common
{

    public class ArticleProduct: ModelBase, IDisposable
    {

        private Article article;
        private ushort parent;
        private ushort parentMultiplier;
        private List<WarehouseProductComponent> components;
        private string description;

        byte idProductType;
        ulong idProduct;
        private string reference;

        [NotMapped]
        [DataMember]
        public Article Article
        {
            get
            {
                return article;
            }
            set
            {
                article = value;
                OnPropertyChanged("Article");
            }
        }
        [NotMapped]
        [DataMember]
        public List<WarehouseProductComponent> Components
        {
            get
            {
                return components;
            }
            set
            {
                components = value;
                OnPropertyChanged("Components");
            }
        }

        [NotMapped]
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
        [DataMember]
        public ushort Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }

        [NotMapped]
        [DataMember]
        public ushort ParentMultiplier
        {
            get
            {
                return parentMultiplier;
            }
            set
            {
                parentMultiplier = value;
                OnPropertyChanged("ParentMultiplier");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IdProductType
        {
            get
            {
                return idProductType;
            }

            set
            {
                idProductType = value;
                OnPropertyChanged("IdProductType");
            }
        }

        [NotMapped]
        [DataMember]
        public ulong IdProduct
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
        [NotMapped]
        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

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
