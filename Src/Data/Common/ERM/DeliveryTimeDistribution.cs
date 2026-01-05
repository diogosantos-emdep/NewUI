using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class DeliveryTimeDistribution : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 iddeliverytimedistribution;
        string code;
        string name;
        string name_es;
        string name_fr;
        string name_ro;
        string name_zh;
        string name_pt;
        string name_ru;
        string description;
        string description_es;
        string description_fr;
        string description_ro;
        string description_zh;
        string description_pt;
        string description_ru;
        DateTime effectiveDate;
        DateTime expiryDate;
        UInt32 idStatus;
        byte isDeleted;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        LookupValue status;
        string plants;
        DateTime lastUpdated;
        private string statusAbbreviation;
        private string statusHtmlColor;
        private string htmlColor;
        List<Site> plantList;
        List<Site> plantList_Cloned;
        List<Site> addPlantList;
        List<Site> deletePlantList;
        List<Site> updatePlantList;
        List<DeliveryTimeDistributionModules> lstDeliveryTimeDistributionModules;
        List<LogentriesbyDeliveryTimeDistribution> lstDeliveryTimeDistributionChangeLogList;
        #endregion

        #region Properties

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
        public List<Site> PlantList_Cloned
        {
            get
            {
                return plantList_Cloned;
            }

            set
            {
                plantList_Cloned = value;
                OnPropertyChanged("PlantList_Cloned");
            }
        }
        [DataMember]
        public List<Site> AddPlantList
        {
            get
            {
                return addPlantList;
            }

            set
            {
                addPlantList = value;
                OnPropertyChanged("AddPlantList");
            }
        }

        [DataMember]
        public List<Site> DeletePlantList
        {
            get
            {
                return deletePlantList;
            }

            set
            {
                deletePlantList = value;
                OnPropertyChanged("DeletePlantList");
            }
        }
        [DataMember]
        public List<Site> UpdatePlantList
        {
            get
            {
                return updatePlantList;
            }

            set
            {
                updatePlantList = value;
                OnPropertyChanged("UpdatePlantList");
            }
        }

        [DataMember]
        public ulong Iddeliverytimedistribution
        {
            get
            {
                return iddeliverytimedistribution;
            }
            set
            {
                iddeliverytimedistribution = value;
                OnPropertyChanged("Iddeliverytimedistribution");
            }

        }
        [Key]
        [Column("Code")]
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
        [Key]
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

        [DataMember]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDate;
            }

            set
            {
                effectiveDate = value;
                OnPropertyChanged("EffectiveDate");
            }
        }

        [DataMember]
        public DateTime ExpiryDate
        {
            get
            {
                return expiryDate;
            }

            set
            {
                expiryDate = value;
                OnPropertyChanged("ExpiryDate");
            }
        }



        [DataMember]
        public UInt32 IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public byte IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
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
        public LookupValue Status
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
        public string Plants
        {
            get
            {
                return plants;
            }

            set
            {
                plants = value;
                OnPropertyChanged("Plants");
            }
        }


        [DataMember]
        public DateTime LastUpdated
        {
            get
            {
                return lastUpdated;
            }

            set
            {
                lastUpdated = value;
                OnPropertyChanged("LastUpdated");
            }
        }
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }
        [DataMember]
        public string StatusHtmlColor
        {
            get
            {
                return statusHtmlColor;
            }

            set
            {
                statusHtmlColor = value;
                OnPropertyChanged("StatusHtmlColor");
            }
        }
     
        [DataMember]

       public List<DeliveryTimeDistributionModules> LstDeliveryTimeDistributionModules
        {
            get { return lstDeliveryTimeDistributionModules; }
            set { lstDeliveryTimeDistributionModules = value;
                OnPropertyChanged("LstDeliveryTimeDistributionModules");
            }
        }

        [DataMember]

        public List<LogentriesbyDeliveryTimeDistribution> LstDeliveryTimeDistributionChangeLogList
        {
            get { return lstDeliveryTimeDistributionChangeLogList; }
            set
            {
                lstDeliveryTimeDistributionChangeLogList = value;
                OnPropertyChanged("LstDeliveryTimeDistributionChangeLogList");
            }
        }

        #endregion

        #region Constructor

        public DeliveryTimeDistribution()
        {
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
