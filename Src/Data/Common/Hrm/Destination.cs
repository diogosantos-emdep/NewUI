using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{//[Sudhir.jangra][GEOS2-4816]
    public class Destination : ModelBase, IDisposable
    {
        #region Fields
        Int32 idDestination;
        string name;
        string alias;
        string shortName;
        byte? idCountry;
        byte isLocation;
        byte isOrganization;
        byte isCompany;
        Byte? isStillActive;
        //[rgadhave][GEOS2-6385][27-09-2024]
        Int32 idCurrency;
        #endregion

        #region Properties
        [DataMember]
        public Int32 IdDestination
        {
            get { return idDestination; }
            set
            {
                idDestination = value;
                OnPropertyChanged("IdDestination");
            }
        }
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
        [Column("Alias")]
        [DataMember]
        public string Alias
        {
            get { return alias; }
            set
            {
                alias = value;
                OnPropertyChanged("Alias");
            }
        }

        [NotMapped]
        [DataMember]
        public string ShortName
        {
            get { return shortName; }
            set
            {
                shortName = value;
                OnPropertyChanged("ShortName");
            }
        }

        [Column("IdCountry")]
        [ForeignKey("Country")]
        [DataMember]
        public byte? IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [DataMember]
        public virtual Country Country { get; set; }

        [Column("IsLocation")]
        [DataMember]
        public byte IsLocation
        {
            get { return isLocation; }
            set
            {
                isLocation = value;
                OnPropertyChanged("IsLocation");
            }
        }

        [Column("IsOrganization")]
        [DataMember]
        public byte IsOrganization
        {
            get { return isOrganization; }
            set
            {
                isOrganization = value;
                OnPropertyChanged("IsOrganization");
            }
        }

        [Column("IsCompany")]
        [DataMember]
        public byte IsCompany
        {
            get { return isCompany; }
            set
            {
                isCompany = value;
                OnPropertyChanged("IsCompany");
            }
        }

        [Column("IsStillActive")]
        [DataMember]
        public Byte? IsStillActive
        {
            get { return isStillActive; }
            set
            {
                isStillActive = value;
                OnPropertyChanged("IsStillActive");
            }
        }
        //[rgadhave][GEOS2-6385][27-09-2024]
        [DataMember]
        public Int32 IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }
        #endregion

        #region Constructor
        public Destination()
        {

        }
        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
