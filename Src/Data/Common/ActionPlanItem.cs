using Emdep.Geos.Data.Common.Crm;
using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
   
    public class ActionPlanItem : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idActionPlan;
        Int64 idActionPlanItem;
        Int32 idScope;
        Int64 number;
        Int32 idGroup;
        Int32 idSite;
        string title;
        
        //string action;
        DateTime openDate;
        DateTime expectedDueDate;
        DateTime currentDueDate;
        Int64 dueDateChangeCount;
        DateTime? closeDate;
       // Int32 idSource;
        Int32 idReporter;
        Int32 idAssignee;
        Int32 idStatus;
        DateTime creationDate;
        Int32 idCreator;
        DateTime? modificationDate;
        Int32? idModifier;
        byte isDeleted;
        List<LogEntriesByActionItem> logEntriesByActionItems;
        string scope;
        string reporter;
        string assignee;
        string group;
        string plant;
        string country;
        LookupValue status;
        ActionPlan actionPlan;
        string subjectOrComment;
        string dueDateWeekly;
        Int32? idSalesActivityType;
        LookupValue salesActivityType;
        List<People> lstResponsible;
        bool isUpdatedRow;
        string creator;
        string modifier;
        byte isInternal;
        UInt64? idTag;
        Tag tag;
        List<Tag> tagList;
        List<LookupValue> statusList;
        List<Tag> actionTags; //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        public string name;
        List<ActionsLinkedItems> actionsLinkedItems;//chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:
        Int32 idOwner; //chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”

        #region chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        string subject;
        string description;
        LookupValue lookupValue;
        People people;
        byte? isCompleted;
        string type;
        #endregion
        #endregion

        #region Properties

        [DataMember]
        public Int64 IdActionPlanItem
        {
            get { return idActionPlanItem; }
            set
            {
                idActionPlanItem = value;
                OnPropertyChanged("IdActionPlanItem");
            }
        }

        [DataMember]
        public Int64 IdActionPlan
        {
            get { return idActionPlan; }
            set
            {
                idActionPlan = value;
                OnPropertyChanged("IdActionPlan");
            }
        }

        

        [DataMember]
        public Int32 IdScope
        {
            get { return idScope; }
            set
            {
                idScope = value;
                OnPropertyChanged("IdScope");
            }
        }

        [DataMember]
        public Int64 Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }


        [DataMember]
        public Int32 IdGroup
        {
            get { return idGroup; }
            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }


        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [DataMember]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }


        //[DataMember]
        //public string Action
        //{
        //    get { return action; }
        //    set
        //    {
        //        action = value;
        //        OnPropertyChanged("Action");
        //    }
        //}


        [DataMember]
        public DateTime OpenDate
        {
            get { return openDate; }
            set
            {
                openDate = value;
                OnPropertyChanged("OpenDate");
            }
        }


        [DataMember]
        public DateTime ExpectedDueDate
        {
            get { return expectedDueDate; }
            set
            {
                expectedDueDate = value;
                OnPropertyChanged("ExpectedDueDate");
            }
        }


        [DataMember]
        public DateTime CurrentDueDate
        {
            get { return currentDueDate; }
            set
            {
                currentDueDate = value;
                OnPropertyChanged("CurrentDueDate");
            }
        }


        [DataMember]
        public Int64 DueDateChangeCount
        {
            get { return dueDateChangeCount; }
            set
            {
                dueDateChangeCount = value;
                OnPropertyChanged("DueDateChangeCount");
            }
        }

        [DataMember]
        public DateTime? CloseDate
        {
            get { return closeDate; }
            set
            {
                closeDate = value;
                OnPropertyChanged("CloseDate");
            }
        }

       
        [DataMember]
        public Int32 IdReporter
        {
            get { return idReporter; }
            set
            {
                idReporter = value;
                OnPropertyChanged("IdReporter");
            }
        }

        [DataMember]
        public Int32 IdAssignee
        {
            get { return idAssignee; }
            set
            {
                idAssignee = value;
                OnPropertyChanged("IdAssignee");
            }
        }

        [DataMember]
        public Int32 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public Int32 IdCreator
        {
            get { return idCreator; }
            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [DataMember]
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public Int32? IdModifier
        {
            get { return idModifier; }
            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }


        [DataMember]
        public byte IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged("isDeleted");
            }
        }

        [DataMember]
        public List<LogEntriesByActionItem> LogEntriesByActionItems
        {
            get { return logEntriesByActionItems; }
            set
            {
                logEntriesByActionItems = value;
                OnPropertyChanged("LogEntriesByActionItems");
            }
        }

        [DataMember]
        public ActionPlan ActionPlan
        {
            get { return actionPlan; }
            set
            {
                actionPlan = value;
                OnPropertyChanged("ActionPlan");
            }
        }

        [DataMember]
        public string Scope
        {
            get { return scope; }
            set
            {
                scope = value;
                OnPropertyChanged("Scope");
            }
        }

        [DataMember]
        public string Reporter
        {
            get { return reporter; }
            set
            {
                reporter = value;
                OnPropertyChanged("Reporter");
            }
        }

        [DataMember]
        public string Assignee
        {
            get { return assignee; }
            set
            {
                assignee = value;
                OnPropertyChanged("Assignee");
            }
        }

        [DataMember]
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        [DataMember]
        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [DataMember]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [DataMember]
        [NotMapped]
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
        [NotMapped]
        public string DueDateWeekly
        {
            get
            {
                return dueDateWeekly;
            }

            set
            {
                dueDateWeekly = value;
                OnPropertyChanged("DueDateWeekly");
            }
        }

        [DataMember]
        [NotMapped]
        public string SubjectOrComment
        {
            get
            {
                return subjectOrComment;
            }

            set
            {
                subjectOrComment = value;
                OnPropertyChanged("SubjectOrComment");
            }
        }

        [DataMember]
        [NotMapped]
        public Int32? IdSalesActivityType
        {
            get
            {
                return idSalesActivityType;
            }

            set
            {
                idSalesActivityType = value;
                OnPropertyChanged("IdSalesActivityType");
            }
        }

        [DataMember]
        [NotMapped]
        public LookupValue SalesActivityType
        {
            get
            {
                return salesActivityType;
            }

            set
            {
                salesActivityType = value;
                OnPropertyChanged("SalesActivityType");
            }
        }

        [DataMember]
        [NotMapped]
        public List<People> LstResponsible
        {
            get
            {
                return lstResponsible;
            }

            set
            {
                lstResponsible = value;
                OnPropertyChanged("LstResponsible");
            }
        }

        [DataMember]
        [NotMapped]
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


        [DataMember]
        public string Creator
        {
            get { return creator; }
            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }

        [DataMember]
        public string Modifier
        {
            get { return modifier; }
            set
            {
                modifier = value;
                OnPropertyChanged("Modifier");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IsInternal
        {
            get { return isInternal; }
            set
            {
                isInternal = value;
                OnPropertyChanged("IsInternal");
            }
        }

        [NotMapped]
        [DataMember]
        public ulong? IdTag
        {
            get
            {
                return idTag;
            }

            set
            {
                idTag = value;
                OnPropertyChanged("IdTag");
            }
        }

        [NotMapped]
        [DataMember]
        public Tag Tag
        {
            get
            {
                return tag;
            }

            set
            {
                tag = value;
                OnPropertyChanged("Tag");
            }
        }

        [NotMapped]
        [DataMember]
        public List<Tag> TagList
        {
            get
            {
                return tagList;
            }

            set
            {
                tagList = value;
                OnPropertyChanged("TagList");
            }
        }

        [NotMapped]
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

        //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        [NotMapped]
        [DataMember]
        public List<Tag> ActionTag
        {
            get { return actionTags; }
            set
            {
                actionTags = value;
                OnPropertyChanged("Tag");
            }
        }

        //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        [Column("Name")]
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

        //chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:
        [NotMapped]
        [DataMember]
        public List<ActionsLinkedItems> ActionsLinkedItems
        {
            get
            {
                return actionsLinkedItems;
            }

            set
            {
                actionsLinkedItems = value;
                OnPropertyChanged("ActionsLinkedItems");
            }
        }

        //chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
        [Column("IdOwner")]
        [DataMember]
        public Int32 IdOwner
        {
            get
            {
                return idOwner;
            }

            set
            {
                idOwner = value;
                OnPropertyChanged("Description");
            }
        }
        #region chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        [Column("Subject")]
        [DataMember]
        public string Subject
        {
            get
            {
                return subject;
            }

            set
            {
                subject = value;
                OnPropertyChanged("Subject");
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
        [DataMember]
        public LookupValue LookupValue
        {
            get
            {
                return lookupValue;
            }

            set
            {
                lookupValue = value;
                OnPropertyChanged("LookupValue");
            }
        }

        [NotMapped]
        [DataMember]
        public People People
        {
            get
            {
                return people;
            }

            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }
        [Column("IsCompleted")]
        [DataMember]
        public byte? IsCompleted
        {
            get
            {
                return isCompleted;
            }

            set
            {
                isCompleted = value;
                OnPropertyChanged("IsCompleted");
            }
        }

        [DataMember]
        [NotMapped]
        public String Type
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
        #endregion
        #endregion

        #region Constructor

        public ActionPlanItem()
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
            ActionPlanItem actionPlanItem = (ActionPlanItem)this.MemberwiseClone();

            if (actionPlanItem.ActionPlan != null)
                actionPlanItem.ActionPlan = (ActionPlan)this.ActionPlan.Clone();

            if (actionPlanItem.Status != null)
                actionPlanItem.Status = (LookupValue)this.Status.Clone();

            if (actionPlanItem.LogEntriesByActionItems != null)
                actionPlanItem.LogEntriesByActionItems = LogEntriesByActionItems.Select(x => (LogEntriesByActionItem)x.Clone()).ToList();

            if (actionPlanItem.Tag != null)
                actionPlanItem.Tag = (Tag)this.Tag.Clone();

            return actionPlanItem;
        }

        #endregion
    }
}
