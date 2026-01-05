using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class WorkStages : ModelBase, IDisposable
    {
        #region Declarations
        private Int32 idStage;
        private string code;
        private string name;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_zh;
        private string name_ru;
        private string description;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_zh;
        private string description_ru;
        private string activeInPlants;
        private UInt32 idStatus;
        private LookupValue status;
        private DateTime lastUpdated;
        private string isProductionStage;
        private string parent;
        private string plantName;
        private string sequence;
        private Int32 createdBy;
        private Int32 activateRework;
        private Int32 modifiedBy;
        private bool isSequenceExists;
        List<LogentriesbyStages> lstStagesChangeLogList;

        #endregion

        #region Properties
        [DataMember]
        public Int32 IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [DataMember]
        public string Name_es
        {
            get { return name_es; }
            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [DataMember]
        public string Name_fr
        {
            get { return name_fr; }
            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [DataMember]
        public string Name_pt
        {
            get { return name_pt; }
            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }

        [DataMember]
        public string Name_ro
        {
            get { return name_ro; }
            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }

        [DataMember]
        public string Name_zh
        {
            get { return name_zh; }
            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [DataMember]
        public string Name_ru
        {
            get { return name_ru; }
            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
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
        public string Description_es
        {
            get { return description_es; }
            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }

        [DataMember]
        public string Description_fr
        {
            get { return description_fr; }
            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }

        [DataMember]
        public string Description_pt
        {
            get { return description_pt; }
            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
            }
        }

        [DataMember]
        public string Description_ro
        {
            get { return description_ro; }
            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }

        [DataMember]
        public string Description_zh
        {
            get { return description_zh; }
            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }

        [DataMember]
        public string Description_ru
        {
            get { return description_ru; }
            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru");
            }
        }
        [DataMember]
        public string ActiveInPlants
        {
            get { return activeInPlants; }
            set
            {
                activeInPlants = value;
                OnPropertyChanged("ActiveInPlants");
            }
        }
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
        [DataMember]
        public UInt32 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }
        [DataMember]
        public DateTime LastUpdated
        {
            get { return lastUpdated; }
            set
            {
                lastUpdated = value;
                OnPropertyChanged("LastUpdated");
            }
        }
        [DataMember]
        public string IsProductionStage
        {
            get { return isProductionStage; }
            set
            {
                isProductionStage = value;
                OnPropertyChanged("IsProductionStage");
            }
        }
        [DataMember]
        public string Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }
        [DataMember]
        public string PlantName
        {
            get { return plantName; }
            set
            {
                plantName = value;
                OnPropertyChanged("PlantName");
            }
        }
        [DataMember]
        public string Sequence
        {
            get { return sequence; }
            set
            {
                sequence = value;
                OnPropertyChanged("Sequence");
            }
        }
        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }
        [DataMember]
        public Int32 ActivateRework
        {
            get { return activateRework; }
            set
            {
                activateRework = value;
                OnPropertyChanged("ActivateRework");
            }
        }

        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        //[GEOS2-3841][Rupali Sarode][11-1-2023]
        [DataMember]
        public bool IsSequenceExists
        {
            get { return isSequenceExists; }
            set
            {
                isSequenceExists = value;
                OnPropertyChanged("IsSequenceExists");
            }
        }

        //[GEOS2-3908][Rupali Sarode][16-1-2023]
        [DataMember]
        public List<LogentriesbyStages> LstStagesChangeLogList
        {
            get
            {
                return lstStagesChangeLogList;
            }

            set
            {
                lstStagesChangeLogList = value;
                OnPropertyChanged("LstStagesChangeLogList");
            }
        }

        #endregion
        #region Constructor

        public WorkStages()
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
            var newWOClone = (WorkStages)this.MemberwiseClone();
            // start GEOS2-3880 add log
            //if (Type != null)
            //    newWOClone.Type = (LookupValue)this.Type.Clone();
            if (Status != null)
                newWOClone.Status = (LookupValue)this.Status.Clone();
            // end GEOS2-3880 add log
            // return this.MemberwiseClone();
            return newWOClone;
        }
        #endregion
    }
}
