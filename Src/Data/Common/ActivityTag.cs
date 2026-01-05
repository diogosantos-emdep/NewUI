using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("activity_tags")]
    [DataContract]
    public class ActivityTag : ModelBase, IDisposable
    {
        #region Fields

        Int64 idActivityTag;
        Int64 idActivity;
        Int64 idTag;
        Activity activity;
        Tag tag;
        bool isDeleted;

        #endregion

        #region Constructor
        public ActivityTag()
        {

        }
        #endregion

        #region Properties

        [Key]
        [Column("IdActivityTag")]
        [DataMember]
        public Int64 IdActivityTag
        {
            get { return idActivityTag; }
            set
            {
                idActivityTag = value;
                OnPropertyChanged("IdActivityTag");
            }
        }

        [Column("IdActivity")]
        [DataMember]
        public Int64 IdActivity
        {
            get { return idActivity; }
            set
            {
                idActivity = value;
                OnPropertyChanged("IdActivity");
            }
        }

        [Column("IdTag")]
        [DataMember]
        public Int64 IdTag
        {
            get { return idTag; }
            set
            {
                idTag = value;
                OnPropertyChanged("IdTag");
            }
        }

        [NotMapped]
        [DataMember]
        public Activity Activity
        {
            get { return activity; }
            set
            {
                activity = value;
                OnPropertyChanged("Activity");
            }
        }

        [NotMapped]
        [DataMember]
        public Tag Tag
        {
            get { return tag; }
            set
            {
                tag = value;
                OnPropertyChanged("Tag");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
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
