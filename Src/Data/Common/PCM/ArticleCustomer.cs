using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ArticleCustomer : ModelBase, IDisposable
    {

        #region Fields

        string key;
        string groupName;
        string regionName;
        string parent;
        UInt32 idGroup;
        UInt32? idRegion;
        UInt32 idCreator;
        bool isChecked;
        string uniqueId;
        Country country;
        byte? idCountry;
        byte[] countryIconbytes;
        Site plant;
        uint? idPlant;
       // UInt64 idArticleCustomerReferences;
        string iso;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;

        UInt64 idArticleList;
        Int32 isIncluded;
        string referenceCustomer;
        Int32 idArticleCustomerReferences;
        
            List<ArticleSuppliers> articleSuppliersList;
        TransactionOperations transactionOperation;
        #endregion

        #region Constructor

        public ArticleCustomer()
        {

        }

        #endregion


        #region Properties

        [DataMember]
        public Int32 IdArticleCustomerReferences
        {
            get
            {
                return idArticleCustomerReferences;
            }

            set
            {
                idArticleCustomerReferences = value;
                OnPropertyChanged("IdArticleCustomerReferences");
            }
        }

        [DataMember]
        public string Parent
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

        [DataMember]
        public uint IdGroup
        {
            get
            {
                return idGroup;
            }

            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }

        [DataMember]
        public uint? IdRegion
        {
            get
            {
                return idRegion;
            }

            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        [DataMember]
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

        [DataMember]
        public string GroupName
        {
            get
            {
                return groupName;
            }

            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        [DataMember]
        public bool IsChecked
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
        public string RegionName
        {
            get
            {
                return regionName;
            }

            set
            {
                regionName = value;
                OnPropertyChanged("RegionName");
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
        public string UniqueId
        {
            get
            {
                return uniqueId;
            }

            set
            {
                uniqueId = value;
                OnPropertyChanged("UniqueId");
            }
        }

        [DataMember]
        public Country Country
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
        public byte? IdCountry
        {
            get
            {
                return idCountry;
            }

            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [DataMember]
        public Site Plant
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
        public uint? IdPlant
        {
            get
            {
                return idPlant;
            }

            set
            {
                idPlant = value;
                OnPropertyChanged("IdPlant");
            }
        }

        [DataMember]
        public string ReferenceCustomer
        {
            get
            {
                return referenceCustomer;
            }

            set
            {
                referenceCustomer = value;
                OnPropertyChanged("Reference");
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
        public ulong IdArticleList
        {
            get
            {
                return idArticleList;
            }

            set
            {
                idArticleList = value;
                OnPropertyChanged("IdArticleList");
            }
        }

        [DataMember]
        public Int32 IsIncluded
        {
            get
            {
                return isIncluded;
            }

            set
            {
                isIncluded = value;
                OnPropertyChanged("IsIncluded");
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
        [NotMapped]
        [DataMember]
        public string Iso
        {
            get { return iso; }
            set
            {
                iso = value;
                OnPropertyChanged("Iso");
            }
        }

        [DataMember]
        public List<ArticleSuppliers> ArticleSuppliersList
        {
            get { return articleSuppliersList; }
            set
            {
                articleSuppliersList = value;
                OnPropertyChanged("ArticleSuppliersList");
            }

        }
        [DataMember]
        public TransactionOperations TransactionOperation
        {
            get
            {
                return this.transactionOperation;
            }
            set
            {
                this.transactionOperation = value;
                this.OnPropertyChanged("TransactionOperation");
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
            ArticleCustomer articleCustomer = (ArticleCustomer)this.MemberwiseClone();
            if (Country != null)
                articleCustomer.Country = (Country)this.Country.Clone();

            if (plant != null)
                articleCustomer.plant = (Site)this.plant.Clone();

            return articleCustomer;
        }

        #endregion
    }
}
