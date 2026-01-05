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
    [Table("activity_watchers")]
    [DataContract]
    public class ActivityWatcher : ModelBase, IDisposable
    {
        #region Fields

        Int64 idActivityWatcher;
        Int64 idActivity;
        Int32 idUser;
        bool isDeleted;
        People people;

        #endregion

        #region Constructor

        public ActivityWatcher()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdActivityWatcher")]
        [DataMember]
        public Int64 IdActivityWatcher
        {
            get { return idActivityWatcher; }
            set
            {
                idActivityWatcher = value;
                OnPropertyChanged("IdActivityWatcher");
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

        [Column("IdUser")]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
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

        [NotMapped]
        [DataMember]
        public People People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged("People");
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
