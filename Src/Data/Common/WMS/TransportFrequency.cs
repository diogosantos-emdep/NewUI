using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    //[nsatpute][18.11.2025][GEOS2-9364]    
    [DataContract]
    public class TransportFrequency : ModelBase, IDisposable, IEquatable<TransportFrequency>
    {
        #region Private Members
        private int local;
        private int ground;
        private int sea;
        private int air;
        private int idCompany;
        private string name;
        private string countryName;
        private bool isSelected;
        private string countryIconUrl;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public string CountryName
        {
            get { return countryName; }
            set
            {
                countryName = value;
                OnPropertyChanged("CountryName");
            }
        }

        [NotMapped]
        [DataMember]
        public int Local
        {
            get { return local; }
            set
            {
                local = value;
                OnPropertyChanged("Local");
            }
        }

        [NotMapped]
        [DataMember]
        public int Ground
        {
            get { return ground; }
            set
            {
                ground = value;
                OnPropertyChanged("Ground");
            }
        }

        [NotMapped]
        [DataMember]
        public int Sea
        {
            get { return sea; }
            set
            {
                sea = value;
                OnPropertyChanged("Sea");
            }
        }

        [NotMapped]
        [DataMember]
        public int Air
        {
            get { return air; }
            set
            {
                air = value;
                OnPropertyChanged("Air");
            }
        }    
        [NotMapped]
        [DataMember]
        public string CountryIconUrl
        {
            get { return countryIconUrl; }
            set
            {
                countryIconUrl = value;
                OnPropertyChanged("CountryIconUrl");
            }
        }
        #endregion

        #region Equality Implementation
        public override bool Equals(object obj)
        {
            return Equals(obj as TransportFrequency);
        }

        public bool Equals(TransportFrequency other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return IdCompany == other.IdCompany &&
                   Name == other.Name &&
                   CountryName == other.CountryName &&
                   Local == other.Local &&
                   Ground == other.Ground &&
                   Sea == other.Sea &&
                   Air == other.Air;     
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + IdCompany.GetHashCode();
                hash = hash * 23 + (Name?.GetHashCode() ?? 0);
                hash = hash * 23 + (CountryName?.GetHashCode() ?? 0);
                hash = hash * 23 + Local.GetHashCode();
                hash = hash * 23 + Ground.GetHashCode();
                hash = hash * 23 + Sea.GetHashCode();
                hash = hash * 23 + Air.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(TransportFrequency left, TransportFrequency right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(TransportFrequency left, TransportFrequency right)
        {
            return !(left == right);
        }
        #endregion

        #region Cloning
        public override object Clone()
        {
            return new TransportFrequency
            {
                IdCompany = this.IdCompany,
                Name = this.Name,
                CountryName = this.CountryName,
                Local = this.Local,
                Ground = this.Ground,
                Sea = this.Sea,
                Air = this.Air,
                IsSelected = this.IsSelected
            };
        }

        // Alternative deep clone method if you prefer explicit naming
        public TransportFrequency DeepClone()
        {
            return (TransportFrequency)this.Clone();
        }
        #endregion

        #region Change Detection Helper
        public List<string> GetChangedProperties(TransportFrequency original)
        {
            var changedProperties = new List<string>();

            if (IdCompany != original.IdCompany) changedProperties.Add(nameof(IdCompany));
            if (Name != original.Name) changedProperties.Add(nameof(Name));
            if (CountryName != original.CountryName) changedProperties.Add(nameof(CountryName));
            if (Local != original.Local) changedProperties.Add(nameof(Local));
            if (Ground != original.Ground) changedProperties.Add(nameof(Ground));
            if (Sea != original.Sea) changedProperties.Add(nameof(Sea));
            if (Air != original.Air) changedProperties.Add(nameof(Air));
            if (IsSelected != original.IsSelected) changedProperties.Add(nameof(IsSelected));

            return changedProperties;
        }

        public bool HasChanges(TransportFrequency original)
        {
            return !this.Equals(original) || IsSelected != original.IsSelected;
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return $"TransportFrequency: {Name} (Company: {IdCompany}, Country: {CountryName})";
        }
    }
}
