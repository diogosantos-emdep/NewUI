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
    [Table("activity_templates")]
    [DataContract]
    public class ActivityTemplate : ModelBase, IDisposable
    {
        #region Fields

        Int16 idActivityTemplate;
        Int16 idActivityType;
        string subject;
        string description;
        Int16 dueDaysAfterCreation;
        LookupValue activityType;

        #endregion

        #region Constructor

        public ActivityTemplate()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdActivityTemplate")]
        [DataMember]
        public Int16 IdActivityTemplate
        {
            get { return idActivityTemplate; }
            set
            {
                idActivityTemplate = value;
                OnPropertyChanged("IdActivityTemplate");
            }
        }

        [Column("IdActivityType")]
        [DataMember]
        public Int16 IdActivityType
        {
            get { return idActivityType; }
            set
            {
                idActivityType = value;
                OnPropertyChanged("IdActivityType");
            }
        }

        [Column("Subject")]
        [DataMember]
        public string Subject
        {
            get { return subject; }
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
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("DueDaysAfterCreation")]
        [DataMember]
        public Int16 DueDaysAfterCreation
        {
            get { return dueDaysAfterCreation; }
            set
            {
                dueDaysAfterCreation = value;
                OnPropertyChanged("DueDaysAfterCreation");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue ActivityType
        {
            get { return activityType; }
            set
            {
                activityType = value;
                OnPropertyChanged("ActivityType");
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
