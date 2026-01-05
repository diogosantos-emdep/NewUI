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
    [Table("offer_contacts")]
    [DataContract]
    public class OfferContact : ModelBase, IDisposable
    {
        #region Fields
        Int64 idOfferContact;
        Int64 idOffer;
        Int32 idContact;
        People people;
        Company site;
        Int32 idUser;
        byte? isPrimaryOfferContact;
        bool isDeleted;
        #endregion

        #region Constructor
        public OfferContact()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdOfferContact")]
        [DataMember]
        public Int64 IdOfferContact
        {
            get
            {
                return idOfferContact;
            }

            set
            {
                idOfferContact = value;
                OnPropertyChanged("IdOfferContact");
            }
        }

        [Column("IdOffer")]
        [DataMember]
        public Int64 IdOffer
        {
            get
            {
                return idOffer;
            }

            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [Column("IdContact")]
        [DataMember]
        public Int32 IdContact
        {
            get
            {
                return idContact;
            }

            set
            {
                idContact = value;
                OnPropertyChanged("IdContact");
            }
        }

        [Column("IsPrimaryOfferContact")]
        [DataMember]
        public byte? IsPrimaryOfferContact
        {
            get
            {
                return isPrimaryOfferContact;
            }

            set
            {
                isPrimaryOfferContact = value;
                OnPropertyChanged("IsPrimaryOfferContact");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public Company Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
                OnPropertyChanged("Site");
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
