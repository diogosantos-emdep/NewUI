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
    [Table("agencies")]
    [DataContract]
    public class TransportAgency : ModelBase, IDisposable
    {
        #region Fields
        Int32 idAgency;
        string name;
        sbyte isObsolete;
        string telephone;
        string city;
        string address;
        string cif;
        #endregion

        #region Constructor
        public TransportAgency()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdAgency")]
        [DataMember]
        public Int32 IdAgency
        {
            get
            {
                return idAgency;
            }

            set
            {
                idAgency = value;
                OnPropertyChanged("IdAgency");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("IsObsolete")]
        [DataMember]
        public sbyte IsObsolete
        {
            get
            {
                return isObsolete;
            }

            set
            {
                isObsolete = value;
                OnPropertyChanged("IsObsolete");
            }
        }


        [Column("Telephone")]
        [DataMember]
        public string Telephone
        {
            get
            {
                return telephone;
            }

            set
            {
                telephone = value;
                OnPropertyChanged("Telephone");
            }
        }


        [Column("City")]
        [DataMember]
        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }


        [Column("Address")]
        [DataMember]
        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }


        [Column("CIF")]
        [DataMember]
        public string CIF
        {
            get
            {
                return cif;
            }

            set
            {
                cif = value;
                OnPropertyChanged("CIF");
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
