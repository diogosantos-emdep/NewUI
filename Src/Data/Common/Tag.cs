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
    [Table("tags")]
    [DataContract]
    public class Tag : ModelBase, IDisposable
    {
        #region Fields
        #region chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        Int64 idActionTag;
        Int64 idActionPlanItem;
        Int64 idTag;
        bool isDeleted;
        #endregion
        string name;
        DateTime createdIn;
        Int32 createdBy;

        #endregion

        #region Constructor
        public Tag()
        {

        }
        #endregion

        #region Properties

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

        [Column("CreatedIn")]
        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("CreatedBy")]
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

        #region chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:

        [Key]
        [Column("IdActionTag")]
        [DataMember]
        public Int64 IdActionTag
        {
            get { return idActionTag; }
            set
            {
                idActionTag = value;
                OnPropertyChanged("IdActionTag");
            }
        }

        [Column("IdActionPlanItem")]
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
