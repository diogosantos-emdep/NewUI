using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.PCM;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common.ERM
{
    /// <summary>
    /// StandardOperationsDictionary class is created by renaming StandardTime class
    /// </summary>
    [DataContract]
    public class StandardOperationsDictionary : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idStandardOperationsDictionary;
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
        List<StandardOperationsDictionarySupplement> lstStandardOperationsDictionarySupplement;
        List<StandardOperationsDictionarySupplement> lstStandardOperationsDictionarySupplement_Cloned;
        List<StandardOperationsDictionaryModules> lstStandardOperationsDictionaryModules;
        List<StandardOperationsDictionaryDetection> lstStandardOperationsDictionaryDetection;
        List<StandardOperationsDictionaryOption> lstStandardOperationsDictionaryOption;
        List<LogentriesbyStandardOperationsDictionary> lstStandardOperationsDictionaryChangeLogList;
        List<StandardOperationsDictionaryWays> lstStandardOperationsDictionaryWay;
        List<StandardOperationsDictionaryStructures> lstStandardOperationsDictionaryStructure;
        //GEOS2-4033 Gulab Lakade Standard Operations Dictionary (SOD) in Spare Parts per workstation
        List<StandardOperationsDictionarySparePart> lstStandardOperationsDictionarySparePart;
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
        public ulong IdStandardOperationsDictionary {
            get
            {
                return idStandardOperationsDictionary;
            }
            set
            {
                idStandardOperationsDictionary = value;
                OnPropertyChanged("IdStandardOperationsDictionary");
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
        public string StatusAbbreviation
        {
            get
            {
                return statusAbbreviation;
            }

            set
            {
                statusAbbreviation = value;
                OnPropertyChanged("StatusAbbreviation");
            }
        }

        [DataMember]
        public List<StandardOperationsDictionarySupplement> LstStandardOperationsDictionarySupplement
        {
            get
            {
                return lstStandardOperationsDictionarySupplement;
            }

            set
            {
                lstStandardOperationsDictionarySupplement = value;
                OnPropertyChanged("LstStandardOperationsDictionarySupplement");
            }
        }

        [DataMember]
        public List<StandardOperationsDictionarySupplement> LstStandardOperationsDictionarySupplement_Cloned
        {
            get
            {
                return lstStandardOperationsDictionarySupplement_Cloned;
            }

            set
            {
                lstStandardOperationsDictionarySupplement_Cloned = value;
                OnPropertyChanged("LstStandardOperationsDictionarySupplement_Cloned");
            }
        }

        [DataMember]
        public List<StandardOperationsDictionaryModules> LstStandardOperationsDictionaryModules
        {
            get
            {
                return lstStandardOperationsDictionaryModules;
            }

            set
            {
                lstStandardOperationsDictionaryModules = value;
                OnPropertyChanged("LstStandardOperationsDictionaryModules");
            }
        }

        [DataMember]
        public List<StandardOperationsDictionaryDetection> LstStandardOperationsDictionaryDetection
        {
            get
            {
                return lstStandardOperationsDictionaryDetection;
            }

            set
            {
                lstStandardOperationsDictionaryDetection = value;
                OnPropertyChanged("LstStandardOperationsDictionaryDetection");
            }
        }

        [DataMember]
        public List<StandardOperationsDictionaryOption> LstStandardOperationsDictionaryOption
        {
            get
            {
                return lstStandardOperationsDictionaryOption;
            }

            set
            {
                lstStandardOperationsDictionaryOption = value;
                OnPropertyChanged("LstStandardOperationsDictionaryOption");
            }
        }

        [DataMember]
        public List<LogentriesbyStandardOperationsDictionary> LstStandardOperationsDictionaryChangeLogList
        {
            get
            {
                return lstStandardOperationsDictionaryChangeLogList;
            }

            set
            {
                lstStandardOperationsDictionaryChangeLogList = value;
                OnPropertyChanged("LstStandardOperationsDictionaryChangeLogList");
            }
        }

        [DataMember]
        public List<StandardOperationsDictionaryWays> LstStandardOperationsDictionaryWay
        {
            get
            {
                return lstStandardOperationsDictionaryWay;
            }

            set
            {
                lstStandardOperationsDictionaryWay = value;
                OnPropertyChanged("LstStandardOperationsDictionaryWay");
            }
        }

        [DataMember]
        public List<StandardOperationsDictionaryStructures> LstStandardOperationsDictionaryStructure
        {
            get
            {
                return lstStandardOperationsDictionaryStructure;
            }

            set
            {
                lstStandardOperationsDictionaryStructure = value;
                OnPropertyChanged("LstStandardOperationsDictionaryStructure");
            }
        }
        //GEOS2-4033 - Gulab lakade Standard Operations Dictionary (SOD) in Spare Parts per workstation
        [DataMember]
        public List<StandardOperationsDictionarySparePart> LstStandardOperationsDictionarySparePart
        {
            get
            {
                return lstStandardOperationsDictionarySparePart;
            }

            set
            {
                lstStandardOperationsDictionarySparePart = value;
                OnPropertyChanged("LstStandardOperationsDictionarySparePart");
            }
        }
        #endregion

        #region Constructor

        public StandardOperationsDictionary()
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
            var newSODClone = (StandardOperationsDictionary)this.MemberwiseClone();
            
            if (Status != null)
                newSODClone.Status = (LookupValue)this.Status.Clone();
            
            if (PlantList != null)
                newSODClone.PlantList = PlantList.Select(x => (Site)x.Clone()).ToList();
            
            if (PlantList_Cloned != null)
                newSODClone.PlantList_Cloned = PlantList_Cloned.Select(x => (Site)x.Clone()).ToList();
            //if (LstStandardOperationsDictionarySupplement_Cloned != null)
            //    newSODClone.LstStandardOperationsDictionarySupplement_Cloned = LstStandardOperationsDictionarySupplement_Cloned.Select(x => (StandardOperationsDictionarySupplement)x.Clone()).ToList();

            if (LstStandardOperationsDictionarySupplement != null)
                newSODClone.LstStandardOperationsDictionarySupplement = LstStandardOperationsDictionarySupplement.Select(x => (StandardOperationsDictionarySupplement)x.Clone()).ToList();

            return newSODClone;
        }

        #endregion
    }
}
