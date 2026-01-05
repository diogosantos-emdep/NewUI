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
    [Table("activity_template_triggers")]
    [DataContract]
    public class ActivityTemplateTrigger : ModelBase, IDisposable
    {
        #region Fields

        Int16 idActivityTemplateTrigger;
        Int16 idLinkedObject;
        LookupValue linkedObject;
        string linkedObjectFieldName;
        string linkedObjectFieldValue;
        ActivityTemplateTriggerCondition activityTemplateTriggerCondition;
        List<ActivityTemplate> activityTemplates;


        #endregion

        #region Constructor

        public ActivityTemplateTrigger()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdActivityTemplateTrigger")]
        [DataMember]
        public Int16 IdActivityTemplateTrigger
        {
            get { return idActivityTemplateTrigger; }
            set
            {
                idActivityTemplateTrigger = value;
                OnPropertyChanged("IdActivityTemplateTrigger");
            }
        }

        [Column("IdLinkedObject")]
        [DataMember]
        public Int16 IdLinkedObject
        {
            get { return idLinkedObject; }
            set
            {
                idLinkedObject = value;
                OnPropertyChanged("IdLinkedObject");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue LinkedObject
        {
            get { return linkedObject; }
            set
            {
                linkedObject = value;
                OnPropertyChanged("LinkedObject");
            }
        }

        [Column("LinkedObjectFieldName")]
        [DataMember]
        public string LinkedObjectFieldName
        {
            get { return linkedObjectFieldName; }
            set
            {
                linkedObjectFieldName = value;
                OnPropertyChanged("LinkedObjectFieldName");
            }
        }

        [Column("LinkedObjectFieldValue")]
        [DataMember]
        public string LinkedObjectFieldValue
        {
            get { return linkedObjectFieldValue; }
            set
            {
                linkedObjectFieldValue = value;
                OnPropertyChanged("LinkedObjectFieldValue");
            }
        }

        [NotMapped]
        [DataMember]
        public ActivityTemplateTriggerCondition ActivityTemplateTriggerCondition
        {
            get { return activityTemplateTriggerCondition; }
            set
            {
                activityTemplateTriggerCondition = value;
                OnPropertyChanged("ActivityTemplateTriggerCondition");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ActivityTemplate> ActivityTemplates
        {
            get { return activityTemplates; }
            set
            {
                activityTemplates = value;
                OnPropertyChanged("ActivityTemplates");
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
            return this.MemberwiseClone();
        }

        #endregion
    }
}
