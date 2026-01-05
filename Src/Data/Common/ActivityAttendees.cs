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
    [Table("activity_attendees")]
    [DataContract]
    public class ActivityAttendees : ModelBase, IDisposable
    {
        #region Fields
        Int64 idActivityAttendees;
        Int64 idActivity;
        Int32 idUser;
        bool isDeleted;
        People people;
        #endregion

        #region Constructor
        public ActivityAttendees()
        {
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdActivityAttendees")]
        [DataMember]
        public Int64 IdActivityAttendees
        {
            get
            {
                return idActivityAttendees;
            }

            set
            {
                idActivityAttendees = value;
                OnPropertyChanged("IdActivityAttendees");
            }
        }


        [Column("IdActivity")]
        [DataMember]
        public Int64 IdActivity
        {
            get
            {
                return idActivity;
            }

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
            get
            {
                return idUser;
            }

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
            get
            {
                return isDeleted;
            }

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
