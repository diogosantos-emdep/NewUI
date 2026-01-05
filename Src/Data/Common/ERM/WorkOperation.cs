using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class WorkOperation : ModelBase, IDisposable
    {
        #region Fields
        private UInt32 idWorkOperation;
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
        private string parent;
        private UInt32? idType;
        private UInt32 idStatus;
        private string statusAbbreviation;
        private string statusHtmlColor;
        private LookupValue type;
        private LookupValue status;
        private DateTime lastUpdated;
        private string code;
        private ulong? idParent;
        private string htmlColor;
        private UInt32 idStage;
        private string workStage;
        private UInt32 position;
        private ulong order;
        private string orderName;
        private UInt64 idOrder;
        private Int32 modifiedBy;
        private Int32 createdBy;
        private List<Stages> stages;
        //[001][kshinde][08/06/2022][GEOS2-3711]
        private float distance;
        private float? observedTime;
        private Int32 activity;
        private float normalTime;
        private string detectedProblems;
        private string improvementsProposals;
        List<LookupValue> statusList;
        bool isUpdatedRow;
        string woStatus;
        string statusHTMLColor;
        List<WorkOperation> parentList;
        string remarks; //[GEOS2-3933][Rupali Sarode][19/09/2022]
        List<WorkOperationChangeLog> lstWorkOperationChangeLogList; //GEOS2-3880 changes log
        #region GEOS2-3954 time HH:MM:SS
        private TimeSpan uITempobservedTime;
        private TimeSpan uITempNormalTime;
        bool isObservedTimeHoursExist;
        bool isNormalTimeHoursExist;
        #endregion

        #endregion


        #region Properties

        [DataMember]
        public UInt32 IdWorkOperation
        {
            get { return idWorkOperation; }
            set
            {
                idWorkOperation = value;
                OnPropertyChanged("IdWorkOperation");
            }
        }

        [DataMember]
        public List<LookupValue> StatusList
        {
            get
            {
                return statusList;
            }

            set
            {
                statusList = value;
                OnPropertyChanged("StatusList");
            }
        }

        [DataMember]
        public bool IsUpdatedRow
        {
            get
            {
                return isUpdatedRow;
            }

            set
            {
                isUpdatedRow = value;
                OnPropertyChanged("IsUpdatedRow");
            }
        }

        public List<WorkOperation> ParentList
        {
            get
            {
                return parentList;
            }

            set
            {
                parentList = value;
                OnPropertyChanged("ParentList");
            }
        }

        [DataMember]
        public string WOStatus
        {
            get
            {
                return woStatus;
            }

            set
            {
                woStatus = value;
                OnPropertyChanged("WOStatus");
            }
        }

        [DataMember]
        public string StatusHTMLColor
        {
            get
            {
                return statusHTMLColor;
            }

            set
            {
                statusHTMLColor = value;
                OnPropertyChanged("StatusHTMLColor");
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
        public UInt64? IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
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
        public UInt64 Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }
        [DataMember]
        public UInt64 IdOrder
        {
            get { return idOrder; }
            set
            {
                idOrder = value;
                OnPropertyChanged("IdOrder");
            }
        }
        [DataMember]
        public string OrderName
        {
            get { return orderName; }
            set
            {
                orderName = value;
                OnPropertyChanged("OrderName");
            }
        }

        [DataMember]
        public UInt32? IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
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
        public LookupValue Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                OnPropertyChanged("Type");
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
        public UInt32 IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        [DataMember]
        public string WorkStage
        {
            get { return workStage; }
            set
            {
                workStage = value;
                OnPropertyChanged("WorkStage");
            }
        }

        [DataMember]
        public UInt32 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [DataMember]
        public string KeyName { get; set; }
        int workOperation_count_original;
        [DataMember]
        public int WorkOperation_count_original
        {
            get
            {
                return workOperation_count_original;
            }

            set
            {
                workOperation_count_original = value;
                OnPropertyChanged("WorkOperation_count_original");
            }
        }
        int workOperation_count;
        [DataMember]
        public int WorkOperation_count
        {
            get
            {
                return workOperation_count;
            }

            set
            {
                workOperation_count = value;
                OnPropertyChanged("WorkOperation_count");
            }
        }

        string nameWithCount;
        [DataMember]
        public string NameWithCount
        {
            get
            {
                return nameWithCount;
            }

            set
            {
                nameWithCount = value;
                OnPropertyChanged("NameWithCount");
            }
        }
        [DataMember]
        public List<Stages> Stages
        {
            get
            {
                return stages;
            }

            set
            {
                stages = value;
                OnPropertyChanged("Stages");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        //[001][kshinde][08/06/2022][GEOS2-3711]
        [DataMember]
        public float Distance
        {
            get { return distance; }
            set
            {
                distance = value;
                OnPropertyChanged("Distance");
            }
        }
        [DataMember]
        public float? ObservedTime
        {
            get
            {
                return observedTime;
            }
            set
            {
                observedTime = value;
                OnPropertyChanged("ObservedTime");
            }
        }
        [DataMember]
        public Int32 Activity
        {
            get
            {
                return activity;
            }
            set
            {

                activity = value;
                if (value < 0)
                    activity = 0;
                OnPropertyChanged("Activity");
            }
        }
        [DataMember]
        public float NormalTime
        {
            get
            {
                return normalTime;
            }
            set
            {
                normalTime = value;
                OnPropertyChanged("NormalTime");
            }
        }

        [DataMember]
        public string DetectedProblems
        {
            get { return detectedProblems; }
            set
            {
                detectedProblems = value;
                OnPropertyChanged("DetectedProblems");
            }
        }
        [DataMember]
        public string ImprovementsProposals
        {
            get { return improvementsProposals; }
            set
            {
                improvementsProposals = value;
                OnPropertyChanged("ImprovementsProposals");
            }
        }

        //[GEOS2-3933][Rupali Sarode][19/09/2022]
        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }
        #region GEOS2-3880
        [DataMember]
        public List<WorkOperationChangeLog> LstWorkOperationChangeLogList
        {
            get { return lstWorkOperationChangeLogList; }
            set
            {
                lstWorkOperationChangeLogList = value;
                OnPropertyChanged("LstWorkOperationChangeLogList");
            }
        }
        #endregion

        #region GEOS2-3954 time HH:MM:SS

        [DataMember]
        public TimeSpan UITempobservedTime
        {
            get { return uITempobservedTime; }
            set
            {
                uITempobservedTime = value;
                if (UITempobservedTime.Hours > 0)
                {
                    IsObservedTimeHoursExist = true;
                }
                else
                {
                    IsObservedTimeHoursExist = false;
                }
                OnPropertyChanged("UITempobservedTime");
            }   
        }
        [DataMember]
        public TimeSpan UITempNormalTime
        {
            get { return uITempNormalTime; }
            set
            {
                uITempNormalTime = value;
                if (UITempNormalTime.Hours > 0)
                {
                    IsNormalTimeHoursExist = true;
                }
                else
                {
                    IsNormalTimeHoursExist = false;
                }
                OnPropertyChanged("UITempNormalTime");
            }
        }
       

        [DataMember]
        public bool IsObservedTimeHoursExist
        {
            get { return isObservedTimeHoursExist; }
            set
            {
                isObservedTimeHoursExist = value;
                OnPropertyChanged("IsObservedTimeHoursExist");
            }
        }
        [DataMember]
        public bool IsNormalTimeHoursExist
        {
            get { return isNormalTimeHoursExist; }
            set
            {
                isNormalTimeHoursExist = value;
                OnPropertyChanged("IsNormalTimeHoursExist");
            }
        }
        #endregion
        #endregion


        #region Constructor

        public WorkOperation()
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
            var newWOClone = (WorkOperation)this.MemberwiseClone();
            // start GEOS2-3880 add log
            if (Type != null)
                newWOClone.Type = (LookupValue)this.Type.Clone();
            if (Status != null)
                newWOClone.Status = (LookupValue)this.Status.Clone();
            // end GEOS2-3880 add log
            // return this.MemberwiseClone();
            return newWOClone;
        }

        //public override string ToString()
        //{
        //    return Name;
        //}
        #endregion
    }
}
