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
    [Table("Activity_template_trigger_conditions")]
    [DataContract]
    public class ActivityTemplateTriggerCondition : ModelBase, IDisposable
    {
        #region Fields

        Int16 idActivityTemplateTriggerCondition;
        Int16 idActivityTemplateTrigger;
        string conditionFieldName;
        string conditionOperator;
        string conditionFieldValue;
        string conditionFieldType;
        byte isUserConfirmationRequired;

        #endregion

        #region Constructor

        public ActivityTemplateTriggerCondition()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdActivityTemplateTriggerCondition")]
        [DataMember]
        public Int16 IdActivityTemplateTriggerCondition
        {
            get { return idActivityTemplateTriggerCondition; }
            set
            {
                idActivityTemplateTriggerCondition = value;
                OnPropertyChanged("IdActivityTemplateTriggerCondition");
            }
        }

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

        [Column("ConditionFieldName")]
        [DataMember]
        public string ConditionFieldName
        {
            get { return conditionFieldName; }
            set
            {
                conditionFieldName = value;
                OnPropertyChanged("ConditionFieldName");
            }
        }

        [Column("ConditionOperator")]
        [DataMember]
        public string ConditionOperator
        {
            get { return conditionOperator; }
            set
            {
                conditionOperator = value;
                OnPropertyChanged("ConditionOperator");
            }
        }

        [Column("ConditionFieldValue")]
        [DataMember]
        public string ConditionFieldValue
        {
            get { return conditionFieldValue; }
            set
            {
                conditionFieldValue = value;
                OnPropertyChanged("ConditionFieldValue");
            }
        }

        [Column("ConditionFieldType")]
        [DataMember]
        public string ConditionFieldType
        {
            get { return conditionFieldType; }
            set
            {
                conditionFieldType = value;
                OnPropertyChanged("ConditionFieldType");
            }
        }

        [Column("IsUserConfirmationRequired")]
        [DataMember]
        public byte IsUserConfirmationRequired
        {
            get { return isUserConfirmationRequired; }
            set
            {
                isUserConfirmationRequired = value;
                OnPropertyChanged("IsUserConfirmationRequired");
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
