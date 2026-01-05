using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ProductTypeCompatibility : ModelBase, IDisposable, IDataErrorInfo
    {
        #region Fields

        uint idCompatibility;
        byte idCPType;
        byte? idCPtypeCompatibility;
        UInt32? idArticleCompatibility;
        UInt32 idTypeCompatibility;
        Int32? minimumElements;
        Int32? maximumElements;
        Int32 idRelationshipType;
        int? quantity;
        string remarks;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;

        LookupValue relationshipType;
        ProductTypes productType;
        Articles article;

        string code;
        string name;
        #endregion

        #region Constructor

        public ProductTypeCompatibility()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public uint IdCompatibility
        {
            get
            {
                return idCompatibility;
            }

            set
            {
                idCompatibility = value;
                OnPropertyChanged("IdCompatibility");
            }
        }

        [DataMember]
        public byte IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }

        [DataMember]
        public byte? IdCPtypeCompatibility
        {
            get
            {
                return idCPtypeCompatibility;
            }

            set
            {
                idCPtypeCompatibility = value;
                OnPropertyChanged("IdCPtypeCompatibility");
            }
        }

        [DataMember]
        public UInt32? IdArticleCompatibility
        {
            get
            {
                return idArticleCompatibility;
            }

            set
            {
                idArticleCompatibility = value;
                OnPropertyChanged("IdArticleCompatibility");
            }
        }

        [DataMember]
        public uint IdTypeCompatibility
        {
            get
            {
                return idTypeCompatibility;
            }

            set
            {
                idTypeCompatibility = value;
                OnPropertyChanged("IdTypeCompatibility");
            }
        }

        [DataMember]
        public int? MinimumElements
        {
            get
            {
                return minimumElements;
            }

            set
            {
                minimumElements = value;
                OnPropertyChanged("MinimumElements");
            }
        }

        [DataMember]
        public int? MaximumElements
        {
            get
            {
                return maximumElements;
            }

            set
            {
                maximumElements = value;
                OnPropertyChanged("MaximumElements");
            }
        }

        [DataMember]
        public int IdRelationshipType
        {
            get
            {
                return idRelationshipType;
            }

            set
            {
                idRelationshipType = value;
                OnPropertyChanged("IdRelationshipType");
            }
        }

        [DataMember]
        public int? Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [DataMember]
        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                remarks = value;
                char[] trimChars = {' '};
                remarks = remarks == null ? "" : remarks;
                if (remarks.Contains(" "))
                {
                    remarks = remarks.TrimEnd(trimChars);
                    remarks = remarks.TrimStart(trimChars);
                }
                OnPropertyChanged("Remarks");
            }
        }

        [DataMember]
        public uint IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

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
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public uint? IdModifier
        {
            get
            {
                return idModifier;
            }

            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }

            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public LookupValue RelationshipType
        {
            get
            {
                return relationshipType;
            }

            set
            {
                relationshipType = value;
                OnPropertyChanged("RelationshipType");
            }
        }

        [DataMember]
        public ProductTypes ProductType
        {
            get
            {
                return productType;
            }

            set
            {
                productType = value;
                OnPropertyChanged("ProductType");
            }
        }

        [DataMember]
        public Articles Article
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
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
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
            ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)this.MemberwiseClone();

            if (RelationshipType != null)
                productTypeCompatibility.RelationshipType = (LookupValue)this.RelationshipType.Clone();

            if (ProductType != null)
                productTypeCompatibility.ProductType = (ProductTypes)this.ProductType.Clone();

            if (Article != null)
                productTypeCompatibility.Article = (Articles)this.Article.Clone();

            return productTypeCompatibility;
        }
        #endregion

        #region Validation

        public string Error
        {
            get { return GetError(); }
        }

        public string this[string columnName]
        {
            get { return GetError(columnName); }
        }

        string GetError(string name = null)
        {
            switch (name)
            {
                case "MinimumElements":
                    return MinimumElements > MaximumElements ? "Min value must be equal or less than max value." : null;

                case "MaximumElements":
                    return MaximumElements < MinimumElements ? "Max value must be equal or greater than min value." : null;

                case "IdRelationshipType":
                    return (IdRelationshipType != 251 && (Quantity == 0 || Quantity == null)) ? "For this relationship type quantity should be greater than zero." : null;

                case "Quantity":
                    return (IdRelationshipType != 251 && (Quantity == 0 || Quantity == null)) ? "For this relationship type quantity should be greater than zero." : null;

                default:
                    return null;
            }
        }

        #endregion
    }
}
