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
    public class ActionPlan : ModelBase, IDisposable
    {
        #region Declaration
        Int64 idActionPlan;
        string code;
        string name;
       // Int32 idAccountable;
        DateTime lastMeetingDate;
        DateTime nextMeetingDate;
        DateTime creationDate;
        Int32 idCreator;
        DateTime modificationDate;
        Int32 idModifier;
        byte isDeleted;
        List<ActionPlanItem> actionPlanItems;
        #endregion

        #region Properties

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


        //[DataMember]
        //public Int32 IdAccountable
        //{
        //    get { return idAccountable; }
        //    set
        //    {
        //        idAccountable = value;
        //        OnPropertyChanged("IdAccountable");
        //    }
        //}


        [DataMember]
        public DateTime LastMeetingDate
        {
            get { return lastMeetingDate; }
            set
            {
                lastMeetingDate = value;
                OnPropertyChanged("LastMeetingDate");
            }
        }

        [DataMember]
        public DateTime NextMeetingDate
        {
            get { return nextMeetingDate; }
            set
            {
                nextMeetingDate = value;
                OnPropertyChanged("NextMeetingDate");
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
        public DateTime ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }


        [DataMember]
        public Int32 IdModifier
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
                OnPropertyChanged("IsDeleted");
            }
        }

        [DataMember]
        public List<ActionPlanItem> ActionPlanItems
        {
            get { return actionPlanItems; }
            set
            {
                actionPlanItems = value;
                OnPropertyChanged("ActionPlanItems");
            }
        }

        #endregion

        #region Constructor

        public ActionPlan()
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
            return this.MemberwiseClone();
        }

        #endregion
    }
}
