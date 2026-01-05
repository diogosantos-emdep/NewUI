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
    public class LogEntriesByActionItem : ModelBase, IDisposable
    {
        #region Declaration
        Int64 idLogEntryByActionItem;
        Int64 idActionPlanItem;
        string comment;
        DateTime creationDate;
        Int32 idCreator;
        People peopleCreator;
        DateTime modificationDate;
        Int32 idModifier;
        Int32 idLogEntryType;
        string creator;
        string modifier;
        bool isRtfText;
        bool isEnabled = true;
        #endregion

        #region Properties

        [DataMember]
        public Int64 IdLogEntryByActionItem
        {
            get { return idLogEntryByActionItem; }
            set
            {
                idLogEntryByActionItem = value;
                OnPropertyChanged("IdLogEntryByActionItem");
            }
        }

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
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
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
        public Int32 IdLogEntryType
        {
            get { return idLogEntryType; }
            set
            {
                idLogEntryType = value;
                OnPropertyChanged("IdLogEntryType");
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

        [DataMember]
        public bool IsRtfText
        {
            get { return isRtfText; }
            set
            {
                isRtfText = value;
                OnPropertyChanged("IsRtfText");
            }
        }

        [NotMapped]
        [DataMember]
        public People PeopleCreator
        {
            get { return peopleCreator; }
            set
            {
                peopleCreator = value;
                OnPropertyChanged("PeopleCreator");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        #endregion

        #region Constructor

        public LogEntriesByActionItem()
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
            //return this.MemberwiseClone();

            LogEntriesByActionItem logEntriesByActionItem = (LogEntriesByActionItem)this.MemberwiseClone();

            if (logEntriesByActionItem.PeopleCreator != null)
                logEntriesByActionItem.PeopleCreator = (People)this.PeopleCreator.Clone();

            return logEntriesByActionItem;
        }

        #endregion
    }
}
