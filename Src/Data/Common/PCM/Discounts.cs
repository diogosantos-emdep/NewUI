using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.PCM
{
    public class Discounts : ModelBase, IDisposable
    {
        #region Fields
        int _IdScope;
        int _IdPlatform;
        uint idCreator;
        uint idModifier;
        private Int32 _Id = 0;
        List<Site> plantList;
        string name;
        string name_es;
        string name_fr;
        string name_ro;
        string name_zh;
        string name_pt;
        string name_ru;
        string _Description;
        string description_es;
        string description_fr;
        string description_ro;
        string description_zh;
        string description_pt;
        string description_ru;
        private string _Scope = string.Empty;
        private string _Platform = string.Empty;
        private decimal _Value;
        private bool _IsReadOnly = false;
        private string _StartDate;
        private string _EndDate;
        private string _InUse = string.Empty;
        private string _Plant = string.Empty;
        private string _Plants = string.Empty;
        private DateTime _CreationDate;
        private string _Creator = string.Empty;
        private string _LastUpdate;
        private string _Modifier = string.Empty;
        private string _Group = string.Empty;
        private string _Region = string.Empty;
        private string _Country = string.Empty;
        private bool overdueEndDate;
        List<DiscountArticles> discountArticles;
        private DateTime _StartDateNew;
        private DateTime _EndDateNew;
        private DateTime _LastUpdateNew;
        List<Site> deletedPlantList;
        List<Site> addedPlantList;
        List<DiscountArticles> deletedDiscountArticles;
        List<DiscountCustomers> newDiscountCustomer;
        List<DiscountCustomers> updateDiscountCustomer;
        List<DiscountLogEntry> discountLogEntryList;
        List<DiscountLogEntry> getdiscountLogEntryList;
        DiscountLogEntry discountLogEntry;
        DiscountArticles discountsArticles;

        List<DiscountLogEntry> discountCommentsList;//[Sudhir.Jangra][GEOS2-4935]
        #endregion

        #region Constructor
        public Discounts()
        {

        }
        #endregion

        #region Properties
        [Key]
        [DataMember]
        public Int32 Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged("Id");
            }
        }

        [Column("Name")]
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

        [DataMember]
        public string Name_es
        {
            get
            {
                return name_es;
            }

            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [DataMember]
        public string Name_fr
        {
            get
            {
                return name_fr;
            }

            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [DataMember]
        public string Name_ro
        {
            get
            {
                return name_ro;
            }

            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }

        [DataMember]
        public string Name_zh
        {
            get
            {
                return name_zh;
            }

            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [DataMember]
        public string Name_pt
        {
            get
            {
                return name_pt;
            }

            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }

        [DataMember]
        public string Name_ru
        {
            get
            {
                return name_ru;
            }

            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
            }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public string Description_es
        {
            get
            {
                return description_es;
            }

            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }

        [DataMember]
        public string Description_fr
        {
            get
            {
                return description_fr;
            }

            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }

        [DataMember]
        public string Description_ro
        {
            get
            {
                return description_ro;
            }

            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }

        [DataMember]
        public string Description_zh
        {
            get
            {
                return description_zh;
            }

            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }

        [DataMember]
        public string Description_pt
        {
            get
            {
                return description_pt;
            }

            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
            }
        }

        [DataMember]
        public string Description_ru
        {
            get
            {
                return description_ru;
            }

            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru");
            }
        }

        [Column("Scope")]
        [DataMember]
        public string Scope
        {
            get { return _Scope; }
            set
            {
                _Scope = value;
                OnPropertyChanged("Scope");
            }
        }

        [Column("Platform")]
        [DataMember]
        public string Platform
        {
            get
            {
                return _Platform;
            }

            set
            {
                _Platform = value;
                OnPropertyChanged("Platform");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public string StartDate
        {
            get
            {
                return _StartDate;
            }

            set
            {
                _StartDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Column("EndDate")]
        [DataMember]
        public string EndDate
        {
            get
            {
                return _EndDate;
            }

            set
            {
                _EndDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public DateTime StartDateNew
        {
            get
            {
                return _StartDateNew;
            }

            set
            {
                _StartDateNew = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Column("EndDate")]
        [DataMember]
        public DateTime EndDateNew
        {
            get
            {
                return _EndDateNew;
            }

            set
            {
                _EndDateNew = value;
                OnPropertyChanged("EndDate");
            }
        }

        [Column("Value")]
        [DataMember]
        public decimal Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = value;
                OnPropertyChanged("Value");
            }
        }

        [Column("Plant")]
        [DataMember]
        public string Plant
        {
            get
            {
                return _Plant;
            }

            set
            {
                _Plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [Column("Group")]
        [DataMember]
        public string Group
        {
            get
            {
                return _Group;
            }

            set
            {
                _Group = value;
                OnPropertyChanged("Group");
            }
        }

        [Column("Region")]
        [DataMember]
        public string Region
        {
            get
            {
                return _Region;
            }

            set
            {
                _Region = value;
                OnPropertyChanged("Region");
            }
        }
        [Column("Country")]
        [DataMember]
        public string Country
        {
            get
            {
                return _Country;
            }

            set
            {
                _Country = value;
                OnPropertyChanged("Country");
            }
        }

        [Column("Plants")]
        [DataMember]
        public string Plants
        {
            get
            {
                return _Plants;
            }

            set
            {
                _Plants = value;
                OnPropertyChanged("Plants");
            }
        }

        [Column("LastUpdate")]
        [DataMember]
        public string LastUpdate
        {
            get
            {
                return _LastUpdate;
            }

            set
            {
                _LastUpdate = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        [Column("LastUpdate")]
        [DataMember]
        public DateTime LastUpdateNew
        {
            get
            {
                return _LastUpdateNew;
            }

            set
            {
                _LastUpdateNew = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        [Column("InUse")]
        [DataMember]
        public string InUse
        {
            get
            {
                return _InUse;
            }

            set
            {
                _InUse = value;
                OnPropertyChanged("InUse");
            }
        }

        [DataMember]
        public string Creator
        {
            get
            {
                return _Creator;
            }

            set
            {
                _Creator = value;
                OnPropertyChanged("Creator");
            }
        }
        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return _CreationDate;
            }

            set
            {
                _CreationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public bool IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }

            set
            {
                _IsReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }
        [DataMember]
        public bool OverdueEndDate
        {
            get
            {
                return overdueEndDate;
            }

            set
            {
                overdueEndDate = value;
                OnPropertyChanged("OverdueEndDate");
            }
        }


        [DataMember]
        public List<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged("PlantList");
            }
        }

        [DataMember]
        public List<Site> AddedPlantList
        {
            get
            {
                return addedPlantList;
            }

            set
            {
                addedPlantList = value;
                OnPropertyChanged("AddedPlantList");
            }
        }

        [DataMember]
        public List<Site> DeletedPlantList
        {
            get
            {
                return deletedPlantList;
            }

            set
            {
                deletedPlantList = value;
                OnPropertyChanged("DeletedPlantList");
            }
        }

        public List<DiscountArticles> DeletedDiscountArticles
        {
            get { return deletedDiscountArticles; }
            set
            {
                deletedDiscountArticles = value;
                OnPropertyChanged("DeletedDiscountArticles");
            }

        }
        public DiscountArticles DiscountsArticles
        {
            get { return discountsArticles; }
            set
            {
                discountsArticles = value;
                OnPropertyChanged("DiscountsArticles");
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
        public uint IdModifier
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
        public List<DiscountArticles> DiscountArticles
        {
            get { return discountArticles; }
            set
            {
                discountArticles = value;
                OnPropertyChanged("DiscountArticles");
            }

        }

        [DataMember]
        public int IdScope
        {
            get { return _IdScope; }
            set
            {
                _IdScope = value;
                OnPropertyChanged("IdScope");
            }
        }

        [DataMember]
        public int IdPlatform
        {
            get
            {
                return _IdPlatform;
            }

            set
            {
                _IdPlatform = value;
                OnPropertyChanged("IdPlatform");
            }
        }

        [DataMember]
        public List<DiscountCustomers> NewDiscountCustomer
        {
            get
            {
                return newDiscountCustomer;
            }

            set
            {
                newDiscountCustomer = value;
                OnPropertyChanged("NewDiscountCustomer");
            }
        }
        [DataMember]
        public List<DiscountCustomers> UpdateDiscountCustomer
        {
            get
            {
                return updateDiscountCustomer;
            }

            set
            {
                updateDiscountCustomer = value;
                OnPropertyChanged("DiffDiscountCustomer");
            }
        }
        [DataMember]
        public List<DiscountLogEntry> DiscountLogEntryList
        {
            get
            {
                return discountLogEntryList;
            }

            set
            {
                discountLogEntryList = value;
                OnPropertyChanged("DiscountLogEntryList");
            }
        }

        public List<DiscountLogEntry> GetDiscountLogEntryList
        {
            get
            {
                return getdiscountLogEntryList;
            }

            set
            {
                getdiscountLogEntryList = value;
                OnPropertyChanged("GetDiscountLogEntryList");
            }
        }
        public DiscountLogEntry DiscountLogEntry
        {
            get
            {
                return discountLogEntry;
            }

            set
            {
                discountLogEntry = value;
                OnPropertyChanged("DiscountLogEntry");
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        [DataMember]
        public List<DiscountLogEntry> DiscountCommentsList
        {
            get { return discountCommentsList; }
            set
            {
                discountCommentsList = value;
                OnPropertyChanged("DiscountCommentsList");
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
            var newDiscountClone = (Discounts)this.MemberwiseClone();

            if (DiscountArticles != null)
                newDiscountClone.DiscountArticles = DiscountArticles.Select(x => (DiscountArticles)x.Clone()).ToList();
            if(DiscountCommentsList!=null)
                newDiscountClone.DiscountCommentsList = DiscountCommentsList.Select(x => (DiscountLogEntry)x.Clone()).ToList();

            return newDiscountClone;
        }

        #endregion
    }
}
