using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ConnectorLocation : ModelBase, IDisposable
    {
        #region Fields
        private int quantity;
        private string location;
        private string name;
        private bool isDamaged;
        private string refSolidWorks;
        private string creatorName;
        private string creatorSurname;
        private string creatorSitename;
        private DateTime createdIn;
        private string modifierName;
        private string modifierSurname;
        private string modifierSitename;
        private DateTime modifiedIn;
        private int idsite;
        public int idLocationByConnector;
        private string shortName;
        private int quantityWithoutWires;      
        private int quantityModified;
        private string countryName;
        private int quantityWithoutWiresModified;
        private string countryIconUrl;
        private Color color;     
        private ObservableCollection<Color> colorList;
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        //[rushikesh.gaikwad][GEOS2-5752][14.08.2024]
        public Int32 creatorId;
        public bool isDelVisible;
        public bool isEditVisible; 
        #endregion

        #region Properties
        [DataMember]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged("Quantity"); }
        }

        [DataMember]
        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged("Location"); }
        }
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        [DataMember]
        public bool IsDamaged
        {
            get { return isDamaged; }
            set { isDamaged = value; OnPropertyChanged("IsDamaged"); }
        }
        [DataMember]
        public string RefSolidWorks
        {
            get { return refSolidWorks; }
            set { refSolidWorks = value; OnPropertyChanged("RefSolidWorks"); }
        }

        [DataMember]
        public string CreatorName
        {
            get { return creatorName; }
            set { creatorName = value; OnPropertyChanged("CreatorName"); }
        }

        [DataMember]
        public string CreatorSurname
        {
            get { return creatorSurname; }
            set { creatorSurname = value; OnPropertyChanged("CreatorSurname"); }
        }

        [DataMember]
        public string CreatorSitename
        {
            get { return creatorSitename; }
            set { creatorSitename = value; OnPropertyChanged("CreatorSitename"); }
        }

        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set { createdIn = value; OnPropertyChanged("CreatedIn"); }
        }

        [DataMember]
        public string ModifierName
        {
            get { return modifierName; }
            set { modifierName = value; OnPropertyChanged("ModifierName"); }
        }

        [DataMember]
        public string ModifierSurname
        {
            get { return modifierSurname; }
            set { modifierSurname = value; OnPropertyChanged("ModifierSurname"); }
        }

        [DataMember]
        public string ModifierSitename
        {
            get { return modifierSitename; }
            set { modifierSitename = value; OnPropertyChanged("ModifierSitename"); }
        }

        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set { modifiedIn = value; OnPropertyChanged("ModifiedIn"); }
        }

        [DataMember]
        public int Idsite
        {
            get { return idsite; }
            set { idsite = value; OnPropertyChanged("Idsite"); }
        }

        [DataMember]
        public int IdLocationByConnector
        {
            get { return idLocationByConnector; }
            set { idLocationByConnector = value; OnPropertyChanged("IdLocationByConnector"); }
        }

        [DataMember]
        public string ShortName
        {
            get { return shortName; }
            set { shortName = value; OnPropertyChanged("ShortName"); }
        }

        [DataMember]
        public int QuantityWithoutWires
        {
            get { return quantityWithoutWires; }
            set { quantityWithoutWires = value; OnPropertyChanged("QuantityWithoutWires"); }
        }

        [DataMember]
        public int QuantityModified
        {
            get
            {
                return quantityModified;
            }
            set
            {
                quantityModified = value;
                OnPropertyChanged("QuantityModified");
            }
        }

        [DataMember]
        public int QuantityWithoutWiresModified
        {
            get { return quantityWithoutWiresModified; }
            set { quantityWithoutWiresModified = value; OnPropertyChanged("QuantityWithoutWiresModified"); }
        }

        [DataMember]
        public string CountryName
        {
            get { return countryName; }
            set { countryName = value; OnPropertyChanged("CountryName"); }
        }

        [DataMember]
        public string CountryIconUrl
        {
            get { return countryIconUrl; }
            set { countryIconUrl = value; OnPropertyChanged("CountryIconUrl"); }
        }

        [DataMember]
        public Int32 CreatorId
        {
            get { return creatorId; }
            set { creatorId = value; OnPropertyChanged("CreatorId"); }
        }
        [DataMember]
        public bool IsDelVisible
        {
            get { return isDelVisible; }
            set { isDelVisible = value; OnPropertyChanged("IsDelVisible"); }
        }
        [DataMember]
        public bool IsEditVisible
        {
            get { return isEditVisible; }
            set { isEditVisible = value; OnPropertyChanged("IsEditVisible"); }
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
